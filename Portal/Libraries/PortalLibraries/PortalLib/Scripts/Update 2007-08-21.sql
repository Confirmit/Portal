/****** Object:  Table [dbo].[UsersDeliveries]    Script Date: 08/21/2007 14:22:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersDeliveries]') AND type in (N'U'))
DROP TABLE [dbo].[UsersDeliveries]
GO

/****** Object:  Table [dbo].[UsersDeliveries]    Script Date: 08/21/2007 14:22:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersDeliveries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DeliveryID] [int] NOT NULL,
	[DeliveryPresentation] [int] NOT NULL
) ON [PRIMARY]
GO