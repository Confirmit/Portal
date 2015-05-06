/****** Object:  Table [dbo].[Offices]    Script Date: 08/27/2007 12:52:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Offices]') AND type in (N'U'))
DROP TABLE [dbo].[Offices]
GO

/****** Object:  Table [dbo].[Offices]    Script Date: 08/27/2007 12:54:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OfficeName] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[StatusesServiceURL] [nvarchar](1000) COLLATE Cyrillic_General_CI_AS NULL
) ON [PRIMARY]
GO