namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingConfig
    {
        public string SqlConnectionString { get; set; }
        public string Schema { get; set; } = SqlApplicationSettingsConstants.Schema;
        public string Table { get; set; } = SqlApplicationSettingsConstants.Table;
    }
}