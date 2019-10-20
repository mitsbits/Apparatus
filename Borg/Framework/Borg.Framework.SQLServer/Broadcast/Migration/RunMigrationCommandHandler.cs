using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast.Migration
{
    public class RunMigrationCommandHandler : IRequestHandler<RunMigrationCommand>, IDisposable
    {
        protected readonly SqlConnection sqlConnection;
        private string sqlCommandText;
        private readonly ILogger logger;

        public RunMigrationCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            var options = Configurator<SqlBroadcastBusConfig>.Build(logger, configuration, SqlBroadcastBusConfig.Key);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
            using (var stream = GetType().Assembly.GetManifestResourceStream(MigrationPipeline.migrationResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), MigrationPipeline.migrationResourcePath);
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

        public Task<Unit> Handle(RunMigrationCommand request, CancellationToken cancellationToken)
        {
            sqlCommandText = sqlCommandText.Replace("@version", request.CurrnetSchemaVersion.ToString());
            var server = new Server(new ServerConnection(sqlConnection));
            server.ConnectionContext.ExecuteNonQuery(sqlCommandText);

            return Unit.Task;
        }
    }
}