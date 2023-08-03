USE [FMS]
GO

/****** Object:  Table [dbo].[M_Reminder_Type]    Script Date: 03/08/2023 15:02:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[M_Reminder_Type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReminderName] [varchar](300) NULL,
	[IsDeleted] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [varchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[T_Reminder]    Script Date: 03/08/2023 15:02:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_Reminder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReminderName] [varchar](300) NULL,
	[ReminderRef1] [varchar](300) NULL,
	[ReminderRef2] [varchar](300) NULL,
	[ReminderRef3] [varchar](300) NULL,
	[ReminderType] [int] NULL,
	[ReminderDateStart] [date] NULL,
	[ReminderDueDate] [date] NULL,
	[Day1] [bit] NULL,
	[Day2] [bit] NULL,
	[Day3] [bit] NULL,
	[Day4] [bit] NULL,
	[Day5] [bit] NULL,
	[Day6] [bit] NULL,
	[Day7] [bit] NULL,
	[IsRepeat] [int] NULL,
	[Remark] [varchar](800) NULL,
	[IsDeleted] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [varchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[T_Reminder_Mail]    Script Date: 03/08/2023 15:02:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_Reminder_Mail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReminderId] [varchar](300) NULL,
	[MailAddress] [varchar](300) NULL,
	[MailName] [varchar](300) NULL,
	[IsDeleted] [int] NULL,
	[CreateDate] [datetime] NULL,
	[CreateUser] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[M_Reminder_Type] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[T_Reminder] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[T_Reminder_Mail] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO


