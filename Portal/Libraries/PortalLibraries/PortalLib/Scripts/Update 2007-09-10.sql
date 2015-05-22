/****** Object:  Table [dbo].[Offices]    Script Date: 09/10/2007 13:36:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Offices]') AND type in (N'U'))
BEGIN
	ALTER TABLE [Offices] ADD StatusesServiceUserName nvarchar(50) null
	ALTER TABLE [Offices] ADD StatusesServicePassword nvarchar(50) null
END
GO