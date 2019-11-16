namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
{
    internal class CheckForSchemaCommandResult
    {
        public CheckForSchemaCommandResult(int databaseSchemaVersion, int migrationSchemaVersion)
        {
            DatabaseSchemaVersion = databaseSchemaVersion;
            MigrationSchemaVersion = migrationSchemaVersion;
        }

        public int DatabaseSchemaVersion { get; }
        public int MigrationSchemaVersion { get; }
        public bool IsNewerThanDatabase => MigrationSchemaVersion > DatabaseSchemaVersion;
    }
}