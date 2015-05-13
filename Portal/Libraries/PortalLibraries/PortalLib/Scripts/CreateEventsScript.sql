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
COMMIT

BEGIN TRANSACTION
GO

CREATE TABLE dbo.Events
	(
	ID int NOT NULL IDENTITY (1, 1),
	Date datetime NOT NULL,
	Description nvarchar(MAX) NULL,
	Title nvarchar(50) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Events ADD CONSTRAINT
	PK_Event PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

BEGIN TRANSACTION
GO

CREATE TABLE dbo.GroupEvents
	(
	ID int NOT NULL IDENTITY (1, 1),
	GroupID int NOT NULL,
	EventID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.GroupEvents ADD CONSTRAINT
	PK_GroupEvents PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.GroupEvents ADD CONSTRAINT
	FK_GroupEvents_Groups FOREIGN KEY
	(
	GroupID
	) REFERENCES dbo.Groups
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO

ALTER TABLE dbo.GroupEvents  ADD  CONSTRAINT 
	FK_GroupEvents_Events FOREIGN KEY
	(
	EventID
	)REFERENCES dbo.Events 
	(
	ID
	)ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE
GO

COMMIT

BEGIN TRANSACTION
GO
COMMIT

BEGIN TRANSACTION
GO
CREATE TABLE dbo.UserEvents
	(
	ID int NOT NULL IDENTITY (1, 1),
	UserID int NOT NULL,
	EventID int NOT NULL,
	IsIgnore bit NOT NULL CONSTRAINT [DF_UserEvents_IsIgnore]  DEFAULT ((0)),
	)  ON [PRIMARY]
GO

ALTER TABLE dbo.UserEvents ADD CONSTRAINT
	PK_UserEvents PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

ALTER TABLE dbo.UserEvents ADD CONSTRAINT
	FK_UserEvents_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO

ALTER TABLE dbo.UserEvents ADD CONSTRAINT
	FK_UserEvents_Events FOREIGN KEY
	(
	EventID
	) REFERENCES dbo.Events
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT