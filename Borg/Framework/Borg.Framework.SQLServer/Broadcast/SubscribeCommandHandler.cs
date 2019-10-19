using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;

        protected SubscribeCommandHandler(string connectionString)
        {
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(connectionString, nameof(connectionString)));
        }

        public async Task<Unit> Handle(SubscribeCommand request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var args = request;
            if (args != null)
            {
                var command = new SqlCommand("[broadcast].[Subscribe]", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@queuesubscriber", args.SubscriberName);
                command.Parameters.AddWithValue("@queues", string.Join(";", args.QueueNames));
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