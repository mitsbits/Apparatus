using Borg.Framework.Cms.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Framework.ModelStores
{
    public class ModelStoreExplorer : AssemblyExplorer
    {
        private readonly List<AssemblyScanResult> results = new List<AssemblyScanResult>();
        private readonly ILoggerFactory loggerFactory;

        public ModelStoreExplorer(ILoggerFactory loggerfactory, IEnumerable<IAssemblyProvider> providers) : base(loggerfactory)
        {
            this.loggerFactory = loggerfactory;
            Populate(Preconditions.NotEmpty(providers, nameof(providers)));
            AsyncHelpers.RunSync(async () => await ScanInternal());
        }

        protected override IEnumerable<AssemblyScanResult> ResultsInternal()
        {
            return results;
        }

        protected override async Task ScanInternal()
        {
            foreach (var asml in assemblies)
            {
                results.Add(await ScanInternal(asml));
            }
            scanCompleted = true;
        }

        private async Task<AssemblyScanResult> ScanInternal(Assembly asmbl)
        {
            return await new ModelStoreAssemblyScanner(asmbl, loggerFactory).Scan();
        }

        private void Populate(IEnumerable<IAssemblyProvider> providers)
        {
            foreach (var asmbl in providers.SelectMany(p => p.GetAssemblies())
                .Where(asmbl => asmbl.Assimilated()
                    && !assemblies.Any(x => x.FullName == asmbl.FullName)))
            {
                Logger.Info($"Discoverd assembly for the hive - {asmbl.FullName}");
                assemblies.Add(asmbl);
            }
        }
    }
}