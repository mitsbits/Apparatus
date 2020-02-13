
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'broadcast')
EXEC sys.sp_executesql N'CREATE SCHEMA [broadcast]'
GO
IF NOT EXISTS (SELECT * FROM sys.sequences WHERE name = N'QueueMessageId' AND schema_id = SCHEMA_ID(N'broadcast'))
BEGIN
CREATE SEQUENCE [broadcast].[QueueMessageId] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CYCLE 
 CACHE 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[QueueMessage]') AND type in (N'U'))
BEGIN
CREATE TABLE [broadcast].[QueueMessage](
	[Id] [bigint] NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL,
	[QueueName] [varchar](256) NOT NULL,
	[SubscriberName] [varchar](256) NOT NULL,
	[PayloadType] [nvarchar](1024) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[broadcast].[QueueMessage]') AND name = N'PK_QueueMessage_Id_QueueName_SubscciberName')
CREATE UNIQUE CLUSTERED INDEX [PK_QueueMessage_Id_QueueName_SubscciberName] ON [broadcast].[QueueMessage]
(
	[Id] ASC,
	[QueueName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[SchemaVersion]') AND type in (N'U'))
BEGIN
CREATE TABLE [broadcast].[SchemaVersion](
	[Version] [int] NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[broadcast].[SchemaVersion]') AND name = N'PK_SchemaVersiom_Version')
CREATE UNIQUE CLUSTERED INDEX [PK_SchemaVersiom_Version] ON [broadcast].[SchemaVersion]
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[Subscriber]') AND type in (N'U'))
BEGIN
CREATE TABLE [broadcast].[Subscriber](
	[QueueName] [varchar](256) NOT NULL,
	[SubscriberName] [varchar](256) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[broadcast].[Subscriber]') AND name = N'PK_Subscriber_QueueName_SubscriberName')
CREATE UNIQUE CLUSTERED INDEX [PK_Subscriber_QueueName_SubscriberName] ON [broadcast].[Subscriber]
(
	[QueueName] ASC,
	[SubscriberName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[broadcast].[QueueMessage]') AND name = N'IX_QueueMessage_QueueName_SubscriberName')
CREATE NONCLUSTERED INDEX [IX_QueueMessage_QueueName_SubscriberName] ON [broadcast].[QueueMessage]
(
	[QueueName] ASC,
	[SubscriberName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[DF_QueueMessage_Id]') AND type = 'D')
BEGIN
ALTER TABLE [broadcast].[QueueMessage] ADD  CONSTRAINT [DF_QueueMessage_Id]  DEFAULT (NEXT VALUE FOR [broadcast].[QueueMessageId]) FOR [Id]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[DF_QueueMessage_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [broadcast].[QueueMessage] ADD  CONSTRAINT [DF_QueueMessage_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[DF_SchemaVersion_CreatedOn]') AND type = 'D')
BEGIN
ALTER TABLE [broadcast].[SchemaVersion] ADD  CONSTRAINT [DF_SchemaVersion_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[PublisherAddQueueMessage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [broadcast].[PublisherAddQueueMessage] AS' 
END
GO
ALTER PROCEDURE [broadcast].[PublisherAddQueueMessage]
		 @queuename varchar(256), 
		 @payloadType varchar(256),
		 @payload nvarchar(MAX)

AS
BEGIN

	SET NOCOUNT ON;

		INSERT INTO [broadcast].[QueueMessage] ([QueueName], [SubscriberName], [PayloadType], [Payload] )
			SELECT @queuename, s.SubscriberName,   @payloadType, @payload 
			FROM [broadcast].[Subscriber] s 
			WHERE s.QueueName = @queuename 

END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[Subscribe]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [broadcast].[Subscribe] AS' 
END
GO
ALTER PROCEDURE [broadcast].[Subscribe]
	
			 @subscriberName nvarchar(256),
			 @queues nvarchar(MAX)
AS
BEGIN

	SET NOCOUNT ON;
	WITH cte as (
		SELECT [SubscriberName], [QueueName] 
		FROM [broadcast].[Subscriber]
		WHERE [SubscriberName] = @subscriberName)

	DELETE FROM cte
	INSERT INTO [broadcast].[Subscriber]([SubscriberName], [QueueName] )
	SELECT @subscriberName, [value] FROM STRING_SPLIT(@queues, ';')


END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[broadcast].[SubscriberReadQueueMessage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [broadcast].[SubscriberReadQueueMessage] AS' 
END
GO
ALTER PROCEDURE [broadcast].[SubscriberReadQueueMessage]
			 @queuename varchar(256), 
			 @subscriberName varchar(256)
AS
BEGIN

	SET NOCOUNT ON;

	WITH cte  AS (
		SELECT TOP 1 * FROM [broadcast].[QueueMessage] m with (rowlock, readpast)  WHERE m.QueueName = @queuename AND m.SubscriberName =  @subscriberName ORDER BY m.CreatedOn, m.Id)
		  delete from cte
		  output deleted.CreatedOn, deleted.PayloadType, deleted.Payload;
END
GO
INSERT INTO [broadcast].[SchemaVersion] ([Version]) VALUES ({version});
GO