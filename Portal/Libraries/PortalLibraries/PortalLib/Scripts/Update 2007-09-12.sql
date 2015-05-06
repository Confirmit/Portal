/****** Object:  Table [dbo].[Holiday]    Script Date: 09/12/2007 14:46:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Holiday]') AND type in (N'U'))
DROP TABLE [dbo].[Holiday]
GO

/****** Object:  Table [dbo].[Calendar]    Script Date: 09/12/2007 14:46:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[WorkTime] [datetime] NOT NULL,
	[Comment] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NULL,
 CONSTRAINT [PK_Calendar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[GetHoliday]    Script Date: 09/12/2007 14:47:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetHoliday]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetHoliday]
GO

/****** Object:  StoredProcedure [dbo].[GetCalendarDate]    Script Date: 09/12/2007 14:47:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCalendarDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCalendarDate]
GO

/****** Object:  StoredProcedure [dbo].[GetCalendarDate]    Script Date: 09/12/2007 14:47:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 2007.08.06
-- Description:	Returns record about calendar date.
-- =============================================
CREATE PROCEDURE [dbo].[GetCalendarDate] 
	@Date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Calendar WHERE FLOOR(CONVERT( float, Date)) = FLOOR(CONVERT( float, @Date))
END
GO