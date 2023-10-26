CREATE DATABASE BizIntegrator
GO

USE BizIntegrator
GO

CREATE TABLE [dbo].[APIs](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[AccountKey] [nvarchar](max) NULL,
	[Username] [varchar](100) NULL,
	[Password] [varchar](100) NULL,
	[PublicKey] [nvarchar](max) NULL,
	[PrivateKey] [nvarchar](max) NULL,
	[EndPointBase] [varchar](1000) NULL,
	[AuthenticationType] [varchar](50) NULL,
	[UseAPIKey] [char](1) NULL,
	[EanCode] [varchar](20) ,
	[IsActive] [bit] NULL
	)
GO

CREATE TABLE [dbo].[APIEndpoints](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[API_Id] UNIQUEIDENTIFIER NOT NULL,
	[EndPoint] [varchar](1000) NULL,
	[TransactionType] [varchar](15) NULL,
	[HttpVerb] [varchar](15) NULL
	)
GO

CREATE TABLE [dbo].[Suppliers](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[API_Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[EanCode] [varchar](14) NOT NULL,
	[IsEnabled] [bit] NOT NULL
	)
GO

ALTER TABLE [dbo].[Suppliers]  WITH CHECK ADD  CONSTRAINT [FK_Suppliers_APIs] FOREIGN KEY([API_Id])
REFERENCES [dbo].[APIs] ([Id])
GO

ALTER TABLE [dbo].[Suppliers] CHECK CONSTRAINT [FK_Suppliers_APIs]
GO


CREATE TABLE [dbo].[Orders](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[OrdNo] [varchar](50) NULL,
	[OrdDate] [datetime] NULL,
	[OrdDesc] [varchar](50) NULL,
	[OrdType] [varchar](50) NULL,
	[OrdTerm] [varchar](50) NULL,
	[OrdTermDesc] [varchar](50) NULL,
	[OrdStat] [int] NULL,
	[OrderStatus] [varchar](50) NULL,
	[Origin] [varchar](50) NULL,
	[PromDate] [datetime] NULL,
	[CompName] [varchar](50) NULL,
	[BranchNo] [varchar](50) NULL,
	[BranchName] [varchar](50) NULL,
	[BranchAddr1] [varchar](50) NULL,
	[BranchAddr2] [varchar](50) NULL,
	[BranchTel] [varchar](50) NULL,
	[BranchFax] [varchar](50) NULL,
	[BranchEmail] [varchar](50) NULL,
	[BranchVat] [varchar](50) NULL,
	[VendorRef] [varchar](50) NULL,
	[VendorNo] [varchar](50) NULL,
	[VendorName] [varchar](50) NULL,
	[VendorAddr1] [varchar](50) NULL,
	[VendorAddr2] [varchar](50) NULL,
	[VendorSuburb] [varchar](50) NULL,
	[VendorCity] [varchar](50) NULL,
	[VendorContact] [varchar](50) NULL,
	[TotLines] [int] NULL,
	[TotQty] [numeric](10, 2) NULL,
	[TotExcl] [numeric](10, 2) NULL,
	[TotTax] [numeric](10, 2) NULL,
	[TotVal] [numeric](10, 2) NULL,
	[DelivAddr1] [varchar](50) NULL,
	[DelivAddr2] [varchar](50) NULL,
	[DelivSuburb] [varchar](50) NULL,
	[DelivCity] [varchar](50) NULL,
	[BuyerNote] [varchar](50) NULL,
	[ConfirmInd] [char](1) NULL,
	[CompID] [varchar](50) NULL,
	[ResendOrder] [char](1) NULL,
	[Processed] [char](1) NULL
	)
GO

CREATE TABLE OrderLines
(
Id 							UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
,OrderNo					varchar(50) not null
,OrdLn						int NULL
,ItemNo						Varchar(50)  NULL
,ItemDesc					Varchar(50)  NULL
,MfrItem					Varchar(50)  NULL
,QtyConv					numeric(10,2) NULL
,OrdQty						numeric(10,2) NULL
,PurcUom					Varchar(50)  NULL
,PurcUomConv				numeric(10,2) NULL
,TaxCde						Varchar(50)  NULL
,TaxRate					numeric(10,2) NULL
,UnitPrc					numeric(10,2) NULL
,LineTotExcl				numeric(10,2) NULL
,LineTotTax					numeric(10,2) NULL
,LineTotVal					numeric(10,2) NULL
)
GO

CREATE TABLE [dbo].[MessageException](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ExceptionDate] [datetime] NULL,
	[Exception] [varchar](50) NULL,
	[Method] [varchar](50) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SupplierVendors](
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
	[SupplierId] UNIQUEIDENTIFIER NOT NULL,
	[EanCode] [nvarchar](13) NOT NULL,
	--[SystemAdded] [bit] NULL,
	[VendorName] [varchar](50) NULL
	)
GO

ALTER TABLE [dbo].[SupplierVendors]  WITH CHECK ADD  CONSTRAINT [FK_SupplierVendors_Suppliers] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO

ALTER TABLE [dbo].[SupplierVendors] CHECK CONSTRAINT [FK_SupplierVendors_Suppliers]
GO

CREATE PROCEDURE SP_CREATE_ORDER
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
	@PromDate datetime NULL,
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
	@Processed bit NULL

AS

DECLARE @OrderCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()
 BEGIN
	SELECT @OrderCount = COUNT(*) FROM Orders WHERE OrdNo = @OrdNo

	IF @OrderCount = 0

		BEGIN
			INSERT INTO ORDERS([Id],[OrdNo],[OrdDate],[OrdDesc],[OrdType],[OrdTerm],[OrdTermDesc],[OrdStat],[OrderStatus],[Origin],[PromDate],[CompName],[BranchNo],[BranchName],[BranchAddr1],[BranchAddr2],[BranchTel],[BranchFax],[BranchEmail],[BranchVat],[VendorRef],[VendorNo],[VendorName],[VendorAddr1],[VendorAddr2],[VendorSuburb],[VendorCity],[VendorContact],[TotLines],[TotQty],[TotExcl],[TotTax],[TotVal],[DelivAddr1],[DelivAddr2],[DelivSuburb],[DelivCity],[ConfirmInd],[CompID],[ResendOrder],[Processed])
			VALUES (@ID,	@OrdNo  ,@OrdDate  ,@OrdDesc  ,@OrdType  ,@OrdTerm  ,@OrdTermDesc  ,@OrdStat ,@OrderStatus  ,@Origin  ,@PromDate  ,@CompName  ,@BranchNo  ,@BranchName  ,@BranchAddr1  ,@BranchAddr2  ,@BranchTel  ,@BranchFax  ,@BranchEmail  ,@BranchVat  ,@VendorRef  ,@VendorNo  ,@VendorName  ,@VendorAddr1  ,@VendorAddr2  ,@VendorSuburb  ,@VendorCity  ,@VendorContact  ,@TotLines ,@TotQty  ,@TotExcl  ,@TotTax  ,@TotVal  ,@DelivAddr1  ,@DelivAddr2  ,@DelivSuburb  ,@DelivCity  ,@ConfirmInd  ,@CompID  ,@ResendOrder,@Processed )
		END
END
GO

CREATE PROCEDURE [dbo].[SP_CREATE_ORDER_LINES]
--DECLARE
	@OrdLn INT NULL,
	@OrderNo varchar(50),
      @ItemNo varchar(50),
      @ItemDesc varchar(50),
      @MfrItem varchar(50),
      @QtyConv varchar(50),
      @OrdQty varchar(50),
      @PurcUom varchar(50),
      @PurcUomConv varchar(50),
      @TaxCde varchar(50),
      @TaxRate varchar(50),
      @UnitPrc varchar(50),
      @LineTotExcl varchar(50),
      @LineTotTax varchar(50),
      @LineTotVal varchar(50)

AS

DECLARE @OrderLineCount int = 0

DECLARE @ID UNIQUEIDENTIFIER = NEWID()

BEGIN

	SELECT @OrderLineCount = COUNT(*) FROM OrderLines WHERE OrderNo + CONVERT(VARCHAR(10),OrdLn) = @OrderNo + CONVERT(VARCHAR(10),@OrdLn)

	IF @OrderLineCount = 0
		BEGIN
			INSERT INTO OrderLines([Id],[OrdLn], [OrderNo],[ItemNo],[ItemDesc],[MfrItem],[QtyConv],[OrdQty],[PurcUom],[PurcUomConv],[TaxCde],[TaxRate],[UnitPrc],[LineTotExcl],[LineTotTax],[LineTotVal])
			VALUES (@ID, @OrdLn, @OrderNo ,@ItemNo ,@ItemDesc ,@MfrItem ,@QtyConv ,@OrdQty ,@PurcUom ,@PurcUomConv ,@TaxCde ,@TaxRate ,@UnitPrc ,@LineTotExcl ,@LineTotTax ,@LineTotVal )
		End
END
GO


