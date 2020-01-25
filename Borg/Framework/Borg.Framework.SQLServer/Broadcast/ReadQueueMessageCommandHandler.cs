using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using Borg.Framework.Dispatch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class ReadQueueMessageCommandHandler : IRequestHandler<ReadQueueMessageCommand, ReadQueueMessageCommandResult>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;
        protected readonly ILogger logger;

        public ReadQueueMessageCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            var options = Configurator<SqlBroadcastBusConfig>.Build( configuration, SqlBroadcastBusConfig.Key);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
        }

        public async Task<ReadQueueMessageCommandResult> Handle(ReadQueueMessageCommand request, CancellationToken cancellationToken)
        {
            var command = new SqlCommand("[broadcast].[SubscriberReadQueueMessage]", sqlConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@queuename", request.QueueName);
            command.Parameters.AddWithValue("@queuesubscriber", request.SubscriberName);
            if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
            DateTimeOffset createdOn = DateTimeOffset.MinValue;
            string payloadType = string.Empty;
            string payload = string.Empty;
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    createdOn = reader.GetDateTimeOffset(0);
                    payloadType = reader.GetString(1);
                    payload = reader.GetString(2);
                }
                reader.Close();
            }

            return new ReadQueueMessageCommandResult(createdOn, payloadType, payload);
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