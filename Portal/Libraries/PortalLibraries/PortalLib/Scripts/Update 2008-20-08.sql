/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
USE [Portal]
go

BEGIN TRANSACTION
GO

ALTER TABLE dbo.News ADD
	PostID int NULL
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NewsAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NewsID] [int] NOT NULL,
	[FileName] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
ALTER TABLE [dbo].[NewsAttachments]  WITH CHECK ADD  CONSTRAINT [FK_NewsAttachments_News] FOREIGN KEY([NewsID])
REFERENCES [dbo].[News] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[NewsAttachments] CHECK CONSTRAINT [FK_NewsAttachments_News]
*/

CREATE PROCEDURE [dbo].[GetNewsAttachments]
                   @NewsID int
AS
BEGIN
    SELECT * 
    FROM NewsAttachments
    WHERE NewsID = @NewsID
END
GO

CREATE PROCEDURE [dbo].[CleanAttachments] 
AS
BEGIN
DELETE
	FROM NewsAttachments WHERE NewsID = 0
END
GO

CREATE PROCEDURE [dbo].[GetUnnecessaryAttachments] 
AS
BEGIN
	select * from NewsAttachments where NewsID = 0
END
GO

INSERT INTO [dbo].[AllowTags] (tagName) VALUES ('BR')
GO

INSERT INTO [dbo].[AllowTags] (tagName) VALUES ('br')
GO

COMMIT

BEGIN TRANSACTION
GO

ALTER TABLE dbo.UptimeEvents
	DROP CONSTRAINT FK_Events_Users
GO

COMMIT

BEGIN TRANSACTION
GO

ALTER TABLE dbo.UptimeEvents WITH NOCHECK ADD CONSTRAINT
	FK_Events_Users FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.Users
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
