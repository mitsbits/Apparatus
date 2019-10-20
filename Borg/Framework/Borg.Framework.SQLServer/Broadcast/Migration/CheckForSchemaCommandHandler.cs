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

namespace Borg.Framework.SQLServer.Broadcast.Migration
{
    public class CheckForSchemaCommandHandler : IRequestHandler<CheckForSchemaCommand, CheckForSchemaCommandResult>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;
        private string sqlCommandText;
        private readonly ILogger logger;

        public CheckForSchemaCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            var options = Configurator<SqlBroadcastBusConfig>.Build(logger, configuration, SqlBroadcastBusConfig.Key);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
            using (var stream = GetType().Assembly.GetManifestResourceStream(MigrationPipeline.checkVersionResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), MigrationPipeline.checkVersionResourcePath);
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

            using (var command = new SqlCommand(sqlCommandText, sqlConnection))
            {
                command.Parameters.AddWithValue("@version", request.CurrnetSchemaVersion);
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