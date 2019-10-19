CREATE SCHEMA [brodacast];  
GO

CREATE SEQUENCE [broadcast].[QueueMessageId] 
 AS [bigint]
 START WITH 1
 INCREMENT BY 1
 MINVALUE -9223372036854775808
 MAXVALUE 9223372036854775807
 CYCLE 
 CACHE 
GO
CREATE TABLE [broadcast].[QueueMessage](
	[Id] [bigint] NOT NULL,
	[CreatedOn] [datetimeoffset](7) NOT NULL,
	[QueueName] [nvarchar](512) NOT NULL,
	[QueueSubscriber] [nvarchar](512) NOT NULL,
	[PayloadType] [nvarchar](1024) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE CLUSTERED INDEX [PK_QueueMessage] ON [broadcast].[QueueMessage]
(
	[Id] ASC,
	[CreatedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [broadcast].[QueueSubscriber](
	[QueueName] [nvarchar](512) NOT NULL,
	[QueueSubscriber] [nvarchar](512) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE UNIQUE CLUSTERED INDEX [PK_QueueSbscriber] ON [broadcast].[QueueSubscriber]
(
	[QueueName] ASC,
	[QueueSubscriber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_QueueMessage_QueueName] ON [broadcast].[QueueMessage]
(
	[QueueName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_QueueMessage_QueueSubscriber] ON [broadcast].[QueueMessage]
(
	[QueueSubscriber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [broadcast].[QueueMessage] ADD  CONSTRAINT [DF_QueueMessageId]  DEFAULT (NEXT VALUE FOR [broadcast].[QueueMessageId]) FOR [Id]
GO
ALTER TABLE [broadcast].[QueueMessage] ADD  CONSTRAINT [DF_QueueMessage_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [broadcast].[PublisherAddQueueMessage]
		 @queuename nvarchar(512), 
		 @payloadType nvarchar(512),
		 @payload nvarchar(MAX)

AS
BEGIN

	SET NOCOUNT ON;
		WITH InsertValue as (
			SELECT[QueueName], [QueueSubscriber], [PayloadType], [Payload] 
			FROM [broadcast].[QueueMessage] 
			WHERE QueueName = @queuename)

		INSERT INTO InsertValue
			SELECT @queuename, s.QueueSubscriber,   @payloadType, @payload 
			FROM [broadcast].[QueueSubscriber] s 
			WHERE s.QueueName = @queuename 

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [broadcast].[SubscriberReadQueueMessage]
			 @queuename nvarchar(512), 
			 @queuesubscriber nvarchar(512)
AS
BEGIN

	SET NOCOUNT ON;

    WITH cte  AS (
		SELECT TOP 1 * FROM [broadcast].[QueueMessage] m with (rowlock, readpast)  WHERE m.QueueName = @queuename AND m.QueueSubscriber =  @queuesubscriber ORDER BY m.CreatedOn, m.Id)
		  delete from cte
		  output deleted.CreatedOn, deleted.PayloadType, deleted.Payload;
END
GO