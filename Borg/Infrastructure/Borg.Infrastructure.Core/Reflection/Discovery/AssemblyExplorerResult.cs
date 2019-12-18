using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public class AssemblyExplorerResult : IAssemblyExplorerResult
    {
        private ILogger Logger { get; }

        private readonly List<AssemblyScanResult> results = new List<AssemblyScanResult>();
        private readonly List<AssemblyScanResult> resultsWithErrors = new List<AssemblyScanResult>();

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
                var result = explorer.ScanAndResult();
                foreach (var r in result)
                {
                    if (r.Success)
                    {
                        if (!results.Contains(r))
                        {
                            results.Add(r);
                        }
                    }
                    else
                    {
                        if (!resultsWithErrors.Contains(r))
                        {
                            resultsWithErrors.Add(r);
                        }
                    }
                }
            }
        }

        public IEnumerable<AssemblyScanResult> Results => results;
    }
}