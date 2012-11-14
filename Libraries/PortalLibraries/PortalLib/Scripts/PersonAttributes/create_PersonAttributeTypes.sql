USE [Portal]
GO

/****** Объект:  Table [dbo].[PersonAttributeTypes]    Дата сценария: 02/09/2009 22:45:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonAttributeTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AttributeName] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsShownToUsers] [bit] NOT NULL CONSTRAINT [DF_PersonAttributeTypes_IsShownToUsers]  DEFAULT ((1)),
 CONSTRAINT [PK_PersonAttributeTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UK_PersonAttributeTypes] UNIQUE NONCLUSTERED 
(
	[AttributeName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('DomainName'
   ,0)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('PublicPassword'
   ,0)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('Photo'
   ,0)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('Contact'
   ,1)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('Phone'
   ,1)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('Address'
   ,1)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('EventDateFormat'
   ,1)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('Room'
   ,1)
GO

INSERT INTO [Portal].[dbo].[PersonAttributeTypes]
   ([AttributeName]
   ,[IsShownToUsers])
 VALUES
   ('EMail'
   ,1)
GO