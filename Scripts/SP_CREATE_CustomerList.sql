USE [BizIntegrator]
GO
/****** Object:  StoredProcedure [dbo].[SP_CREATE_CustomerList]    Script Date: 2024/02/28 23:17:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_CREATE_CustomerList]
--DECLARE
@CustomerCode varchar(20),
@CustomerName varchar(100),
@PhysicalAddress varchar(1000),
@Email varchar(20),
@UcARBrnchNo varchar(50),
@BranchCode varchar(20) ,
@DateTimeStamp DateTime,
@GroupCode varchar(20),
@GroupDescription varchar(50),
@Area varchar(20),
@AreaDescription varchar(20),
@UlARWHLinked varchar(20) NULL,
@API_Id varchar(100)

AS
DECLARE @CustomerCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @CustomerCount = COUNT(*) FROM CustomerList WHERE CustomerCode = @CustomerCode

	IF @CustomerCount = 0
		BEGIN
			INSERT INTO CustomerList([CUSTID],[CustomerCode],[CustomerName],[PhysicalAddress],[Email],[UcARBrnchNo],[BranchCode],[DateTimeStamp],[GroupCode],[GroupDescription],[Area],[AreaDescription],[UlARWHLinked],[API_Id])
			VALUES (@ID,@CustomerCode,@CustomerName ,@PhysicalAddress ,@Email,@UcARBrnchNo ,@BranchCode  ,@DateTimeStamp ,@GroupCode ,@GroupDescription ,@Area,@AreaDescription,@UlARWHLinked,@API_Id)
		END
END
