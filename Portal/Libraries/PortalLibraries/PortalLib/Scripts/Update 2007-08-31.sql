/****** Object:  Table [dbo].[Reports]    Script Date: 08/31/2007 14:34:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reports]') AND type in (N'U'))
DROP TABLE [dbo].[Reports]
GO

/****** Object:  Table [dbo].[Reports]    Script Date: 08/31/2007 15:22:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[ToAddress] [nvarchar](100) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Subject] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Message] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsSend] [bit] NOT NULL
) ON [PRIMARY]
GO