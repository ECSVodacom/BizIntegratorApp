
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
	SELECT @InvoiceCount = COUNT(*) FROM Invoices WHERE InvoiceNumber + OrderNumber = @InvoiceNumber + @OrderNumber

	IF @InvoiceCount = 0
		BEGIN
			INSERT INTO Invoices(INId,InvoiceNumber,InvoiceId,InvoiceDate,DocumentState,OrderNumber,ExternalOrderNumber,CustomerCode,GrossTotalInVat
								,GrossTotalExVat,GrossTaxTotal,DiscountAmountInVat,DiscountAmountExVat,NetTotalExVat,NetTaxTotal
								,TotalInvoiceRounding,NetTotalInVat, Processed, API_Id)
			VALUES (@Id,@InvoiceNumber,@InvoiceId,@InvoiceDate,@DocumentState,@OrderNumber,@ExternalOrderNumber,@CustomerCode,@GrossTotalInVat
								,@GrossTotalExVat,@GrossTaxTotal,@DiscountAmountInVat,@DiscountAmountExVat,@NetTotalExVat
								,@NetTaxTotal,@TotalInvoiceRounding,@NetTotalInVat, @Processed, @API_Id)
		END
END
