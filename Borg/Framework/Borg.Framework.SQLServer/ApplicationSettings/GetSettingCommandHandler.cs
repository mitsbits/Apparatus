using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class GetSettingCommandHandler : IRequestHandler<GetSettingCommand, PaylodCommandResult>, IDisposable

    {
        protected readonly SqlConnection sqlConnection;
        protected readonly ILogger logger;
        protected readonly SqlApplicationSettingConfig options;

        public GetSettingCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            options = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
        }

        public async Task<PaylodCommandResult> Handle(GetSettingCommand request, CancellationToken cancellationToken)
        {
            string statement = PrepareStatement();
            string jsonData = "{}";

            using (var command = new SqlCommand(statement, sqlConnection))
            {
                command.Parameters.AddWithValue("@typeName", request.SettingType.FullName);
                command.CommandType = System.Data.CommandType.Text;
                if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        if (await reader.ReadAsync())
                        {
                            jsonData = reader.GetString(0);
                        }
                    }
                }
            }
            var payload = JsonSerializer.Deserialize(jsonData, request.SettingType);
            return new PaylodCommandResult(request.SettingType, payload);
        }

        private string PrepareStatement()
        {
            return $"SELECT TOP 1 [Payload] FROM [{options.Schema}].[{options.Table}] WHERE [TypeName] = @typeName";
        }

        public void Dispose()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}