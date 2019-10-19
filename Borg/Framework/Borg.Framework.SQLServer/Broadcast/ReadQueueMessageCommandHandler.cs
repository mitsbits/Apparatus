using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class ReadQueueMessageCommandHandler : IRequestHandler<ReadQueueMessageCommand, ReadQueueMessageCommandResult>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;

        protected ReadQueueMessageCommandHandler(string connectionString)
        {
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(connectionString, nameof(connectionString)));
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
            DateTimeOffset createdOn;
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