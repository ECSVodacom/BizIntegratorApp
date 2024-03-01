USE [BizIntegrator]
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_StockList]    Script Date: 2024/02/24 00:27:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_CREATE_StockList]
--DECLARE
@ProductCode varchar(50)
      ,@ProductName varchar(100)
      ,@AlternateName varchar(100)
      ,@PriceInVat numeric(10,5)
      ,@PriceExVat numeric(10,5)
      ,@BottleBarcode varchar(20)
      ,@CaseBarcode varchar(20)
	  ,@API_Id VARCHAR(100)
AS
DECLARE @StockCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @StockCount = COUNT(*) FROM StockList WHERE ProductCode = @ProductCode

	IF @StockCount = 0
		BEGIN
			INSERT INTO StockList([STID], [ProductCode],[ProductName],[AlternateName],[PriceInVat],[PriceExVat],[BottleBarcode],[CaseBarcode],[API_Id])
			VALUES (@ID,@ProductCode ,@ProductName ,@AlternateName ,@PriceInVat ,@PriceExVat ,@BottleBarcode ,@CaseBarcode,@API_Id)
		END
END
