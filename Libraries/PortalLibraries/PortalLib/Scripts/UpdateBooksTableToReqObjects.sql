USE [Portal]
GO

/* CREATE TABLES */
/****** Object:  Table [dbo].[RequestObject]    Script Date: 04/15/2010 16:01:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RequestObject](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[OwnerID] [int] NULL,
	[OfficeID] [int] NOT NULL,
 CONSTRAINT [PK_1_RequsetObject] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[RequestObject]  WITH CHECK ADD  CONSTRAINT [FK_1_RequsetObject_Offices] FOREIGN KEY([OfficeID])
REFERENCES [dbo].[Offices] ([ID])
GO
ALTER TABLE [dbo].[RequestObject] CHECK CONSTRAINT [FK_1_RequsetObject_Offices]
GO
ALTER TABLE [dbo].[RequestObject]  WITH CHECK ADD  CONSTRAINT [FK_1_RequsetObject_Users] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[RequestObject] CHECK CONSTRAINT [FK_1_RequsetObject_Users]
GO

/****** Object:  Table [dbo].[Disks]    Script Date: 04/15/2010 16:04:19 ******/
CREATE TABLE [dbo].[Disks](
	[ID] [int] NOT NULL,
	[Manufacturers] [nvarchar](max) NOT NULL,
	[PublishingYear] [int] NOT NULL,
	[Annotation] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_1_Disks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Disks]  WITH CHECK ADD  CONSTRAINT [FK_1_Disks_1_RequsetObject] FOREIGN KEY([ID])
REFERENCES [dbo].[RequestObject] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Disks] CHECK CONSTRAINT [FK_1_Disks_1_RequsetObject]
GO

/****** Object:  Table [dbo].[DiscountCard]    Script Date: 04/15/2010 16:05:05 ******/

/****** Object:  Table [dbo].[DiscountCard]    Script Date: 06/25/2010 16:43:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscountCard]') AND type in (N'U'))
DROP TABLE [dbo].[DiscountCard]
GO

CREATE TABLE [dbo].[DiscountCard](
	[ID] [int] NOT NULL,
	[ValuePercent] [tinyint] NOT NULL,
	[ShopName] [nvarchar](255) NULL,
	[ShopSite] [nvarchar](255) NULL,
 CONSTRAINT [PK_DiscountCard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DiscountCard]  WITH CHECK ADD  CONSTRAINT [FK_DiscountCard_RequestObject] FOREIGN KEY([ID])
REFERENCES [dbo].[RequestObject] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DiscountCard] CHECK CONSTRAINT [FK_DiscountCard_RequestObject]
GO

/****** Object:  Table [dbo].[Requests]    Script Date: 04/15/2010 16:05:48 ******/
CREATE TABLE [dbo].[Requests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ObjectID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsTaken] [bit] NOT NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_RequestObject] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[RequestObject] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_RequestObject]
GO

ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_Users]
GO
/* END CREATE TABLES */

/* CREATE FUNCTIONS */
/****** Object:  UserDefinedFunction [dbo].[GetRequestObjectHolderID]    Script Date: 04/15/2010 15:56:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		VadimS
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetRequestObjectHolderID]
(
	-- Add the parameters for the function here
	@ObjectID INT
)
RETURNS INT
AS
BEGIN
	DECLARE @OwnerID INT, @LastOperationDate DATETIME

	SET @OwnerID = (SELECT OwnerID FROM [RequestObject] WHERE ID = @ObjectID) 
	SET @LastOperationDate = (SELECT MAX(DATE) FROM [Requests] WHERE ObjectID = @ObjectID) 

	IF (@LastOperationDate is null) 
	BEGIN
		 RETURN @OwnerID
	END 

	RETURN (SELECT 
		CASE WHEN IsTaken = 0 
		THEN @OwnerID 
		ELSE UserID 
		END as holderID 
	FROM [Requests] 
	WHERE Date = @LastOperationDate AND ObjectID = @ObjectID )
END
GO

/****** Object:  UserDefinedFunction [dbo].[GetRequestObjectType]    Script Date: 04/15/2010 15:57:08 ******/
-- =============================================
-- Author:		VadimS
-- Description:	GetRequestObjectType
-- =============================================
CREATE FUNCTION [dbo].[GetRequestObjectType]
(
	@ObjectID INT
)
RETURNS NVARCHAR(50)
AS
BEGIN
	IF (EXISTS (SELECT ID FROM Disks WHERE ID = @ObjectID))
		RETURN 'Disk'

	IF (EXISTS (SELECT ID FROM DiscountCard WHERE ID = @ObjectID))
		RETURN 'Card'

	IF (EXISTS (SELECT ID FROM Books_Books WHERE ID = @ObjectID))
		RETURN 'Book'
	
	RETURN 'UNKNOWN'
END
GO

/****** Object:  StoredProcedure [dbo].[GetObjectsOnHand]    Script Date: 04/16/2010 16:29:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------------------------------------------
CREATE procedure [dbo].[GetObjectsOnHand](@UserID int) as
BEGIN
	IF (@UserID = 0)
		SET @UserID = NULL	
	
	SELECT Title, 
		FirstName as OwnerFirstName, 
		CASE WHEN LastName is null
		THEN '<MLText><Text lang="en">Office</Text><Text lang="ru">Офис</Text></MLText>'
		ELSE LastName 
		END as OwnerLastName,
		dbo.GetRequestObjectType(RequestObject.ID) as ObjType
	FROM RequestObject
	LEFT JOIN Users ON Users.ID = OwnerID
	WHERE 
		(@UserID IS NULL AND [dbo].GetRequestObjectHolderID(RequestObject.ID) IS NULL)
		OR
		(@UserID IS NOT NULL AND [dbo].GetRequestObjectHolderID(RequestObject.ID) = @UserID)
END
GO

/* UPDATE BOOKS */
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Books_Books
	DROP CONSTRAINT FK_Books_Books_Offices
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Books_Books
	DROP CONSTRAINT DF_Books_Books_IsElectronic
GO
CREATE TABLE dbo.Tmp_Books_Books
	(
	ID int NOT NULL,
	Authors nvarchar(MAX) NOT NULL,
	Title nvarchar(MAX) NOT NULL,
	PublishingYear int NOT NULL,
	Annotation nvarchar(MAX) NOT NULL,
	Language nvarchar(100) NOT NULL,
	OfficeID int NOT NULL,
	DownloadLink nvarchar(MAX) NULL,
	IsElectronic bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'Идентификатор книги.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'ID'
GO
DECLARE @v sql_variant 
SET @v = N'Авторы книги.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'Authors'
GO
DECLARE @v sql_variant 
SET @v = N'Название книги.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'Title'
GO
DECLARE @v sql_variant 
SET @v = N'Аннотация к книге.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'Annotation'
GO
DECLARE @v sql_variant 
SET @v = N'Идентификатор офиса, в котором расположена книга.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'OfficeID'
GO
DECLARE @v sql_variant 
SET @v = N'Ссылка для скачивания (только для электронных книг).'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'DownloadLink'
GO
DECLARE @v sql_variant 
SET @v = N'Является ли книга электронной (компьютерный вариант).'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Tmp_Books_Books', N'COLUMN', N'IsElectronic'
GO
ALTER TABLE dbo.Tmp_Books_Books ADD CONSTRAINT
	DF_Books_Books_IsElectronic DEFAULT ((1)) FOR IsElectronic
GO
IF EXISTS(SELECT * FROM dbo.Books_Books)
	 EXEC('INSERT INTO dbo.Tmp_Books_Books (ID, Authors, Title, PublishingYear, Annotation, Language, OfficeID, DownloadLink, IsElectronic)
		SELECT ID, Authors, Title, PublishingYear, Annotation, Language, OfficeID, DownloadLink, IsElectronic FROM dbo.Books_Books WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.Books_BookThemes
	DROP CONSTRAINT FK_Books_BookThemes_Books_Books
GO
DROP TABLE dbo.Books_Books
GO
EXECUTE sp_rename N'dbo.Tmp_Books_Books', N'Books_Books', 'OBJECT' 
GO
ALTER TABLE dbo.Books_Books ADD CONSTRAINT
	PK_Books_Books PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Books_Books ADD CONSTRAINT
	FK_Books_Books_Offices FOREIGN KEY
	(
	OfficeID
	) REFERENCES dbo.Offices
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Связь книги с офисом.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Books_Books', N'CONSTRAINT', N'FK_Books_Books_Offices'
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Books_BookThemes ADD CONSTRAINT
	FK_Books_BookThemes_Books_Books FOREIGN KEY
	(
	BookID
	) REFERENCES dbo.Books_Books
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
DECLARE @v sql_variant 
SET @v = N'Связь с таблицей книг.'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'Books_BookThemes', N'CONSTRAINT', N'FK_Books_BookThemes_Books_Books'
GO
COMMIT

/* update data */

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Books_BookThemes_Books_Books]') AND parent_object_id = OBJECT_ID(N'[dbo].[Books_BookThemes]'))
ALTER TABLE [dbo].[Books_BookThemes] DROP CONSTRAINT [FK_Books_BookThemes_Books_Books]
GO

SET NOCOUNT ON

DECLARE @BookID int, 
		@TmpBookID int, 
		@BookTitle nvarchar(max),
		@Authors nvarchar(max),
		@PublishingYear int,
		@Annotation nvarchar(max),
		@Language nvarchar(100),
		@OfficeID int,
		@OwnerID int,
		@DownloadLink nvarchar(max),
		@IsElectronic bit


DECLARE books_cursor CURSOR FOR 
SELECT * FROM Books_Books

OPEN books_cursor

FETCH NEXT FROM books_cursor 
INTO @BookID, @Authors, @BookTitle, @PublishingYear, @Annotation, @Language, 
		@OfficeID, @DownloadLink, @IsElectronic --, @OwnerID

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO RequestObject (Title, OwnerID, OfficeID) 
		VALUES (@BookTitle, NULL /*@OwnerID*/, @OfficeID)

	SET @TmpBookID = IDENT_CURRENT('RequestObject');

	--PRINT @TmpBookID
	UPDATE Books_Books SET ID = @TmpBookID WHERE ID = @BookID;

	--PRINT @BookID

	UPDATE Books_BookThemes SET BookID = @TmpBookID WHERE BookID = @BookID;
	
	FETCH NEXT FROM books_cursor 
	INTO @BookID, @Authors, @BookTitle, @PublishingYear, @Annotation, @Language, 
		@OfficeID, @DownloadLink, @IsElectronic --, @OwnerID
END 
CLOSE books_cursor
DEALLOCATE books_cursor

GO

ALTER TABLE [dbo].[Books_BookThemes] WITH CHECK ADD  CONSTRAINT [FK_Books_BookThemes_Books_Books] FOREIGN KEY([BookID])
REFERENCES [dbo].[Books_Books] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Books_BookThemes] CHECK CONSTRAINT [FK_Books_BookThemes_Books_Books]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Связь с таблицей книг.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Books_BookThemes', @level2type=N'CONSTRAINT',@level2name=N'FK_Books_BookThemes_Books_Books'
GO

/****** Object:  StoredProcedure [dbo].[CreateRequest]    Script Date: 04/16/2010 15:11:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		VadimS
-- Description:	Create request for object.
-- =============================================
CREATE PROCEDURE [dbo].[CreateRequest]
	@UserID INT,
	@ObjectID INT,
	@IsTaken BIT,
	@Date DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OwnerID INT, @HolderID INT;
	DECLARE @LastOperation table(ID int NOT NULL,
								UserID int,
								ObjectID int NOT NULL,
								Date datetime not null,
								IsTaken bit not null);

	INSERT INTO @LastOperation
		SELECT * FROM Requests
		WHERE ObjectID = @ObjectID 
				AND Date = (SELECT MAX(DATE) FROM Requests WHERE ObjectID = @ObjectID)

	IF (@UserID IS NULL)
		SET @UserID = 0

	SET @HolderID = [dbo].GetRequestObjectHolderID(@ObjectID)

	SET @OwnerID = (SELECT OwnerID FROM RequestObject WHERE ID = @ObjectID)
	IF (@OwnerID IS NULL)
		SET @OwnerID = 0

	IF (@OwnerID = @UserID)
	BEGIN	
		SET @UserID = @HolderID
		SET @IsTaken = 0
	END

	-- Request Table does'n contain taken operation.
	IF ((SELECT Date FROM @LastOperation) is null AND @IsTaken = 0)
	BEGIN
		PRINT '-- NO OPERATIONS'
		RETURN
	END

	-- ALREADY RETURNED
	IF ((SELECT IsTaken FROM @LastOperation) = 0 AND @IsTaken = 0)
	BEGIN
		PRINT '-- ALREADY RETURNED'
		RETURN
	END

	-- ALREADY TAKEN
	IF ((SELECT IsTaken FROM @LastOperation) = 1 AND @IsTaken = 1
		AND @UserID = @HolderID)
	BEGIN
		PRINT '-- ALREADY TAKEN'
		RETURN
	END

	-- CANT RETURN cause holder another person
	IF ((SELECT IsTaken FROM @LastOperation) = 1 
		AND @IsTaken = 0 AND @HolderID <> @UserID)
	BEGIN
		PRINT '-- CANT RETURN'
		RETURN
	END

	IF (@UserID = 0)
		SET @UserID = NULL

	INSERT INTO Requests (UserID, ObjectID, Date, IsTaken) 
				VALUES (@UserID, @ObjectID, @Date, @IsTaken)
END