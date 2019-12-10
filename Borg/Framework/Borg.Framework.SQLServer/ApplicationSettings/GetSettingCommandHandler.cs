//using Borg.Framework.Services.Configuration;
//using Borg.Infrastructure.Core;
//using Borg.Framework.Dispatch;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Data.SqlClient;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.SQLServer.ApplicationSettings
//{
//    public class GetSettingCommandHandler : IRequestHandler<GetSettingCommand, PaylodCommandResult>, IDisposable

//    {
//        protected readonly SqlConnection sqlConnection;
//        protected readonly ILogger logger;
//        protected readonly SqlApplicationSettingConfig options;

//        public GetSettingCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
//        {
//            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            options = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
//            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
//        }

//        public async Task<PaylodCommandResult> Handle(GetSettingCommand request, CancellationToken cancellationToken)
//        {
//            string statement = PrepareStatement();
//            string jsonData = "{}";

//            using (var command = new SqlCommand(statement, sqlConnection))
//            {
//                command.Parameters.AddWithValue("@typeName", request.SettingType.FullName);
//                command.CommandType = System.Data.CommandType.Text;
//                if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
//                using (var reader = await command.ExecuteReaderAsync())
//                {
//                    if (reader.HasRows)
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            jsonData = reader.GetString(0);
//                        }
//                    }
//                }
//            }
//            var payload = JsonSerializer.Deserialize(jsonData, request.SettingType);
//            return new PaylodCommandResult(request.SettingType, payload);
//        }

//        private string PrepareStatement()
//        {
//            return $"SELECT TOP 1 [Payload] FROM [{options.Schema}].[{options.Table}] WHERE [TypeName] = @typeName";
//        }

//        #region IDisposable Support
//        private bool disposedValue = false; // To detect redundant calls

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    if (sqlConnection != null)
//                    {
//                        if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
//                        sqlConnection.Dispose();
//                    }
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
//                // TODO: set large fields to null.

//                disposedValue = true;
//            }
//        }

//        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
//        // ~GetSettingCommandHandler()
//        // {
//        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//        //   Dispose(false);
//        // }

//        // This code added to correctly implement the disposable pattern.
//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(true);
//            // TODO: uncomment the following line if the finalizer is overridden above.
//            // GC.SuppressFinalize(this);
//        }
//        #endregion


//    }
//}