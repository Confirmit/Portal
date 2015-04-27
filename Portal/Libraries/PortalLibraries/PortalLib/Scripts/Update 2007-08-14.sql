/****** Object:  StoredProcedure [dbo].[GetWorkEvent]    Script Date: 08/14/2007 14:58:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWorkEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetWorkEvent]
GO

/****** Object:  StoredProcedure [dbo].[GetWorkEvent]    Script Date: 08/14/2007 14:58:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 14.08.2007
-- Description:	Возвращает рабочее событие за указанную дату.
-- =============================================
CREATE PROCEDURE [dbo].[GetWorkEvent]
	@UserID int,
	@Date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT * FROM UptimeEvents WHERE UserID = @UserID AND  FLOOR(CONVERT( float, BeginTime)) = FLOOR(CONVERT( float, @Date)) AND UptimeEventTypeId=10
END
GO

/****** Object:  Table [dbo].[InternetUsers]    Script Date: 08/15/2007 14:40:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InternetUsers]') AND type in (N'U'))
DROP TABLE [dbo].[InternetUsers]
GO

/****** Object:  Table [dbo].[InternetUsers]    Script Date: 08/15/2007 14:40:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InternetUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Password] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 08/15/2007 14:41:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuthenticateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AuthenticateUser]
GO

/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 08/15/2007 14:41:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 15.08.2007
-- Description:	Процедура аутентификации Internet-пользователя.
-- =============================================
CREATE PROCEDURE [dbo].[AuthenticateUser]
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50),
	@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM InternetUsers 
	WHERE (UserName = @UserName) AND (Password = @Password)
END
GO

/****** Object:  Table [dbo].[InternetUsers]    Script Date: 08/15/2007 16:30:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InternetUsers]') AND type in (N'U'))
DROP TABLE [dbo].[InternetUsers]
GO

/****** Object:  Table [dbo].[InternetUsers]    Script Date: 08/15/2007 16:30:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InternetUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Password] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 08/15/2007 16:30:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuthenticateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AuthenticateUser]
GO

/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 08/15/2007 16:31:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 15.08.2007
-- Description:	Процедура аутентификации Internet-пользователя.
-- =============================================
CREATE PROCEDURE [dbo].[AuthenticateUser]
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50),
	@Password nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) 
	FROM
		Users usr,
		InternetUsers iusr
	WHERE
		(usr.ID = iusr.UserID) AND
		(Lower(usr.DomainName) = Lower(@UserName)) AND 
		(iusr.Password = @Password)
END
GO

/****** Object:  Table [dbo].[PortalAdmins]    Script Date: 08/15/2007 16:39:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PortalAdmins]') AND type in (N'U'))
DROP TABLE [dbo].[PortalAdmins]
GO

/****** Object:  Table [dbo].[PortalAdmins]    Script Date: 08/15/2007 16:39:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PortalAdmins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_PortalAdmins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO