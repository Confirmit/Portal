USE [Portal]
GO
/****** Object:  Table [dbo].[News]    Script Date: 11/08/2007 16:53:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Text] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[AuthorID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ExpireTime] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [Portal]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[Users] ([ID])





GO
/****** Object:  Table [dbo].[AllowTags]    Script Date: 11/08/2007 17:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllowTags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[tagName] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
 CONSTRAINT [PK_AllowTags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
