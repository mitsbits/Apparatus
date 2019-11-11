//using Borg.Framework.Services.Configuration;
//using Borg.Infrastructure.Core;
//using MediatR;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
//{
//    public class RunMigrationCommandHandler : IRequestHandler<RunMigrationCommand>, IDisposable
//    {
//        protected readonly SqlConnection sqlConnection;
//        private string sqlCommandText;
//        private readonly ILogger logger;
//        private SqlApplicationSettingConfig options;

//        public RunMigrationCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
//        {
//            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            options = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
//            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
//            using (var stream = GetType().Assembly.GetManifestResourceStream(SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath))
//            using (var reader = new StreamReader(stream))
//            {
//                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath);
//            }
//        }

//        public void Dispose()
//        {
//            if (sqlConnection != null)
//            {
//                if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
//                sqlConnection.Dispose();
//            }
//        }

//        public async Task<Unit> Handle(RunMigrationCommand request, CancellationToken cancellationToken)
//        {
//            using (var stream = GetType().Assembly.GetManifestResourceStream(SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath))
//            using (var reader = new StreamReader(stream))
//            {
//                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath);
//            }

//            var replacements = new Dictionary<string, string>() {
//                { "{schema}", options.Schema } ,
//                { "{table}", options.Table },
//                { "{versionNumber}", request.CurrnetSchemaVersion.ToString() }
//            };

//            await sqlConnection.RunBatch(sqlCommandText, replacements, true);
//            return Unit.Value;
//        }
//    }
//}