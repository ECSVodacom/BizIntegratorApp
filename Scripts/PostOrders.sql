USE [BizIntegrator]
GO

Declare @Id UniqueIdentifier

Set @Id = NewID()

INSERT INTO [APIEndpoints]([Id],[API_Id],[EndPoint],[TransactionType],[HttpVerb])

SELECT @Id
      ,'9C9B2B80-B076-4DF4-8482-7721E0DD5A22'
      ,'https://api.liquorgistics.co.za:10200/api/Diageo_Vodacom/sales-orders/add'
      ,'PostOrders'
      ,'GET'
  --FROM [BizIntegrator].[dbo].[APIEndpoints]