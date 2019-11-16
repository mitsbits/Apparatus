using Borg.Framework.Modularity.Pipelines;
using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
{
    public class MigrationPipeline : IHostStartUpJob
    {
        private readonly SqlApplicationSettingConfiguration config;
        private readonly ILogger logger;
        public int SchemaVersion = 1;

        public MigrationPipeline(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory.CreateForType(GetType());
            config = Configurator<SqlApplicationSettingConfiguration>.Build(configuration, SqlApplicationSettingsConstants.ConfigKey);
        }

        public event EventHandler<ExecutorEventArgs> OnExecuting;

        public event EventHandler<ExecutorEventArgs> OnExecuted;

        public async Task Execute(CancellationToken cancelationToken)
        {
            OnExecuting?.Invoke(this, ExecutorEventArgs.Empty);
            var schemaResult = await CheckForSchema();
            if (schemaResult.IsNewerThanDatabase)
            {
                await RunMigration();
            }
            OnExecuted?.Invoke(this, ExecutorEventArgs.Empty);
        }

        private async Task<CheckForSchemaCommandResult> CheckForSchema(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckForSchemaCommandResult result = default;
            var sqlCommandText = string.Empty;
            using (var stream = GetType().Assembly.GetManifestResourceStream(SqlApplicationSettingsConstants.CheckForVersionResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), SqlApplicationSettingsConstants.CheckForVersionResourcePath);
            }

            sqlCommandText = sqlCommandText
                .Replace("{schema}", SqlApplicationSettingsConstants.Schema)
                .Replace("{version}", SchemaVersion.ToString());

            using (var sqlConnection = new SqlConnection(config.ConnectionString))
            using (var command = new SqlCommand(sqlCommandText, sqlConnection))
            {
                command.CommandType = System.Data.CommandType.Text;
                if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new CheckForSchemaCommandResult(
                                   reader.GetInt32(0),
                                   reader.GetInt32(1));
                        }
                    }
                }
            }
            return result;
        }

        public async Task RunMigration(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sqlCommandText = string.Empty;
            using (var stream = GetType().Assembly.GetManifestResourceStream(SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), SqlApplicationSettingsConstants.CreateDatabaseSchemaResourcePath);
            }

            var replacements = new Dictionary<string, string>() {
                        { "{schema}", config.Schema } ,
                        { "{table}", config.Table },
                        { "{versionNumber}", SchemaVersion.ToString() }
                    };

            using (var sqlConnection = new SqlConnection(config.ConnectionString))
            {
                await sqlConnection.RunBatch(sqlCommandText, replacements, true);
            }
        }
    }
}