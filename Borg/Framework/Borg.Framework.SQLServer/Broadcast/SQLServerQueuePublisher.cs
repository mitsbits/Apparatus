using Borg.Infra.Messaging;
using Borg.Infrastructure.Core;
using System;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    internal sealed class SQLServerQueuePublisher : SQLServerQueuePublisherBase, IQueuePublisher, IDisposable
    {
        public SQLServerQueuePublisher(string connectionString, string queueName) : base(connectionString, queueName)
        {
        }

        public async Task Publish(Type messageType, object message, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await PlaceMessageToQueue(messageType, message);
        }
    }

    internal class SQLServerQueuePublisherBase : IDisposable
    {
        protected readonly SqlConnection sqlConnection;

        protected SQLServerQueuePublisherBase(string connectionString, string queueName)
        {
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(connectionString, nameof(connectionString)));
            QueueName = Preconditions.NotEmpty(queueName, nameof(queueName));
        }

        public string QueueName { get; protected set; }

        protected async Task PlaceMessageToQueue(Type messageType, object message)
        {
            var command = new SqlCommand("[broadcast].[PublisherAddQueueMessage]", sqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@queuename", QueueName);
            command.Parameters.AddWithValue("@payloadType", messageType.FullName);
            command.Parameters.AddWithValue("@payload", JsonSerializer.Serialize(message));
            if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
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