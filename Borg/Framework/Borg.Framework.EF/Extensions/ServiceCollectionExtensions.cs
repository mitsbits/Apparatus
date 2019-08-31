using Borg;
using Borg.Framework.DAL;
using Borg.Framework.DAL.Inventories;
using Borg.Framework.EF.DAL;
using Borg.Framework.EF.DAL.Inventories;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    //https://forums.asp.net/t/1866727.aspx?How+do+i+get+a+collection+of+class+names+from+DbContext+
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<TDbContext, T>(this IServiceCollection services) where TDbContext : DbContext where T : class
        {
            return AddRepository<TDbContext>(services, typeof(T));
        }

        private static IServiceCollection AddRepository<TDbContext>(IServiceCollection services, Type type) where TDbContext : DbContext
        {
            var dbType = typeof(TDbContext);
            var qryType = typeof(QueryRepository<,>);
            var instType = qryType.MakeGenericType(type, dbType);
            services.AddScoped(typeof(IQueryRepository<>).MakeGenericType(type), instType);
            return services;
        }

        public static IServiceCollection AddGenericInventories(this IServiceCollection services, IAssemblyExplorerResult explorerResult)
        {
            var localResults = explorerResult.Results<EntitiesAssemblyScanResult>(x => x.Success);
            var types = localResults.SelectMany(x => x.AllEntityTypes()).Distinct().ToArray();
            var db = localResults.SelectMany(x => x.DefaultDbs).First();
            foreach (var t in types)
            {
                var contract = typeof(IInventoryFacade<>).MakeGenericType(t);
                var service = typeof(Inventory<,>).MakeGenericType(t, db);
                services.Add(new ServiceDescriptor(contract, service, ServiceLifetime.Scoped));
            }

            return services;
        }
    }
}