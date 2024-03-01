USE [BizIntegrator]
GO

Declare @Id UniqueIdentifier

Set @Id = NewID()

INSERT INTO [APIEndpoints]([Id],[API_Id],[EndPoint],[TransactionType],[HttpVerb])

SELECT @Id
      ,'CCC4471C-8994-4181-BF24-E99E2F1CD788'
      ,'https://edi.yourwoermann.com:5000/api/Order'
      ,'GetOrders'
      ,'GET'
  --FROM [BizIntegrator].[dbo].[APIEndpoints]