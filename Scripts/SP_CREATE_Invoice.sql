USE [BizIntegrator]
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_Invoice]    Script Date: 2024/03/04 14:31:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SP_CREATE_Invoice]
@InvoiceNumber varchar(20),
@InvoiceId int,
        @InvoiceDate datetime,
        @DocumentState int,
        @OrderNumber varchar(20),
        @ExternalOrderNumber varchar(20),
        @CustomerCode varchar(20),
        @GrossTotalInVat numeric(20,5),
        @GrossTotalExVat numeric(20,5),
        @GrossTaxTotal numeric(20,5),
        @DiscountAmountInVat numeric(20,5),
        @DiscountAmountExVat numeric(20,5),
        @NetTotalExVat numeric(20,5),
        @NetTaxTotal numeric(20,5),
        @TotalInvoiceRounding int,
        @NetTotalInVat numeric(20,5),
		@Processed bit NULL,
		@API_Id varchar(100)

AS
DECLARE @InvoiceCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	
if ((select count(InvoiceId) from Invoices where InvoiceId = @InvoiceId and OrderNumber = @OrderNumber) = 0)
begin
	
			INSERT INTO Invoices(INId,InvoiceNumber,InvoiceId,InvoiceDate,DocumentState,OrderNumber,ExternalOrderNumber,CustomerCode,GrossTotalInVat
								,GrossTotalExVat,GrossTaxTotal,DiscountAmountInVat,DiscountAmountExVat,NetTotalExVat,NetTaxTotal
								,TotalInvoiceRounding,NetTotalInVat, Processed, API_Id)
			VALUES (@Id,@InvoiceNumber,@InvoiceId,@InvoiceDate,@DocumentState,@OrderNumber,@ExternalOrderNumber,@CustomerCode,@GrossTotalInVat
								,@GrossTotalExVat,@GrossTaxTotal,@DiscountAmountInVat,@DiscountAmountExVat,@NetTotalExVat
								,@NetTaxTotal,@TotalInvoiceRounding,@NetTotalInVat, @Processed, @API_Id)
		END
END
