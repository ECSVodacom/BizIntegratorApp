USE BizIntegrator
GO

ALTER TABLE ORDERS ADD [API_Id] varchar(200)
GO

ALTER TABLE CustomerList ADD [API_Id] varchar(200)
GO

ALTER TABLE StockList ADD [API_Id] varchar(200)
GO

ALTER TABLE Invoices ADD [Processed] [bit] NULL
GO

ALTER TABLE Invoices ADD [API_Id] varchar(200)
GO