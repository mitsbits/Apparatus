IF OBJECT_ID(N'[broadcast].[SchemaVersion]', N'U') IS  NULL
BEGIN
    SELECT 0 AS [DatabaseSchemaVersion], @version AS [MigrationSchemaVersion];
END
ELSE
BEGIN
    SELECT MAX(ISNULL( [Version], 0)) AS [DatabaseSchemaVersion], @version AS [MigrationSchemaVersion] FROM [broadcast].[SchemaVersion]
END