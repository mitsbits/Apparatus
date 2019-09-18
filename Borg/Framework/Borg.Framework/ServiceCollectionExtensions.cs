using Borg.Framework;

//using Borg.Framework.Cache;
using Borg.Framework.Reflection.Discovery;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceProvider AddServiceLocator(this IServiceCollection services)
        {
            services.AddSingleton<ServiceLocator>();
            var locator = services.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(locator);
            return locator;
        }

        public static IServiceCollection AddCacheClient(this IServiceCollection services)
        {
            // services.AddSingleton<ICacheClient, CacheClient>();
            return services;
        }

        public static IServiceCollection AddAssemblyExplorerOrchestrator(this IServiceCollection services)
        {
            services.AddSingleton<IAssemblyExplorerResult, AssemblyExplorerResult>();
            return services;
        }

        public static IServiceCollection AddPlugableServicesExplorer(this IServiceCollection services)
        {
            services.AddSingleton<PlugableServicesExplorer, PlugableServicesExplorer>();
            return services;
        }
    }
}