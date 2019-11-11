using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;
        protected readonly ILogger logger;

        public SubscribeCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            var options = Configurator<SqlBroadcastBusConfig>.Build( configuration, SqlBroadcastBusConfig.Key);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
        }

        public void Dispose()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public async Task<Unit> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var args = request;
            if (args != null)
            {
                var command = new SqlCommand("[broadcast].[Subscribe]", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@subscriberName", args.SubscriberName);
                command.Parameters.AddWithValue("@queues", string.Join(";", args.QueueNames));
                if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            return Unit.Value;
        }
    }
}