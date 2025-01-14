USE [Portal]
GO

CREATE TABLE [dbo].[ConferenceHall](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OfficeID] [int] NOT NULL,
 CONSTRAINT [PK_ConferenceHall] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ConferenceHall]  WITH CHECK ADD  CONSTRAINT [FK_ConferenceHall_Offices] FOREIGN KEY([OfficeID])
REFERENCES [dbo].[Offices] ([ID])
GO
ALTER TABLE [dbo].[ConferenceHall] CHECK CONSTRAINT [FK_ConferenceHall_Offices]

CREATE TABLE [dbo].[Arrangement](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ConferenceHallID] [int] NOT NULL,
	[TimeBegin] [datetime] NOT NULL,
	[TimeEnd] [datetime] NOT NULL,
	[ListOfGuests] [nvarchar](max) NULL,
	[Equipment] [nvarchar](max) NULL,
 CONSTRAINT [PK_Arrangement] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Arrangement]  WITH CHECK ADD  CONSTRAINT [FK_Arrangement_ConferenceHall] FOREIGN KEY([ConferenceHallID])
REFERENCES [dbo].[ConferenceHall] ([ID])
GO
ALTER TABLE [dbo].[Arrangement] CHECK CONSTRAINT [FK_Arrangement_ConferenceHall]

CREATE TABLE [dbo].[ArrangementDate](
	[ArrangementID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_ArrangementDate] PRIMARY KEY CLUSTERED 
(
	[ArrangementID] ASC,
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ArrangementDate]  WITH CHECK ADD  CONSTRAINT [FK_ArrangementDate_Arrangement] FOREIGN KEY([ArrangementID])
REFERENCES [dbo].[Arrangement] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ArrangementDate] CHECK CONSTRAINT [FK_ArrangementDate_Arrangement]

INSERT INTO [Portal].[dbo].[Groups]
           ([GroupID],[Name]
           ,[Description])
     VALUES ('OfficeArrangementsEditor','<MLText><Text lang="ru"></Text><Text lang="en"></Text></MLText>','<MLText><Text lang="ru">Пользователь может редактировать мероприятия</Text><Text lang="en">User can edit arrangements</Text></MLText>')

INSERT INTO [Portal].[dbo].[Groups]
           ([GroupID],[Name]
           ,[Description])
     VALUES ('GeneralArrangementsEditor','<MLText><Text lang="ru"></Text><Text lang="en"></Text></MLText>','<MLText><Text lang="ru"></Text><Text lang="en"></Text></MLText>')