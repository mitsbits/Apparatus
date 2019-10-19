using Borg.Infra.Messaging;
using Borg.Infrastructure.Core;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    internal class SQLServerQueueSubscriber : IQueueSubscriber, IDisposable
    {
        private static readonly Lazy<ConcurrentDictionary<string, Type>> _typeCache = new Lazy<ConcurrentDictionary<string, Type>>(() => new ConcurrentDictionary<string, Type>());
        protected readonly SqlConnection sqlConnection;

        public SQLServerQueueSubscriber(string connectionString, string queueName, string subscriberName)
        {
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(connectionString, nameof(connectionString)));
            QueueName = Preconditions.NotEmpty(queueName, nameof(queueName));
            SubscriberName = Preconditions.NotEmpty(subscriberName, nameof(subscriberName));
        }

        private ConcurrentDictionary<string, Type> Cache => _typeCache.Value;
        public string QueueName { get; protected set; }

        public string SubscriberName { get; protected set; }

        public async Task<object> Read()
        {
            var command = new SqlCommand("[broadcast].[SubscriberReadQueueMessage]", sqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@queuename", QueueName);
            command.Parameters.AddWithValue("@queuesubscriber", SubscriberName);
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

            if (!string.IsNullOrWhiteSpace(payloadType) && Cache.TryGetValue(payloadType, out Type type))
            {
                return JsonSerializer.Deserialize(payload, type);
            }

            type = Type.GetType(payloadType);
            Cache.TryAdd(payload, type);
            return JsonSerializer.Deserialize(payload, type);
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