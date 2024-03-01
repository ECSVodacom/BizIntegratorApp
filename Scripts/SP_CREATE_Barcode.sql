
CREATE PROCEDURE [dbo].[SP_CREATE_StockBarcode]
--DECLARE
@ProductCode varchar(50),
@ProductDescription [varchar](100) NULL,
@BottleBarcode [varchar](20) NULL,
@CaseBarcode [varchar](20) NULL,
@UomBottle [varchar](20) NULL,
@UomCase [varchar](20) NULL,
@UnitsPerCase [int] NULL,
@API_Id VARCHAR(100)

AS
DECLARE @StockCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @StockCount = COUNT(*) FROM StockList WHERE ProductCode = @ProductCode

	IF @StockCount = 0
		BEGIN
			INSERT INTO [StockBarcodes]([SBID], [ProductCode],[ProductDescription],[BottleBarcode],[CaseBarcode],[UomBottle],[UomCase],[UnitsPerCase],[API_Id])
			VALUES (@ID,@ProductCode,@ProductDescription,@BottleBarcode,@CaseBarcode,@UomBottle,@UomCase,@UnitsPerCase,@API_Id)
		END
END
