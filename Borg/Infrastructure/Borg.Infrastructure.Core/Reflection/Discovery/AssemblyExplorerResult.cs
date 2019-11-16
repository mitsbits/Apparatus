using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
                results.AddRange(explorer.ScanAndResult());
            }
        }

        public IEnumerable<AssemblyScanResult> Results => results;
    }
}