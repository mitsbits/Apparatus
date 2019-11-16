using Borg.Framework.ApplicationSettings;
using Borg.Framework.SQLServer.ApplicationSettings;
using Borg.Infrastructure.Core.Services.Factory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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


        public SqlApplicationSettingsManager(ILoggerFactory loggerFactory, IOptionsMonitor<SqlApplicationSettingConfiguration> options)
        {
            logger = loggerFactory.CreateForType(GetType());
            optionsMonitor = options;
        }

        private SqlApplicationSettingConfiguration Options => optionsMonitor.CurrentValue;

        public event ApplicationSettingChangedEventHandler ApplicationSettingChange;

        public async Task<T> Get<T>() where T : IApplicationSetting, new()
        {
            if (!await Exists<T>())
            {
                return New<T>.Instance();
            }
            var commandText = string.Format(Options.SelectStatement, Options.Schema, Options.Table);
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

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload)))
            {
                stream.Position = 0;
                return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions());
            }
        }

        public async Task<T> UpdateOrCreate<T>(T updated) where T : IApplicationSetting, new()
        {
            var exists = await Exists<T>();
            var commandStatement = (exists) ? string.Format(Options.UpdateStatement, Options.Schema, Options.Table) : string.Format(Options.InsertStatement, Options.Schema, Options.Table);

            using (var connection = new SqlConnection(Options.ConnectionString))
            {
                using (var command = new SqlCommand(commandStatement, connection))
                {
                    command.Parameters.AddWithValue("@typeName", typeof(T).FullName);
                    using (var stream = new MemoryStream())
                    {
                        await JsonSerializer.SerializeAsync(stream, updated, typeof(T));
                        stream.Position = 0;
                        var json = Encoding.UTF8.GetString(stream.ToArray());
                        command.Parameters.AddWithValue("@payload", json);
                    }
                    if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            OnApplicationSettingChange(new ApplicationSettingChangedEventArgs(typeof(T).FullName));
            return updated;
        }

        private async Task<bool> Exists<T>()
        {
            var exsts = string.Format(Options.ExistsStatement, Options.Schema, Options.Table);
            bool result = false;
            using (var connection = new SqlConnection(Options.ConnectionString))
            {
                using (var command = new SqlCommand(exsts, connection))
                {
                    command.Parameters.AddWithValue("@typeName", typeof(T).FullName);
                    if (command.Connection.State == System.Data.ConnectionState.Closed) await command.Connection.OpenAsync();
                    result = (bool)(await command.ExecuteScalarAsync());
                }
            }
            return result;
        }

        private void OnApplicationSettingChange(ApplicationSettingChangedEventArgs e)
        {
            ApplicationSettingChangedEventHandler handler = ApplicationSettingChange;
            handler?.Invoke(this, e);
        }
    }
}