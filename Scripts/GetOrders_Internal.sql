USE [BizIntegrator]
GO

Declare @Id UniqueIdentifier

Set @Id = NewID()

INSERT INTO [Internal_Endpoints]([Id],[API_Id],[EndPoint],[TransactionType],[HttpVerb])

SELECT @Id
      ,'CCC4471C-8994-4181-BF24-E99E2F1CD788'
      ,'https://192.168.100.14/BizIntegratorApp/api/Order'
      ,'GetOrders'
      ,'GET'
  --FROM [BizIntegrator].[dbo].[APIEndpoints]