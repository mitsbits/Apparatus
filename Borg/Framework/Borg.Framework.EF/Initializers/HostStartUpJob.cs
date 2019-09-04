using Borg.Framework.Modularity.Pipelines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Borg.Framework.EF.Initializers
{
    public abstract class HostStartUpJob<TDbContext> : GenericHostStartUp where TDbContext : BorgDbContext
    {
        public HostStartUpJob(TDbContext dbContext, IEnumerable<IPipelineStep<HostStartUpJob<TDbContext>>> steps, ILoggerFactory loggerFactory = null)
        {
            foreach (var step in steps)
            {
                source.Add(step);
            }
        }
    }
}