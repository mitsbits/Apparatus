using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
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
        private readonly SqlBroadcastBusConfig options;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <exception cref="TimeoutException" >the commands run later than the timeout </exception>
        /// <exception cref="SqlException "></exception>
        public RunMigrationCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            options = Configurator<SqlBroadcastBusConfig>.Build(logger, configuration, SqlBroadcastBusConfig.Key);
            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
        }

        public async Task<Unit> Handle(RunMigrationCommand request, CancellationToken cancellationToken)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream(MigrationPipeline.migrationResourcePath))
            using (var reader = new StreamReader(stream))
            {
                sqlCommandText = Preconditions.NotEmpty(reader.ReadToEnd(), MigrationPipeline.migrationResourcePath);
            }
    
            var replacements = new Dictionary<string, string>() { { "{version}", request.CurrnetSchemaVersion.ToString() } };
            await sqlConnection.RunBatch(sqlCommandText, replacements, true);
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