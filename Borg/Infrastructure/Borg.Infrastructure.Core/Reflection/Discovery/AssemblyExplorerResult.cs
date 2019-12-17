using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public class AssemblyExplorerResult : IAssemblyExplorerResult
    {
        private ILogger Logger { get; }

        private readonly List<AssemblyScanResult> results = new List<AssemblyScanResult>();

        public AssemblyExplorerResult(ILoggerFactory loggerFactory, IEnumerable<IAssemblyExplorer> explorers)
        {
            Logger = loggerFactory.CreateForType(GetType());
            Populate(Preconditions.NotEmpty(explorers, nameof(explorers)));
        }

        private void Populate(IEnumerable<IAssemblyExplorer> explorers)
        {
            foreach (var explorer in explorers)
            {
                Logger.Debug($"Exploring {explorer.GetType().Name}");
                var result = explorer.ScanAndResult().Where(x => x.Success);
                foreach (var r in result)
                {
                    if (!result.Contains(r))
                    {
                        results.Add(r);
                    }
                }
            }
        }

        public IEnumerable<AssemblyScanResult> Results => results;
    }
}