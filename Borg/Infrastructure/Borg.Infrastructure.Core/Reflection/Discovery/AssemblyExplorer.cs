using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public abstract class AssemblyExplorer : IAssemblyExplorer
    {
        protected ILogger Logger { get; }
        protected bool scanCompleted = false;
        protected readonly Func<Assembly, bool> explorationPredicate ;
        protected readonly List<Assembly> assemblies = new List<Assembly>();

        protected AssemblyExplorer(ILoggerFactory loggerFactory, Func<Assembly, bool> explorationPredicate = null)
        {
            Logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            if (explorationPredicate != null)
            {
                this.explorationPredicate = explorationPredicate;
            }
            else
            {
                explorationPredicate = (a) => a.Assimilated() && !assemblies.Any(x => x.FullName == a.FullName);
            }
        }

        public IEnumerable<AssemblyScanResult> Results()
        {
            return ResultsInternal();
        }

        public bool ScanCompleted => scanCompleted;

        protected abstract IEnumerable<AssemblyScanResult> ResultsInternal();

        public Task Scan() => ScanInternal();

        protected abstract Task ScanInternal();
    }
}