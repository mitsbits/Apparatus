namespace Borg.Framework.SQLServer.ApplicationSettings
{
    internal class SqlApplicationSettingsConstants
    {
        internal const string Schema = "application";
        internal const string Table = "Settings";
        internal const string CheckForVersionResourcePath = "Borg.Framework.SQLServer.ApplicationSettings.Data.CheckForVersion.sql";
        internal const string CreateDatabaseSchemaResourcePath = "Borg.Framework.SQLServer.ApplicationSettings.Data.CreateDatabaseSchema.sql";
        internal const string ConfigKey = "Borg:Framework:SQLServer:ApplicationSettings";
        internal const string SelectStatement = "SELECT TOP 1 [Payload] FROM [{0}].[{1}] WHERE [TypeName] = @typeName;";
        internal const string ExistsStatement = "SELECT CAST(CASE WHEN EXISTS(SELECT 1 FROM [{0}].[{1}] WHERE [TypeName] = @typeName) THEN 1 ELSE 0 END AS bit);";
        internal const string UpdateStatement = "UPDATE [{0}].[{1}] SET [Payload] = @payload WHERE [TypeName] = @typeName;";
        internal const string InsertStatement = "INSERT INTO  [{0}].[{1}] ([TypeName], [Payload]) VALUES (@typeName, @payload);";
    }
}