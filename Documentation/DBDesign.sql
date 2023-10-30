CREATE DATABASE BizIntegrator
GO

USE BizIntegrator
GO

CREATE TABLE [dbo].[APIs] (
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,[Name] [varchar](20) NOT NULL
	,[AccountKey] [nvarchar](max) NULL
	,[Username] [varchar](100) NULL
	,[Password] [varchar](100) NULL
	,[PublicKey] [nvarchar](max) NULL
	,[PrivateKey] [nvarchar](max) NULL
	,[EndPointBase] [varchar](1000) NULL
	,[AuthenticationType] [varchar](50) NULL
	,[UseAPIKey] [char](1) NULL
	,[EanCode] [varchar](20)
	,[IsActive] [bit] NULL
	)
GO

CREATE TABLE [dbo].[APIEndpoints] (
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,[API_Id] UNIQUEIDENTIFIER NOT NULL
	,[EndPoint] [varchar](1000) NULL
	,[TransactionType] [varchar](15) NULL
	,[HttpVerb] [varchar](15) NULL
	)
GO

CREATE TABLE [dbo].[Suppliers] (
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,[API_Id] UNIQUEIDENTIFIER NOT NULL
	,[Name] [varchar](50) NOT NULL
	,[EanCode] [varchar](14) NOT NULL
	,[IsEnabled] [bit] NOT NULL
	)
GO

ALTER TABLE [dbo].[Suppliers]
	WITH CHECK ADD CONSTRAINT [FK_Suppliers_APIs] FOREIGN KEY ([API_Id]) REFERENCES [dbo].[APIs]([Id])
GO

ALTER TABLE [dbo].[Suppliers] CHECK CONSTRAINT [FK_Suppliers_APIs]
GO

CREATE TABLE [dbo].[Orders] (
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,[OrdNo] [varchar](50) NULL
	,[OrdDate] [datetime] NULL
	,[OrdDesc] [varchar](50) NULL
	,[OrdType] [varchar](50) NULL
	,[OrdTerm] [varchar](50) NULL
	,[OrdTermDesc] [varchar](50) NULL
	,[OrdStat] [int] NULL
	,[OrderStatus] [varchar](50) NULL
	,[Origin] [varchar](50) NULL
	,[PromDate] [datetime] NULL
	,[CompName] [varchar](50) NULL
	,[BranchNo] [varchar](50) NULL
	,[BranchName] [varchar](50) NULL
	,[BranchAddr1] [varchar](50) NULL
	,[BranchAddr2] [varchar](50) NULL
	,[BranchTel] [varchar](50) NULL
	,[BranchFax] [varchar](50) NULL
	,[BranchEmail] [varchar](50) NULL
	,[BranchVat] [varchar](50) NULL
	,[VendorRef] [varchar](50) NULL
	,[VendorNo] [varchar](50) NULL
	,[VendorName] [varchar](50) NULL
	,[VendorAddr1] [varchar](50) NULL
	,[VendorAddr2] [varchar](50) NULL
	,[VendorSuburb] [varchar](50) NULL
	,[VendorCity] [varchar](50) NULL
	,[VendorContact] [varchar](50) NULL
	,[TotLines] [int] NULL
	,[TotQty] [numeric](10, 2) NULL
	,[TotExcl] [numeric](10, 2) NULL
	,[TotTax] [numeric](10, 2) NULL
	,[TotVal] [numeric](10, 2) NULL
	,[DelivAddr1] [varchar](50) NULL
	,[DelivAddr2] [varchar](50) NULL
	,[DelivSuburb] [varchar](50) NULL
	,[DelivCity] [varchar](50) NULL
	,[BuyerNote] [varchar](50) NULL
	,[ConfirmInd] [char](1) NULL
	,[CompID] [varchar](50) NULL
	,[ResendOrder] [char](1) NULL
	,[Processed] [char](1) NULL
	)
GO

CREATE TABLE OrderLines (
	Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,OrderNo VARCHAR(50) NOT NULL
	,OrdLn INT NULL
	,ItemNo VARCHAR(50) NULL
	,ItemDesc VARCHAR(50) NULL
	,MfrItem VARCHAR(50) NULL
	,QtyConv NUMERIC(10, 2) NULL
	,OrdQty NUMERIC(10, 2) NULL
	,PurcUom VARCHAR(50) NULL
	,PurcUomConv NUMERIC(10, 2) NULL
	,TaxCde VARCHAR(50) NULL
	,TaxRate NUMERIC(10, 2) NULL
	,UnitPrc NUMERIC(10, 2) NULL
	,LineTotExcl NUMERIC(10, 2) NULL
	,LineTotTax NUMERIC(10, 2) NULL
	,LineTotVal NUMERIC(10, 2) NULL
	)
GO

CREATE TABLE [dbo].[MessageException] (
	[ID] [int] IDENTITY(1, 1) NOT FOR REPLICATION NOT NULL
	,[ExceptionDate] [datetime] NULL
	,[Exception] [varchar](50) NULL
	,[Method] [varchar](50) NULL
	) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SupplierVendors] (
	[Id] UNIQUEIDENTIFIER PRIMARY KEY NOT NULL
	,[SupplierId] UNIQUEIDENTIFIER NOT NULL
	,[EanCode] [nvarchar](13) NOT NULL
	,
	--[SystemAdded] [bit] NULL,
	[VendorName] [varchar](50) NULL
	)
GO

ALTER TABLE [dbo].[SupplierVendors]
	WITH CHECK ADD CONSTRAINT [FK_SupplierVendors_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers]([Id])
GO

ALTER TABLE [dbo].[SupplierVendors] CHECK CONSTRAINT [FK_SupplierVendors_Suppliers]
GO

CREATE PROCEDURE SP_CREATE_ORDER
	--DECLARE
	@OrdNo VARCHAR(50) NULL
	,@OrdDate DATETIME NULL
	,@OrdDesc VARCHAR(50) NULL
	,@OrdType VARCHAR(50) NULL
	,@OrdTerm VARCHAR(50) NULL
	,@OrdTermDesc VARCHAR(50) NULL
	,@OrdStat INT NULL
	,@OrderStatus VARCHAR(50) NULL
	,@Origin VARCHAR(50) NULL
	,@PromDate DATETIME NULL
	,@CompName VARCHAR(50) NULL
	,@BranchNo VARCHAR(50) NULL
	,@BranchName VARCHAR(50) NULL
	,@BranchAddr1 VARCHAR(50) NULL
	,@BranchAddr2 VARCHAR(50) NULL
	,@BranchTel VARCHAR(50) NULL
	,@BranchFax VARCHAR(50) NULL
	,@BranchEmail VARCHAR(50) NULL
	,@BranchVat VARCHAR(50) NULL
	,@VendorRef VARCHAR(50) NULL
	,@VendorNo VARCHAR(50) NULL
	,@VendorName VARCHAR(50) NULL
	,@VendorAddr1 VARCHAR(50) NULL
	,@VendorAddr2 VARCHAR(50) NULL
	,@VendorSuburb VARCHAR(50) NULL
	,@VendorCity VARCHAR(50) NULL
	,@VendorContact VARCHAR(50) NULL
	,@TotLines INT NULL
	,@TotQty NUMERIC(10, 2) NULL
	,@TotExcl NUMERIC(10, 2) NULL
	,@TotTax NUMERIC(10, 2) NULL
	,@TotVal NUMERIC(10, 2) NULL
	,@DelivAddr1 VARCHAR(50) NULL
	,@DelivAddr2 VARCHAR(50) NULL
	,@DelivSuburb VARCHAR(50) NULL
	,@DelivCity VARCHAR(50) NULL
	,@ConfirmInd BIT NULL
	,@CompID VARCHAR(50) NULL
	,@ResendOrder BIT NULL
	,@Processed BIT NULL
AS
DECLARE @OrderCount INT = 0
DECLARE @ID UNIQUEIDENTIFIER = NEWID()

BEGIN
	SELECT @OrderCount = COUNT(*)
	FROM Orders
	WHERE OrdNo = @OrdNo

	IF @OrderCount = 0
	BEGIN
		INSERT INTO ORDERS (
			[Id]
			,[OrdNo]
			,[OrdDate]
			,[OrdDesc]
			,[OrdType]
			,[OrdTerm]
			,[OrdTermDesc]
			,[OrdStat]
			,[OrderStatus]
			,[Origin]
			,[PromDate]
			,[CompName]
			,[BranchNo]
			,[BranchName]
			,[BranchAddr1]
			,[BranchAddr2]
			,[BranchTel]
			,[BranchFax]
			,[BranchEmail]
			,[BranchVat]
			,[VendorRef]
			,[VendorNo]
			,[VendorName]
			,[VendorAddr1]
			,[VendorAddr2]
			,[VendorSuburb]
			,[VendorCity]
			,[VendorContact]
			,[TotLines]
			,[TotQty]
			,[TotExcl]
			,[TotTax]
			,[TotVal]
			,[DelivAddr1]
			,[DelivAddr2]
			,[DelivSuburb]
			,[DelivCity]
			,[ConfirmInd]
			,[CompID]
			,[ResendOrder]
			,[Processed]
			)
		VALUES (
			@ID
			,@OrdNo
			,@OrdDate
			,@OrdDesc
			,@OrdType
			,@OrdTerm
			,@OrdTermDesc
			,@OrdStat
			,@OrderStatus
			,@Origin
			,@PromDate
			,@CompName
			,@BranchNo
			,@BranchName
			,@BranchAddr1
			,@BranchAddr2
			,@BranchTel
			,@BranchFax
			,@BranchEmail
			,@BranchVat
			,@VendorRef
			,@VendorNo
			,@VendorName
			,@VendorAddr1
			,@VendorAddr2
			,@VendorSuburb
			,@VendorCity
			,@VendorContact
			,@TotLines
			,@TotQty
			,@TotExcl
			,@TotTax
			,@TotVal
			,@DelivAddr1
			,@DelivAddr2
			,@DelivSuburb
			,@DelivCity
			,@ConfirmInd
			,@CompID
			,@ResendOrder
			,@Processed
			)
	END
END
GO

CREATE PROCEDURE [dbo].[SP_CREATE_ORDER_LINES]
	--DECLARE
	@OrdLn INT NULL
	,@OrderNo VARCHAR(50)
	,@ItemNo VARCHAR(50)
	,@ItemDesc VARCHAR(50)
	,@MfrItem VARCHAR(50)
	,@QtyConv VARCHAR(50)
	,@OrdQty VARCHAR(50)
	,@PurcUom VARCHAR(50)
	,@PurcUomConv VARCHAR(50)
	,@TaxCde VARCHAR(50)
	,@TaxRate VARCHAR(50)
	,@UnitPrc VARCHAR(50)
	,@LineTotExcl VARCHAR(50)
	,@LineTotTax VARCHAR(50)
	,@LineTotVal VARCHAR(50)
AS
DECLARE @OrderLineCount INT = 0
DECLARE @ID UNIQUEIDENTIFIER = NEWID()

BEGIN
	SELECT @OrderLineCount = COUNT(*)
	FROM OrderLines
	WHERE OrderNo + CONVERT(VARCHAR(10), OrdLn) = @OrderNo + CONVERT(VARCHAR(10), @OrdLn)

	IF @OrderLineCount = 0
	BEGIN
		INSERT INTO OrderLines (
			[Id]
			,[OrdLn]
			,[OrderNo]
			,[ItemNo]
			,[ItemDesc]
			,[MfrItem]
			,[QtyConv]
			,[OrdQty]
			,[PurcUom]
			,[PurcUomConv]
			,[TaxCde]
			,[TaxRate]
			,[UnitPrc]
			,[LineTotExcl]
			,[LineTotTax]
			,[LineTotVal]
			)
		VALUES (
			@ID
			,@OrdLn
			,@OrderNo
			,@ItemNo
			,@ItemDesc
			,@MfrItem
			,@QtyConv
			,@OrdQty
			,@PurcUom
			,@PurcUomConv
			,@TaxCde
			,@TaxRate
			,@UnitPrc
			,@LineTotExcl
			,@LineTotTax
			,@LineTotVal
			)
	END
END
GO


