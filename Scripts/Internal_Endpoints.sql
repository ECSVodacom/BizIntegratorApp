
CREATE TABLE [dbo].[Internal_Endpoints](
	[Id] [uniqueidentifier] NOT NULL,
	[API_Id] [uniqueidentifier] NOT NULL,
	[EndPoint] [varchar](1000) NULL,
	[TransactionType] [varchar](15) NULL,
	[HttpVerb] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


