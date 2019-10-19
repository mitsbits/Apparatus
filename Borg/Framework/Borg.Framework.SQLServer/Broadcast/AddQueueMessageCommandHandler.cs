using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class AddQueueMessageCommandHandler : IRequestHandler<AddQueueMessageCommand>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;

        protected AddQueueMessageCommandHandler(string connectionString)
        {
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(connectionString, nameof(connectionString)));
        }

        public async Task<Unit> Handle(AddQueueMessageCommand request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var args = request;
            if (args != null)
            {
                var type = args.Payload.GetType();
                var payload = new StringBuilder();
                using (var stream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(stream, args.Payload, type, cancellationToken: cancellationToken);
                    stream.Position = 0;

                    StreamReader reader = new StreamReader(stream);
                    payload.Append(await reader.ReadToEndAsync());
                }

                var command = new SqlCommand("[broadcast].[PublisherAddQueueMessage]", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@queuename", args.QueueName);
                command.Parameters.AddWithValue("@payloadType", type.FullName);
                command.Parameters.AddWithValue("@payload", payload.ToString());
                if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            return Unit.Value;
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