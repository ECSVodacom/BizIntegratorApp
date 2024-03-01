USE [BizIntegrator]
GO

/****** Object:  Table [dbo].[Invoices]    Script Date: 2024/02/28 23:00:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Invoices](
	[INId] [uniqueidentifier] NOT NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[InvoiceId] [int] NULL,
	[InvoiceDate] [datetime] NULL,
	[DocumentState] [varchar](20) NULL,
	[OrderNumber] [varchar](20) NULL,
	[ExternalOrderNumber] [varchar](20) NULL,
	[CustomerCode] [varchar](20) NULL,
	[GrossTotalInVat] [numeric](20, 5) NULL,
	[GrossTotalExVat] [numeric](20, 5) NULL,
	[GrossTaxTotal] [numeric](20, 5) NULL,
	[DiscountAmountInVat] [numeric](20, 5) NULL,
	[DiscountAmountExVat] [numeric](20, 5) NULL,
	[NetTotalExVat] [numeric](20, 5) NULL,
	[NetTaxTotal] [numeric](20, 5) NULL,
	[TotalInvoiceRounding] [int] NULL,
	[NetTotalInVat] [numeric](20, 5) NULL,
	[Processed] [bit] NULL,
	[API_Id] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[INId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


