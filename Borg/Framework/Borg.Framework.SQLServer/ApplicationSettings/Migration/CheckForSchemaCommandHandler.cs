using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
{
    public class CheckForSchemaCommandHandler : IRequestHandler<CheckForSchemaCommand, CheckForSchemaCommandResult>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;
        private string sqlCommandText;
        private readonly ILogger logger;

        public CheckForSchemaCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            var options = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
            using (var stream = GetType().Assembly.GetManifestResourceStream(SqlApplicationSettingsConstants.CheckForVersionResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), SqlApplicationSettingsConstants.CheckForVersionResourcePath);
            }
        }

        public void Dispose()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public async Task<CheckForSchemaCommandResult> Handle(CheckForSchemaCommand request, CancellationToken cancellationToken)
        {
            CheckForSchemaCommandResult result = default;

            sqlCommandText = sqlCommandText
                .Replace("{schema}", SqlApplicationSettingsConstants.Schema)
                .Replace("{version}", request.CurrnetSchemaVersion.ToString());
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
    }
}