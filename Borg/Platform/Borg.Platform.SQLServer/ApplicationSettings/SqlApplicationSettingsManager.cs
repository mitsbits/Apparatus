using Borg.Framework.ApplicationSettings;
using Borg.Framework.SQLServer.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Borg.Platform.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingsManager : IApplicationSettingsManager
    {
        private readonly ILogger logger;
        private readonly IOptionsMonitor<SqlApplicationSettingConfiguration> optionsMonitor;
        private const string selectStatement = "SELECT TOP 1 [Payload] FROM [{0}].[{1}] WHERE [TypeName] = @typeName;";
        public SqlApplicationSettingsManager(ILoggerFactory loggerFactory, IOptionsMonitor<SqlApplicationSettingConfiguration> options)
        {
            logger = loggerFactory.CreateForType(GetType());
            optionsMonitor = options;
        }

        SqlApplicationSettingConfiguration Options => optionsMonitor.CurrentValue;
        public async Task<T> Get<T>() where T : IApplicationSetting, new()
        {
            var commandText = string.Format(selectStatement, Options.Schema, Options.Table);
            string payload = "{}";
            using (var connection = new SqlConnection(Options.ConnectionString))
            {
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@typeName", typeof(T).FullName);
                    if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                    payload = (await command.ExecuteScalarAsync()).ToString();
                }
            }

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(payload)))
            {
               stream.Position = 0;
               return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions());

            }

        }

        public Task<T> UpdateOrCreate<T>(T updated) where T : IApplicationSetting, new()
        {
            throw new NotImplementedException();
        }
    }
}
