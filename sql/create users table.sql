USE [blog]
GO

/****** Object:  Table [dbo].[users]    Script Date: 07/04/2012 17:25:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [varchar](20) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

