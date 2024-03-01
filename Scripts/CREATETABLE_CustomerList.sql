USE [BizIntegrator]
GO

/****** Object:  Table [dbo].[CustomerList]    Script Date: 2024/02/28 23:15:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerList](
	[CUSTID] [uniqueidentifier] NOT NULL,
	[CustomerCode] [varchar](20) NULL,
	[CustomerName] [varchar](100) NULL,
	[PhysicalAddress] [varchar](1000) NULL,
	[Email] [varchar](50) NULL,
	[UcARBrnchNo] [varchar](50) NULL,
	[BranchCode] [varchar](20) NULL,
	[DateTimeStamp] [varchar](50) NULL,
	[GroupCode] [varchar](20) NULL,
	[GroupDescription] [varchar](50) NULL,
	[Area] [varchar](20) NULL,
	[AreaDescription] [varchar](20) NULL,
	[UlARWHLinked] [varchar](20) NULL,
	[API_Id] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[CUSTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


