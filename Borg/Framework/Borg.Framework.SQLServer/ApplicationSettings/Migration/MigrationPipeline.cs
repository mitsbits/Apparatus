//using Borg.Framework.Modularity.Pipelines;
//using Borg.Framework.Services.Configuration;
//using Borg.Infrastructure.Core;
//using MediatR;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
//{
//    public class MigrationPipeline : IHostStartUpJob
//    {
//        private readonly SqlApplicationSettingConfig config;
//        private readonly ILogger logger;
//        private readonly IMediator dispatcher;
//        public int SchemaVersion = 1;

//        public MigrationPipeline(ILoggerFactory loggerFactory, IMediator dispatcher, IConfiguration configuration)
//        {
//            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            config = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
//            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
//        }

//        public event EventHandler<ExecutorEventArgs> OnExecuting;

//        public event EventHandler<ExecutorEventArgs> OnExecuted;

//        public async Task Execute(CancellationToken cancelationToken)
//        {
//            var schemaCommand = new CheckForSchemaCommand(SchemaVersion);
//            var schemaResult = await dispatcher.Send(schemaCommand);
//            if (schemaResult.IsNewerThanDatabase)
//            {
//                await dispatcher.Send(new RunMigrationCommand(SchemaVersion));
//            }
//        }
//    }
//}