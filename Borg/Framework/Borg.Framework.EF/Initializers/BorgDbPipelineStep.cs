using Borg.Framework.Modularity.Pipelines;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.Initializers
{
    public abstract class BorgDbPipelineStep<TDbContext> : GenericPipelineStep<HostStartUpJob<TDbContext>> where TDbContext : BorgDbContext
    {
        protected ILogger Logger { get; }
        protected TDbContext DB { get; }

        public BorgDbPipelineStep(TDbContext dbContext, ILoggerFactory loggerFactory = null)
        {
            DB = Preconditions.NotNull(dbContext, nameof(dbContext));
            Logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }



        protected override async Task ExecuteInternal(CancellationToken cancelationToken)
        {
            cancelationToken.ThrowIfCancellationRequested();
            var watch = Stopwatch.StartNew();
            try
            {
                await ExecuteInternal(cancelationToken);
                watch.Stop();
                Logger.Debug($"Migrated database { DB.Schema } and it took {watch.Elapsed}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Failed to migrate databe {DB.Schema}");
                throw;
            }
            finally
            {
                watch.Stop();
            }
        }
    }
}