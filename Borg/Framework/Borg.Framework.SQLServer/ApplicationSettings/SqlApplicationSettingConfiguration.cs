using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingConfiguration
    {
        private string schema = SqlApplicationSettingsConstants.Schema;
        private string table = SqlApplicationSettingsConstants.Table;
        public string ConnectionString { get; set; }
        public string Schema { get { return schema; } set { schema = (!value.IsNullOrWhiteSpace()) ? value : SqlApplicationSettingsConstants.Schema; } }
        public string Table { get { return table; } set { table = (!value.IsNullOrWhiteSpace()) ? value : SqlApplicationSettingsConstants.Table; } }
        public string SelectStatement { get; set; } = SqlApplicationSettingsConstants.SelectStatement;
        public string ExistsStatement { get; set; } = SqlApplicationSettingsConstants.ExistsStatement;
        public string InsertStatement { get; set; } = SqlApplicationSettingsConstants.InsertStatement;
        public string UpdateStatement { get; set; } = SqlApplicationSettingsConstants.UpdateStatement;

    }
}