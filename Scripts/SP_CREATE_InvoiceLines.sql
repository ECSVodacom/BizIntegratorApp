USE [BizIntegrator]
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_InvoiceLine]    Script Date: 2024/02/28 22:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SP_CREATE_InvoiceLine]
@InvoiceNumber varchar(20),
@InvoiceId int,
@WarehouseCode varchar(20),
@ItemCode varchar(20),
@ModuleCode varchar(20),
@LineDescription varchar(100),
@UnitPriceExcl numeric(20,5),
@UnitPriceIncl numeric(20,5),
@Quantity int ,
@UnitOfMeasure varchar(20),
@LineNetTotalOrderedInVat numeric(20,5),
@LineNetTotalOrderedExVat numeric(20,5),
@LineNetTotalProcessedInVat numeric(20,5),
@LineNetTotalProcessedExVat numeric(20,5),
@LineNotes varchar(100)

AS
DECLARE @InvoiceLineCnt int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @InvoiceLineCnt = COUNT(*) FROM InvoiceLines WHERE [InvoiceNumber] +  [InvoiceId]= @InvoiceNumber + @InvoiceId

	IF @InvoiceLineCnt = 0
		BEGIN
			INSERT INTO InvoiceLines([INLineID],[InvoiceNumber],[InvoiceId],[WarehouseCode],[ItemCode],[ModuleCode],[LineDescription],[UnitPriceExcl]
									,[UnitPriceIncl],[Quantity],[UnitOfMeasure],[LineNetTotalOrderedInVat],[LineNetTotalOrderedExVat]
									,[LineNetTotalProcessedInVat],[LineNetTotalProcessedExVat],[LineNotes])
			VALUES (@ID,@InvoiceNumber,@InvoiceId,@WarehouseCode,@ItemCode,@ModuleCode,@LineDescription,@UnitPriceExcl
									,@UnitPriceIncl,@Quantity,@UnitOfMeasure,@LineNetTotalOrderedInVat,@LineNetTotalOrderedExVat
									,@LineNetTotalProcessedInVat,@LineNetTotalProcessedExVat,@LineNotes)
		END
END
