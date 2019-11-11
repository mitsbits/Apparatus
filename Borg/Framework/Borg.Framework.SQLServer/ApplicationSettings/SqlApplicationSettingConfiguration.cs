using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingConfiguration
    {
        private  string schema = SqlApplicationSettingsConstants.Schema;
        private  string table = SqlApplicationSettingsConstants.Table;
        public string ConnectionString { get; set; }
        public string Schema { get { return schema; } set { schema = (!value.IsNullOrWhiteSpace()) ? value : SqlApplicationSettingsConstants.Schema;  } }
        public string Table { get { return table; } set { table = (!value.IsNullOrWhiteSpace()) ? value : SqlApplicationSettingsConstants.Table; } }
    }
}