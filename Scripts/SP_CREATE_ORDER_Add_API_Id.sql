
ALTER PROCEDURE [dbo].[SP_CREATE_ORDER]
--DECLARE
	@OrdNo varchar(50) NULL,
	@OrdDate datetime NULL,
	@OrdDesc varchar(50) NULL,
	@OrdType varchar(50) NULL,
	@OrdTerm varchar(50) NULL,
	@OrdTermDesc varchar(50) NULL,
	@OrdStat int NULL,
	@OrderStatus varchar(50) NULL,
	@Origin varchar(50) NULL,
	--@PromDate datetime NULL,
	@CompName varchar(50) NULL,
	@BranchNo varchar(50) NULL,
	@BranchName varchar(50) NULL,
	@BranchAddr1 varchar(50) NULL,
	@BranchAddr2 varchar(50) NULL,
	@BranchTel varchar(50) NULL,
	@BranchFax varchar(50) NULL,
	@BranchEmail varchar(50) NULL,
	@BranchVat varchar(50) NULL,
	@VendorRef varchar(50) NULL,
	@VendorNo varchar(50) NULL,
	@VendorName varchar(50) NULL,
	@VendorAddr1 varchar(50) NULL,
	@VendorAddr2 varchar(50) NULL,
	@VendorSuburb varchar(50) NULL,
	@VendorCity varchar(50) NULL,
	@VendorContact varchar(50) NULL,
	@TotLines int NULL,
	@TotQty numeric(10, 2) NULL,
	@TotExcl numeric(10, 2) NULL,
	@TotTax numeric(10, 2) NULL,
	@TotVal numeric(10, 2) NULL,
	@DelivAddr1 varchar(50) NULL,
	@DelivAddr2 varchar(50) NULL,
	@DelivSuburb varchar(50) NULL,
	@DelivCity varchar(50) NULL,
	@ConfirmInd bit NULL,
	@CompID varchar(50) NULL,
	@ResendOrder bit NULL,
	@Processed bit NULL,
	@API_Id varchar(100)

AS

DECLARE @OrderCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @OrderCount = COUNT(*) FROM Orders WHERE OrdNo = @OrdNo

	IF @OrderCount = 0

		BEGIN
			INSERT INTO ORDERS([Id],[OrdNo],[OrdDate],[OrdDesc],[OrdType],[OrdTerm],[OrdTermDesc],[OrdStat],[OrderStatus],[Origin],[PromDate],[CompName],[BranchNo],[BranchName],[BranchAddr1],[BranchAddr2],[BranchTel],[BranchFax],[BranchEmail],[BranchVat],[VendorRef],[VendorNo],[VendorName],[VendorAddr1],[VendorAddr2],[VendorSuburb],[VendorCity],[VendorContact],[TotLines],[TotQty],[TotExcl],[TotTax],[TotVal],[DelivAddr1],[DelivAddr2],[DelivSuburb],[DelivCity],[ConfirmInd],[CompID],[ResendOrder],[Processed], [API_Id])
			VALUES (@ID,	@OrdNo  ,@OrdDate  ,@OrdDesc  ,@OrdType  ,@OrdTerm  ,@OrdTermDesc  ,@OrdStat ,@OrderStatus  ,@Origin  ,@OrdDate  ,@CompName  ,@BranchNo  ,@BranchName  ,@BranchAddr1  ,@BranchAddr2  ,@BranchTel  ,@BranchFax  ,@BranchEmail  ,@BranchVat  ,@VendorRef  ,@VendorNo  ,@VendorName  ,@VendorAddr1  ,@VendorAddr2  ,@VendorSuburb  ,@VendorCity  ,@VendorContact  ,@TotLines ,@TotQty  ,@TotExcl  ,@TotTax  ,@TotVal  ,@DelivAddr1  ,@DelivAddr2  ,@DelivSuburb  ,@DelivCity  ,@ConfirmInd  ,@CompID  ,@ResendOrder,@Processed, @API_Id )
		END
END
