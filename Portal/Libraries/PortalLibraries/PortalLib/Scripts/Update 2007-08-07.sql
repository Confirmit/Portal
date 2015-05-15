/****** Object:  StoredProcedure [dbo].[GetHoliday]    Script Date: 08/07/2007 13:05:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[GetHoliday]    Script Date: 08/10/2007 11:25:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHoliday]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetHoliday]
END
GO

-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 2007.08.06
-- Description:	Возвращает запись о празднике.
-- =============================================
CREATE PROCEDURE [dbo].[GetHoliday] 
	@HolidayDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Holiday WHERE FLOOR(CONVERT( float, Date)) = FLOOR(CONVERT( float, @HolidayDate))
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetUserByID]
(
@ID int
)
AS
SELECT 
	ISNULL(FName,'') AS FName, 
	ISNULL(IName,'') AS IName, 
	ISNULL(LName,'') AS LName, 
	ISNULL(nlFName, '') AS nlFName,
	ISNULL(Initial, '') AS Initial,
	ISNULL(nlLName, '') AS nlLName,
	Sex,
	Birthday,
	ISNULL(DomainName,'') AS DomainName,
	ISNULL(PrimaryEMail, '') AS PrimaryEMail,
	ISNULL(Project, '') AS Project,
	ISNULL(Room, '') AS Room,
	ISNULL(PrimaryIP, '') AS PrimaryIP,
	LongServiceEmployees,
	PersonnelReserve,
	EmployeesUlterSYSMoscow
FROM Users 
WHERE ID=@ID
RETURN @@ROWCOUNT
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetUserByDomainName]
(
@DomainName nvarchar(50)
)
AS
SELECT 
	ID, 
	ISNULL(FName,'') AS FName, 
	ISNULL(IName,'') AS IName, 
	ISNULL(LName,'') AS LName,
	ISNULL(nlFName, '') AS nlFName,
	ISNULL(Initial, '') AS Initial,
	ISNULL(nlLName, '') AS nlLName,
	Sex,
	Birthday,
	ISNULL(DomainName, '') AS DomainName,
	ISNULL(PrimaryEMail, '') AS PrimaryEMail,
	ISNULL(Project, '') AS Project,
	ISNULL(Room, '') AS Room,
	ISNULL(PrimaryIP, '') AS PrimaryIP,
	LongServiceEmployees,
	PersonnelReserve,
	EmployeesUlterSYSMoscow
FROM Users 
WHERE UPPER(DomainName)=UPPER(@DomainName)
RETURN @@ROWCOUNT
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 08/08/2007 10:38:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateUser]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[CreateUser]-- =============================================
END
GO

-- Author:		Ivan Yakimov
-- Create date: 08.08.2007
-- Description:	Процедура создания пользователя.
-- =============================================
CREATE PROCEDURE [dbo].[CreateUser]
	-- Add the parameters for the stored procedure here
	@FirstNameRus nvarchar(50),
	@MiddleNameRus nvarchar(50),
	@LastNameRus nvarchar(100),
	@FirstNameEng nvarchar(50),
	@InitialEng nvarchar(1),
	@LastNameEng nvarchar(100),
	@Sex smallint,
	@Birthday datetime,
	@DomainName nvarchar(50),
	@PrimaryEMail nvarchar(50),
	@Project nvarchar(50),
	@Room nvarchar(5),
	@PrimaryIP nvarchar(23),
	@LongServiceEmployees bit,
	@PersonnelReserve bit,
	@EmployeesUlterSYSMoscow bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Users
	(
		FName, 
		IName, 
		LName, 
		nlFName,
		Initial,
		nlLName,
		Sex,
		Birthday,
		DomainName,
		PrimaryEMail,
		Project,
		Room,
		PrimaryIP,
		LongServiceEmployees,
		PersonnelReserve,
		EmployeesUlterSYSMoscow
	)
	VALUES
	(
		@FirstNameRus,
		@MiddleNameRus,
		@LastNameRus,
		@FirstNameEng,
		@InitialEng,
		@LastNameEng,
		@Sex,
		@Birthday,
		@DomainName,
		@PrimaryEMail,
		@Project,
		@Room,
		@PrimaryIP,
		@LongServiceEmployees,
		@PersonnelReserve,
		@EmployeesUlterSYSMoscow
	)
END
GO

/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 08/08/2007 10:39:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 08/10/2007 11:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUser]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[UpdateUser]-- =============================================
END
GO

-- Author:		Ivan Yakimov
-- Create date: 08.08.2007
-- Description:	Процедура изменения данных пользователя.
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@FirstNameRus nvarchar(50),
	@MiddleNameRus nvarchar(50),
	@LastNameRus nvarchar(100),
	@FirstNameEng nvarchar(50),
	@InitialEng nvarchar(1),
	@LastNameEng nvarchar(100),
	@Sex smallint,
	@Birthday datetime,
	@DomainName nvarchar(50),
	@PrimaryEMail nvarchar(50),
	@Project nvarchar(50),
	@Room nvarchar(5),
	@PrimaryIP nvarchar(23),
	@LongServiceEmployees bit,
	@PersonnelReserve bit,
	@EmployeesUlterSYSMoscow bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Users
	SET
		FName = @FirstNameRus, 
		IName = @MiddleNameRus, 
		LName = @LastNameRus, 
		nlFName = @FirstNameEng,
		Initial = @InitialEng,
		nlLName = @LastNameEng,
		Sex = @Sex,
		Birthday = @Birthday,
		DomainName = @DomainName,
		PrimaryEMail = @PrimaryEMail,
		Project = @Project,
		Room = @Room,
		PrimaryIP = @PrimaryIP,
		LongServiceEmployees = @LongServiceEmployees,
		PersonnelReserve = @PersonnelReserve,
		EmployeesUlterSYSMoscow = @EmployeesUlterSYSMoscow
	WHERE ID = @UserID
END
GO

/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 08/08/2007 10:39:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUser]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[DeleteUser]
END
GO

-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 08.08.2007
-- Description:	Процедура удаления пользователя.
-- =============================================
CREATE PROCEDURE [dbo].[DeleteUser]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Удалить все события пользователя
	DELETE FROM UptimeEvents
	WHERE UserID = @UserID

    -- Insert statements for procedure here
	DELETE FROM Users
	WHERE ID = @UserID
END
GO

/****** Object:  Table [dbo].[NotificationLists]    Script Date: 08/13/2007 12:37:50 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationLists]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[NotificationLists](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[EMail] [nvarchar](100) COLLATE Cyrillic_General_CI_AS NOT NULL,
		[Type] [int] NOT NULL
	) ON [PRIMARY]
END
GO

/****** Object:  StoredProcedure [dbo].[GetNotificationList]    Script Date: 08/13/2007 12:42:09 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNotificationList]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetNotificationList]
END
GO

/****** Object:  StoredProcedure [dbo].[GetNotificationList]    Script Date: 08/13/2007 12:42:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNotificationList] 
	@Type int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EMail FROM NotificationLists WHERE [Type] = @Type
END
GO