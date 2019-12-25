using Borg.Framework.EF.Discovery;
using Borg.Framework.EF.Discovery.AssemblyScanner;
using Borg.Framework.Reflection.Services;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Borg.Platform.EF.Data
{
    internal class PlatformDbContextFactory : IDesignTimeDbContextFactory<PlatformDb>
    {
        private const string sqlConnectionString = "Server=localhost;Database=Borg;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public PlatformDb CreateDbContext(string[] args)
        {
            var providers = new IAssemblyProvider[]
                    {
                                  new DepedencyAssemblyProvider(null),
                                  new ReferenceAssemblyProvider(null, null, GetType().Assembly)
                    };
            var dbexplorer = new BorgDbAssemblyExplorer(null, Preconditions.NotEmpty(providers, nameof(providers)));
            var entexplorer = new EntitiesExplorer(null, providers);
            var result = new AssemblyExplorerResult(null, new IAssemblyExplorer[] { dbexplorer, entexplorer });
            var builder = new DbContextOptionsBuilder<PlatformDb>();
            builder.UseSqlServer(sqlConnectionString, (o) =>
            {
            });

            var db = new PlatformDb(builder.Options, result);

            return db;
        }
    }
}