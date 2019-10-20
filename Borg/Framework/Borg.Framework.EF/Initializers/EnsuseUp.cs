//using Borg.Framework.Modularity.Pipelines;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.EF.Initializers
//{
//    public class EnsuseUp<TDbContext, TPipeline> : BorgDbPipelineStep<TDbContext>, IPipelineStep<TPipeline> where TDbContext : BorgDbContext where TPipeline : HostStartUpJob<TDbContext>
//    {
//        protected EnsuseUp(TDbContext db, ILoggerFactory loggerFactory) : base(db, loggerFactory)
//        {
//        }

//        public override double Weight { get => 0; set => throw new NotImplementedException(nameof(Weight)); }

//        protected override async Task ExecuteInternal(CancellationToken cancelationToken)
//        {
//            cancelationToken.ThrowIfCancellationRequested();
//            var watch = Stopwatch.StartNew();
//            try
//            {
//                Logger.Debug($"About to migrate {DB.Schema}");
//                await DB.Database.MigrateAsync(cancelationToken);
//                watch.Stop();
//                Logger.Debug($"Migrated database { DB.Schema } and it took {watch.Elapsed}");
//            }
//            catch (Exception ex)
//            {
//                Logger.Error(ex, $"Failed to migrate databe {DB.Schema}");
//                throw;
//            }
//            finally
//            {
//                watch.Stop();
//            }
//        }
//    }
//}