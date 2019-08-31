using Borg.Framework.Modularity.Pipelines;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.Initializers
{
   
    public class ExecuteSql<TDbContext, TPipeline> : BorgDbPipelineStep<TDbContext>, IPipelineStep<TPipeline> where TDbContext : BorgDbContext where TPipeline : HostStartUpJob<TDbContext>
    {
        protected string SqlText;
        private readonly string command;
        private readonly object[] parameters;

        protected ExecuteSql(TDbContext db, ILoggerFactory loggerFactory, string command, params object[] parameters) : base(db, loggerFactory)
        {
            var t = typeof(IPipelineStep<HostStartUpJob<TDbContext>>);
            SqlText = Preconditions.NotEmpty(command, nameof(command));
            this.command = Preconditions.NotEmpty(command, nameof(command));
            db.Database.ExecuteSqlCommandAsync(SqlText, default, parameters);
        }

        public override double Weight { get; set; } = 10;

        protected override async Task ExecuteInternal(CancellationToken cancelationToken)
        {
            cancelationToken.ThrowIfCancellationRequested();
            try
            {
                Logger.Debug($"About to Execute SQL");
                await DB.Database.ExecuteSqlCommandAsync(command, cancelationToken, parameters);
            }
            catch (Exception ex)
            {
                Logger.Error($"Unable to execute comamand - reason : {ex}");
                throw;
            }
        }
    }
}