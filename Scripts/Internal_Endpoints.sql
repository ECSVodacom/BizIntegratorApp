USE [BizIntegrator]
GO

/****** Object:  Table [dbo].[Internal_Endpoints]    Script Date: 2024/03/01 09:57:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Internal_Endpoints](
	[Id] [uniqueidentifier] NOT NULL,
	[API_Id] [uniqueidentifier] NOT NULL,
	[EndPoint] [varchar](1000) NULL,
	[TransactionType] [varchar](50) NULL,
	[HttpVerb] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


