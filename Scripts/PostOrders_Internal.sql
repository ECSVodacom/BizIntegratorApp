USE [BizIntegrator]
GO

Declare @Id UniqueIdentifier

Set @Id = NewID()

INSERT INTO [Internal_Endpoints]([Id],[API_Id],[EndPoint],[TransactionType],[HttpVerb])

SELECT @Id
      ,'9C9B2B80-B076-4DF4-8482-7721E0DD5A22'
      ,'https://192.168.100.14/BizIntegratorApp/api/Order'
      ,'PostOrders'
      ,'GET'
  --FROM [BizIntegrator].[dbo].[APIEndpoints]