
ALTER PROCEDURE [dbo].[SP_CREATE_StockBarcode]
--DECLARE
@ProductCode varchar(50),
@ProductDescription varchar(100),
@BottleBarcode varchar(20),
@CaseBarcode varchar(20),
@UomBottle varchar(20),
@UomCase varchar(20),
@UnitsPerCase int,
@API_Id VARCHAR(100)

AS

DECLARE @StockCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @StockCount = COUNT(*) FROM [StockBarcodes] WHERE ProductCode = @ProductCode

	IF @StockCount = 0
		BEGIN
			INSERT INTO [StockBarcodes]([SBID], [ProductCode],[ProductDescription],[BottleBarcode],[CaseBarcode],[UomBottle],[UomCase],[UnitsPerCase],[API_Id])
			VALUES (@ID,@ProductCode,@ProductDescription,@BottleBarcode,@CaseBarcode,@UomBottle,@UomCase,@UnitsPerCase,@API_Id)
		END
END
