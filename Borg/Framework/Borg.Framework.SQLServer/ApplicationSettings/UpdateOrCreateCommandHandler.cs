//using Borg.Framework.Services.Configuration;
//using Borg.Infrastructure.Core;
//using MediatR;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System;
//using System.Data.SqlClient;
//using System.IO;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.SQLServer.ApplicationSettings
//{
//    public class UpdateOrCreateCommandHandler : IRequestHandler<UpdateOrCreateCommand, PaylodCommandResult>, IDisposable
//    {
//        protected readonly SqlConnection sqlConnection;
//        protected readonly ILogger logger;
//        protected readonly SqlApplicationSettingConfig options;

//        public UpdateOrCreateCommandHandler(ILoggerFactory loggerFactory, IConfiguration configuration)
//        {
//            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            options = Configurator<SqlApplicationSettingConfig>.Build(logger, configuration, SqlApplicationSettingsConstants.ConfigKey);
//            sqlConnection = new SqlConnection(Preconditions.NotEmpty(options.SqlConnectionString, nameof(options.SqlConnectionString)));
//        }

//        public async Task<PaylodCommandResult> Handle(UpdateOrCreateCommand request, CancellationToken cancellationToken)
//        {
//            string statement = PrepareStatement();
//            var jsonData = new StringBuilder();
//            using (var stream = new MemoryStream())
//            {
//                await JsonSerializer.SerializeAsync(stream, request.Payload, request.SettingType, cancellationToken: cancellationToken);
//                stream.Position = 0;

//                var reader = new StreamReader(stream);
//                jsonData.Append(await reader.ReadToEndAsync());
//            }
//            var command = new SqlCommand(PrepareStatement(), sqlConnection);
//            command.CommandType = System.Data.CommandType.Text;
//            command.Parameters.AddWithValue("@payloadType", request.SettingType.FullName);

//            command.Parameters.AddWithValue("@payload", jsonData.ToString());
//            if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
//            await command.ExecuteNonQueryAsync();
//            return new PaylodCommandResult(request.SettingType, request.Payload);
//        }

//        private string PrepareStatement()
//        {
//            return $"UDATE [{options.Schema}].[{options.Table}] SET [Payload] = @payload WHERE [TypeName] = @payloadType";
//        }

//        public void Dispose()
//        {
//            if (sqlConnection != null)
//            {
//                if (sqlConnection.State == System.Data.ConnectionState.Open) sqlConnection.Close();
//                sqlConnection.Dispose();
//            }
//        }
//    }
//}