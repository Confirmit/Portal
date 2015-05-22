/****** Object:  StoredProcedure [dbo].[GetInternetUserID]    Script Date: 08/16/2007 13:09:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInternetUserID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetInternetUserID]
GO

/****** Object:  StoredProcedure [dbo].[GetInternetUserID]    Script Date: 08/16/2007 13:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Yakimov
-- Create date: 16.08.2007
-- Description:	Возвращает ID Интернет-пользователя по его имени.
-- =============================================
CREATE PROCEDURE [dbo].[GetInternetUserID] 
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP (1) ID
	FROM Users
	WHERE Lower(DomainName) = Lower(@UserName)
END
GO