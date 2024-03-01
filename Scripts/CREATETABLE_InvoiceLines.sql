USE [BizIntegrator]
GO

/****** Object:  Table [dbo].[InvoiceLines]    Script Date: 2024/02/28 23:01:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceLines](
	[INLineID] [uniqueidentifier] NOT NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[InvoiceId] [int] NULL,
	[WarehouseCode] [varchar](20) NULL,
	[ItemCode] [varchar](20) NULL,
	[ModuleCode] [varchar](20) NULL,
	[LineDescription] [varchar](100) NULL,
	[UnitPriceExcl] [numeric](20, 5) NULL,
	[UnitPriceIncl] [numeric](20, 5) NULL,
	[Quantity] [int] NULL,
	[UnitOfMeasure] [varchar](20) NULL,
	[LineNetTotalOrderedInVat] [numeric](20, 5) NULL,
	[LineNetTotalOrderedExVat] [numeric](20, 5) NULL,
	[LineNetTotalProcessedInVat] [numeric](20, 5) NULL,
	[LineNetTotalProcessedExVat] [numeric](20, 5) NULL,
	[LineNotes] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[INLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


