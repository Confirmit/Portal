/****** Object:  Table [dbo].[Reports]    Script Date: 09/03/2007 15:45:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reports]') AND type in (N'U'))
DROP TABLE [dbo].[Reports]
GO

/****** Object:  Table [dbo].[MailsStorage]    Script Date: 09/03/2007 15:46:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MailsStorage]') AND type in (N'U'))
DROP TABLE [dbo].[MailsStorage]
GO

/****** Object:  Table [dbo].[MailsStorage]    Script Date: 09/03/2007 15:46:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailsStorage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsSend] [bit] NOT NULL,
	[FromAddress] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[ToAddress] [nvarchar](500) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Subject] [nvarchar](500) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Body] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsHTML] [bit] NOT NULL,
	[MessageType] [int] NOT NULL
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Уникальный идентификатор.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'ID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата постановки в таблицу.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'Date'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Отослано ли письмо' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'IsSend'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Адрес отправителя' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'FromAddress'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Одреса получателей, разделенные запятой.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'ToAddress'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тема письма.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'Subject'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Текст письма.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'Body'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имеет ли письмо формат HTML.' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'IsHTML'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип письма (рассылки).' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'MailsStorage', @level2type=N'COLUMN', @level2name=N'MessageType'

GO