
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'{schema}')
EXEC sys.sp_executesql N'CREATE SCHEMA {schema}'
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{schema}].[SchemaVersion]') AND type in (N'U'))
BEGIN
CREATE TABLE [broadcast].[SchemaVersion](
	[Version] [int] NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[{schema}].[SchemaVersion]') AND name = N'PK_SchemaVersiom_Version')
CREATE UNIQUE CLUSTERED INDEX [PK_SchemaVersiom_Version] ON [{schema}].[SchemaVersion]
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [{schema}].[Settings](
	[TypeName] [nchar](450) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [PK_Settings_TypeName]    Script Date: 20/10/2019 8:49:48 μμ ******/
CREATE UNIQUE CLUSTERED INDEX [PK_Settings_TypeName] ON [{schema}].[Settings]
(
	[TypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

INSERT INTO [{schema}].[SchemaVersion] ([Version]) VALUES ({versionNumber});
GO