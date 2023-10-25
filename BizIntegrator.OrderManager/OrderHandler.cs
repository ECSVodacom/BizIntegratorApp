using BizIntegrator.Data;
using BizIntegrator.Models;
using BizIntegrator.PostToBiz;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BizIntegrator.OrderManager
{
    public class OrderHandler
    {
        private string fileName { get; set; }
        private string senderEanCode { get; set; }
        private string recieverEanCode { get; set; }

        string Id { get; set; }
        string ApiKey { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string PrivateKey { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string AuthenticationType { get; set; }
        string UseAPIKey { get; set; }
        string TransactionType { get; set; }

        DataTable dtApiData;

        public void ProcessOrders()
        {
            DataHandler dataHandler = new DataHandler();
            try
            {


                TransactionType = "Orders";

                dtApiData = dataHandler.GetApiData(TransactionType);

                foreach (DataRow row in dtApiData.Rows)
                {
                    Id = row["Id"].ToString();
                    ApiKey = row["AccountKey"].ToString();
                    Name = row["Name"].ToString();
                    Url = row["EndPoint"].ToString();
                    PrivateKey = row["PrivateKey"].ToString();
                    Username = row["Username"].ToString();
                    Password = row["Password"].ToString();
                    AuthenticationType = row["AuthenticationType"].ToString();
                    UseAPIKey = row["UseAPIKey"].ToString();

                    ConvertOrderToXtraEdi(Id, ApiKey, Name, Url, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);


                }
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "ProcessOrders");
            }

        }

        public string ConvertOrderToXtraEdi(string _Id, string _apiKey, string _name, string _url, string _privateKey, string _username, string _Password, string _authenticationType, string _useAPIKey)
        {
            DataHandler dataHandler = new DataHandler();
            Orders ord = new Orders();
            try
            {
                string outputXtraEdit = string.Empty;

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = true
                };


                DataTable dtOrders = dataHandler.GetOrders();

                foreach (DataRow row in dtOrders.Rows)
                {
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                        {
                            XmlDocument outputDocument = new XmlDocument();

                            XmlNode root = outputDocument.CreateElement("orderMessage");

                            XmlNode standardBusinessDocumentHeader;
                            XmlNode headerVersion;
                            XmlNode documentType;
                            XmlNode documentStatusCode;
                            XmlNode documentIdentification;
                            XmlNode standard;
                            XmlNode typeVersion;
                            XmlNode creationDateAndTime;
                            XmlNode receiver;
                            XmlNode sender;
                            XmlNode receiverGln;
                            XmlNode senderGln;

                            ord.OrdNo = row["ordNo"].ToString();
                            ord.OrdDate = row["ordDate"].ToString();
                            ord.OrdDesc = row["ordDesc"].ToString();
                            ord.OrdType = row["ordType"].ToString();
                            ord.OrdTerm = row["ordTerm"].ToString();
                            ord.OrdTermDesc = row["ordTermDesc"].ToString();
                            ord.OrdStat = row["ordStat"].ToString();
                            ord.OrderStatus = row["orderStatus"].ToString();
                            ord.Origin = row["origin"].ToString();
                            ord.PromDate = row["promDate"].ToString();
                            ord.CompName = row["compName"].ToString();
                            ord.BranchNo = row["branchNo"].ToString();
                            ord.BranchName = row["branchName"].ToString();
                            ord.BranchAddr1 = row["branchAddr1"].ToString();
                            ord.BranchAddr2 = row["branchAddr2"].ToString();
                            ord.BranchTel = row["branchTel"].ToString();
                            ord.BranchFax = row["branchFax"].ToString();
                            ord.BranchEmail = row["branchEmail"].ToString();
                            ord.BranchVat = row["branchVat"].ToString();
                            ord.VendorRef = row["vendorRef"].ToString();
                            ord.VendorName = row["vendorName"].ToString();
                            ord.VendorAddr1 = row["vendorAddr1"].ToString();
                            ord.VendorAddr2 = row["vendorAddr2"].ToString();
                            ord.VendorSuburb = row["vendorSuburb"].ToString();
                            ord.VendorCity = row["vendorCity"].ToString();
                            ord.VendorContact = row["vendorContact"].ToString();
                            ord.TotLines = row["totLines"].ToString();
                            ord.TotQty = row["totQty"].ToString();
                            ord.TotExcl = row["totExcl"].ToString();
                            ord.TotTax = row["totTax"].ToString();
                            ord.TotVal = row["totVal"].ToString();
                            ord.DelivAddr1 = row["delivAddr1"].ToString();
                            ord.DelivAddr2 = row["delivAddr2"].ToString();
                            ord.DelivSuburb = row["delivSuburb"].ToString();
                            ord.DelivCity = row["delivCity"].ToString();
                            ord.CompID = row["compID"].ToString();
                            ord.VendorNo = row["vendorNo"].ToString();

                            DataTable dtEancodes = dataHandler.GetEanCodes(ord.VendorNo);

                            foreach (DataRow dr in dtEancodes.Rows)
                            {
                                senderEanCode = dr["SenderGln"].ToString();
                                recieverEanCode = dr["RecieverGln"].ToString();
                            }

                            standardBusinessDocumentHeader = outputDocument.CreateElement("standardBusinessDocumentHeader");
                            headerVersion = outputDocument.CreateElement("headerVersion");
                            headerVersion.InnerText = "1.0";
                            documentType = outputDocument.CreateElement("documentType");
                            documentType.InnerText = "ORDER";
                            documentStatusCode = outputDocument.CreateElement("documentStatusCode");
                            documentStatusCode.InnerText = "ORIGINAL";
                            documentIdentification = outputDocument.CreateElement("documentIdentification");
                            standard = outputDocument.CreateElement("standard");
                            standard.InnerText = "XTRA";
                            typeVersion = outputDocument.CreateElement("typeVersion");
                            typeVersion.InnerText = "1.0";
                            creationDateAndTime = outputDocument.CreateElement("creationDateAndTime");
                            creationDateAndTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
                            receiver = outputDocument.CreateElement("receiver");
                            receiverGln = outputDocument.CreateElement("gln");
                            receiverGln.InnerText = recieverEanCode;
                            sender = outputDocument.CreateElement("sender");
                            senderGln = outputDocument.CreateElement("gln");
                            senderGln.InnerText = senderEanCode;
                            root.AppendChild(standardBusinessDocumentHeader);
                            standardBusinessDocumentHeader.AppendChild(headerVersion);
                            standardBusinessDocumentHeader.AppendChild(documentType);
                            standardBusinessDocumentHeader.AppendChild(documentStatusCode);
                            standardBusinessDocumentHeader.AppendChild(documentIdentification);
                            documentIdentification.AppendChild(standard);
                            documentIdentification.AppendChild(typeVersion);
                            documentIdentification.AppendChild(creationDateAndTime);
                            standardBusinessDocumentHeader.AppendChild(receiver);
                            receiver.AppendChild(receiverGln);
                            standardBusinessDocumentHeader.AppendChild(sender);
                            sender.AppendChild(senderGln);

                            XmlNode newOrder = outputDocument.CreateElement("order");
                            XmlNode newCustomer = outputDocument.CreateElement("customer");
                            XmlNode customerGln = outputDocument.CreateElement("gln");
                            customerGln.InnerText = senderEanCode;
                            XmlNode contactInformation = outputDocument.CreateElement("contactInformation");
                            contactInformation.InnerText = ord.BranchTel;
                            XmlNode customerName = outputDocument.CreateElement("customerName");
                            customerName.InnerText = ord.BranchName;
                            XmlNode newDeliveryPoint = outputDocument.CreateElement("deliveryPoint");
                            XmlNode gln = outputDocument.CreateElement("gln");
                            gln.InnerText = ord.BranchNo;
                            XmlNode deliveryPointName = outputDocument.CreateElement("deliveryPointName");
                            deliveryPointName.InnerText = ord.BranchName;
                            XmlNode newOrderIdentification = outputDocument.CreateElement("orderIdentification");
                            XmlNode orderNo = outputDocument.CreateElement("orderNo");
                            orderNo.InnerText = ord.OrdNo;
                            XmlNode orderDate = outputDocument.CreateElement("orderDate");
                            orderDate.InnerText = ord.OrdDate;
                            XmlNode deliveryDates = outputDocument.CreateElement("deliveryDates");
                            XmlNode dueDate = outputDocument.CreateElement("dueDate");
                            dueDate.InnerText = ord.PromDate;
                            XmlNode earliestDate = outputDocument.CreateElement("earliestDate");
                            earliestDate.InnerText = ord.PromDate;
                            XmlNode latestDate = outputDocument.CreateElement("latestDate");
                            latestDate.InnerText = ord.PromDate;
                            XmlNode orderCurrencyCode = outputDocument.CreateElement("orderCurrencyCode");
                            orderCurrencyCode.InnerText = "ZAR";
                            XmlNode countryOfSupplyOfGoods = outputDocument.CreateElement("countryOfSupplyOfGoods");
                            countryOfSupplyOfGoods.InnerText = "ZA";
                            XmlNode representatives = outputDocument.CreateElement("representatives");
                            representatives.InnerText = ord.VendorNo;
                            XmlNode orderTotals = outputDocument.CreateElement("orderTotals");
                            orderTotals.InnerText = "";
                            XmlNode totalOrderAmountExclusive = outputDocument.CreateElement("totalOrderAmountExclusive");
                            totalOrderAmountExclusive.InnerText = Convert.ToDecimal(ord.TotExcl).ToString("0.00", CultureInfo.InvariantCulture);
                            XmlNode totalOrderAmountInclusive = outputDocument.CreateElement("totalOrderAmountInclusive");
                            totalOrderAmountInclusive.InnerText = Convert.ToDecimal(ord.TotVal).ToString("0.00", CultureInfo.InvariantCulture);

                            outputDocument.AppendChild(root);
                            root.AppendChild(newOrder);
                            newOrder.AppendChild(newCustomer);
                            newOrder.AppendChild(newOrderIdentification);
                            orderTotals.AppendChild(totalOrderAmountExclusive);
                            orderTotals.AppendChild(totalOrderAmountInclusive);
                            newCustomer.AppendChild(customerGln);
                            newCustomer.AppendChild(contactInformation);
                            newCustomer.AppendChild(customerName);
                            newCustomer.AppendChild(newDeliveryPoint);
                            newDeliveryPoint.AppendChild(gln);
                            newDeliveryPoint.AppendChild(deliveryPointName);
                            newOrder.AppendChild(newOrderIdentification);
                            newOrderIdentification.AppendChild(orderNo);
                            newOrderIdentification.AppendChild(orderDate);
                            newOrderIdentification.AppendChild(deliveryDates);
                            deliveryDates.AppendChild(dueDate);
                            deliveryDates.AppendChild(earliestDate);
                            deliveryDates.AppendChild(latestDate);
                            newOrderIdentification.AppendChild(orderCurrencyCode);
                            newOrderIdentification.AppendChild(countryOfSupplyOfGoods);
                            newOrderIdentification.AppendChild(representatives);

                            DataTable dtOrderLines = dataHandler.GetOrdersLines(ord.OrdNo);

                            foreach (DataRow rowOln in dtOrderLines.Rows)
                            {
                                string ordLn = rowOln["ordLn"].ToString();
                                string itemNo = rowOln["itemNo"].ToString();
                                string itemDesc = rowOln["itemDesc"].ToString();
                                string mfrItem = rowOln["mfrItem"].ToString();
                                string ordQty = rowOln["ordQty"].ToString();
                                string purcUom = rowOln["purcUom"].ToString();
                                string unitPrc = rowOln["unitPrc"].ToString();

                                decimal amountExclusiveCalc = Convert.ToDecimal(ordQty) * decimal.Parse(unitPrc);

                                XmlNode orderLineItem = outputDocument.CreateElement("orderLineItem");
                                XmlNode lineItemNumber = outputDocument.CreateElement("lineItemNumber");
                                lineItemNumber.InnerText = ordLn;
                                XmlNode transactionalTradeItem = outputDocument.CreateElement("transactionalTradeItem");
                                transactionalTradeItem.InnerText = "";
                                XmlNode customerIdentifier = outputDocument.CreateElement("customerIdentifier");
                                customerIdentifier.InnerText = "";
                                XmlNode gtin = outputDocument.CreateElement("gtin");
                                gtin.InnerText = itemNo;
                                XmlNode code = outputDocument.CreateElement("code");
                                code.InnerText = mfrItem;
                                XmlNode description = outputDocument.CreateElement("description");
                                description.InnerText = itemDesc;
                                XmlNode supplierIdentifier = outputDocument.CreateElement("supplierIdentifier");
                                supplierIdentifier.InnerText = "";
                                XmlNode supplierGtin = outputDocument.CreateElement("gtin");
                                supplierGtin.InnerText = itemNo;
                                XmlNode orderedQuantity = outputDocument.CreateElement("orderedQuantity");
                                orderedQuantity.InnerText = Convert.ToDecimal(ordQty).ToString("0.00", CultureInfo.InvariantCulture);
                                XmlNode unitOfMeasure = outputDocument.CreateElement("UnitOfMeasure");
                                unitOfMeasure.InnerText = purcUom;
                                XmlNode itemPriceExclusive = outputDocument.CreateElement("itemPriceExclusive");
                                itemPriceExclusive.InnerText = Convert.ToDecimal(unitPrc).ToString("0.00", CultureInfo.InvariantCulture);
                                XmlNode amountExclusiveBeforeDiscount = outputDocument.CreateElement("amountExclusiveBeforeDiscount");
                                amountExclusiveBeforeDiscount.InnerText = amountExclusiveCalc.ToString("0.00", CultureInfo.InvariantCulture);
                                XmlNode discount = outputDocument.CreateElement("discount");
                                discount.InnerText = "";
                                XmlNode amountExclusiveAfterDiscount = outputDocument.CreateElement("amountExclusiveAfterDiscount");
                                amountExclusiveAfterDiscount.InnerText = amountExclusiveCalc.ToString("0.00", CultureInfo.InvariantCulture);
                                XmlNode orderLineTaxInformation = outputDocument.CreateElement("orderLineTaxInformation");
                                representatives.InnerText = ord.VendorNo;

                                newOrder.AppendChild(orderLineItem);
                                transactionalTradeItem.AppendChild(customerIdentifier);
                                customerIdentifier.AppendChild(gtin);
                                customerIdentifier.AppendChild(code);
                                customerIdentifier.AppendChild(description);
                                transactionalTradeItem.AppendChild(supplierIdentifier);
                                supplierIdentifier.AppendChild(supplierGtin);
                                orderLineItem.AppendChild(lineItemNumber);
                                orderLineItem.AppendChild(transactionalTradeItem);
                                orderLineItem.AppendChild(unitOfMeasure);
                                orderLineItem.AppendChild(orderedQuantity);
                                orderLineItem.AppendChild(itemPriceExclusive);
                                orderLineItem.AppendChild(amountExclusiveBeforeDiscount);
                                orderLineItem.AppendChild(discount);
                                orderLineItem.AppendChild(amountExclusiveAfterDiscount);
                                orderLineItem.AppendChild(orderLineTaxInformation);
                            }

                            newOrder.AppendChild(orderTotals);

                            DateTime date = DateTime.Now;
                            string formattedDate = date.ToString("yyyyMMdd");

                            fileName = "PP" + "-ORDER-" + ord.OrdNo + "-" + formattedDate;

                            outputDocument.Save(xmlWriter);
                            outputXtraEdit = stringWriter.ToString();

                            bool orderProcessed = dataHandler.CheckOrders(ord.OrdNo);

                            BizHandler bizHandler = new BizHandler();

                            if (!orderProcessed)
                            {
                                bizHandler.PostToBiz(outputXtraEdit, fileName + ".xml");
                                dataHandler.UpdateProcessedOrder(ord.OrdNo);


                                //Method to send invoice back to the Woerman API
                                ProcessInvoice();
                            }
                        }

                    }

                }

                return outputXtraEdit;
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "ConvertOrderToXtraEdi");
                return "Orders not downloaded";
            }

        }
        public string CreateOrders(string _id, string _apiKey, string _name, string _url, string _privateKey, string _username, string _Password, string _authenticationType, string _useAPIKey)
        {
            DataHandler dataHandler = new DataHandler();
            RestHandler restHandler = new RestHandler();
            Orders o = new Orders();
            OrderLines ol = new OrderLines();
            try
            {
                HttpClient client;

                client = restHandler.SetClient(_id, _name, _url, _apiKey, _privateKey, _username, _Password, _authenticationType, _useAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("").Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync();

                    dynamic jObject = JsonConvert.DeserializeObject(dataObjects.Result);

                    JArray dataArray;

                    if (_name == "DANNIC")
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(dataObjects.Result);

                        dataArray = jsonObject.data;

                        foreach (JObject obj in dataArray)
                        {
                            o.OrdNo = obj["orderNumber"].ToString();
                            o.OrdDate = obj["invoiceDate"].ToString();
                            o.OrdDesc = "";
                            o.OrdType = "";
                            o.OrdTerm = "";
                            o.OrdTermDesc = "";
                            o.OrdStat = obj["documentState"].ToString();
                            o.OrderStatus = "";
                            o.Origin = "";
                            o.PromDate = obj["invoiceDate"].ToString();
                            o.CompName = "";
                            o.BranchNo = "";
                            o.BranchName = "";
                            o.BranchAddr1 = "";
                            o.BranchAddr2 = "";
                            o.BranchTel = "";
                            o.BranchFax = "";
                            o.BranchEmail = "";
                            o.BranchVat = "";
                            o.VendorRef = "";
                            o.VendorNo = obj["customerCode"].ToString();
                            o.VendorName = "";
                            o.VendorAddr1 = "";
                            o.VendorAddr2 = "";
                            o.VendorSuburb = "";
                            o.VendorCity = "";
                            o.VendorContact = "";
                            o.TotLines = "0";
                            o.TotQty = "0";
                            o.TotExcl = "0";
                            o.TotTax = "0";
                            o.TotVal = "0";
                            o.DelivAddr1 = "";
                            o.DelivAddr2 = "";
                            o.DelivSuburb = "";
                            o.DelivCity = "";
                            o.BuyerNote = "";
                            o.ConfirmInd = false;
                            o.CompID = "";
                            o.ResendOrder = false;
                            o.Processed = false;

                            dataHandler.CreateOrders(o.OrdNo, o.OrdDate, o.OrdDesc, o.OrdType, o.OrdTerm
                                                    , o.OrdTermDesc, o.OrdStat, o.OrderStatus, o.Origin, o.PromDate
                                                    , o.CompName, o.BranchNo, o.BranchName, o.BranchAddr1, o.BranchAddr2
                                                    , o.BranchTel, o.BranchFax, o.BranchEmail, o.BranchVat, o.VendorRef
                                                    , o.VendorNo, o.VendorName, o.VendorAddr1, o.VendorAddr2, o.VendorSuburb
                                                    , o.VendorCity, o.VendorContact, o.TotLines, o.TotQty
                                                    , o.TotExcl, o.TotTax, o.TotVal, o.DelivAddr1
                                                    , o.DelivAddr2, o.DelivSuburb, o.DelivCity, o.BuyerNote, (bool)o.ConfirmInd
                                                    , o.CompID, (bool)o.ResendOrder, (bool)o.Processed);

                            JArray dataArrayLines = (JArray)obj["Lines"];

                            int ordLn = 0;
                            foreach (JObject objLines in dataArrayLines)
                            {
                                ordLn++;

                                if (obj["invoiceId"].ToString() == objLines["invoiceId"].ToString())
                                {
                                    ol.OrdLn = ordLn.ToString();
                                    ol.OrdNo = obj["orderNumber"].ToString();
                                    ol.ItemNo = objLines["itemCode"].ToString();
                                    ol.ItemDesc = objLines["lineDescription"].ToString();
                                    ol.MfrItem = "";
                                    ol.QtyConv = "0";
                                    ol.OrdQty = objLines["quantity"].ToString();
                                    ol.PurcUom = objLines["unitOfMeasure"].ToString();
                                    ol.PurcUomConv = "0";
                                    ol.TaxCde = "0";
                                    ol.TaxRate = "0";
                                    ol.UnitPrc = objLines["unitPriceExcl"].ToString();
                                    ol.LineTotExcl = objLines["lineNetTotalOrderedExVat"].ToString();
                                    ol.LineTotTax = "0";
                                    ol.LineTotVal = objLines["lineNetTotalOrderedInVat"].ToString();
                                }

                                dataHandler.CreateOrderLines(ol.OrdLn, ol.OrdNo, ol.ItemNo, ol.ItemDesc, ol.MfrItem
                                                            , ol.QtyConv, ol.OrdQty, ol.PurcUom, ol.PurcUomConv, ol.TaxCde
                                                            , ol.TaxRate, ol.UnitPrc, ol.LineTotExcl, ol.LineTotTax, ol.LineTotVal);
                            }
                        }
                    }

                    else if (_name == "PLASTIC")
                    {
                        dataArray = jObject;

                        foreach (var obj in dataArray)
                        {
                            o.OrdNo = obj["ordNo"].ToString();
                            o.OrdDate = obj["ordDate"].ToString();
                            o.OrdDesc = obj["ordDesc"].ToString();
                            o.OrdType = obj["ordType"].ToString();
                            o.OrdTerm = obj["ordTerm"].ToString();
                            o.OrdTermDesc = obj["ordTermDesc"].ToString();
                            o.OrdStat = obj["ordStat"].ToString();
                            o.OrderStatus = obj["orderStatus"].ToString();
                            o.Origin = obj["origin"].ToString();
                            o.PromDate = obj["promDate"].ToString();
                            o.CompName = obj["compName"].ToString();
                            o.BranchNo = obj["branchNo"].ToString();
                            o.BranchName = obj["branchName"].ToString();
                            o.BranchAddr1 = obj["branchAddr1"].ToString();
                            o.BranchAddr2 = obj["branchAddr2"].ToString();
                            o.BranchTel = obj["branchTel"].ToString();
                            o.BranchFax = obj["branchFax"].ToString();
                            o.BranchEmail = obj["branchEmail"].ToString();
                            o.BranchVat = obj["branchVat"].ToString();
                            o.VendorRef = obj["vendorRef"].ToString();
                            o.VendorNo = obj["vendorNo"].ToString();
                            o.VendorName = obj["vendorName"].ToString();
                            o.VendorAddr1 = obj["vendorAddr1"].ToString();
                            o.VendorAddr2 = obj["vendorAddr2"].ToString();
                            o.VendorSuburb = obj["vendorSuburb"].ToString();
                            o.VendorCity = obj["vendorCity"].ToString();
                            o.VendorContact = obj["vendorContact"].ToString();
                            o.TotLines = obj["totLines"].ToString();
                            o.TotQty = obj["totQty"].ToString();
                            o.TotExcl = obj["totExcl"].ToString();
                            o.TotTax = obj["totTax"].ToString();
                            o.TotVal = obj["totVal"].ToString();
                            o.DelivAddr1 = obj["delivAddr1"].ToString();
                            o.DelivAddr2 = obj["delivAddr2"].ToString();
                            o.DelivSuburb = obj["delivSuburb"].ToString();
                            o.DelivCity = obj["delivCity"].ToString();
                            o.BuyerNote = "";
                            o.ConfirmInd = false;
                            o.CompID = obj["compID"].ToString();
                            o.ResendOrder = false;
                            o.Processed = false;

                            dataHandler.CreateOrders(o.OrdNo, o.OrdDate, o.OrdDesc, o.OrdType, o.OrdTerm
                                                    , o.OrdTermDesc, o.OrdStat, o.OrderStatus, o.Origin, o.PromDate
                                                    , o.CompName, o.BranchNo, o.BranchName, o.BranchAddr1, o.BranchAddr2
                                                    , o.BranchTel, o.BranchFax, o.BranchEmail, o.BranchVat, o.VendorRef
                                                    , o.VendorNo, o.VendorName, o.VendorAddr1, o.VendorAddr2, o.VendorSuburb
                                                    , o.VendorCity, o.VendorContact, o.TotLines, o.TotQty
                                                    , o.TotExcl, o.TotTax, o.TotVal, o.DelivAddr1
                                                    , o.DelivAddr2, o.DelivSuburb, o.DelivCity, o.BuyerNote, (bool)o.ConfirmInd
                                                    , o.CompID, (bool)o.ResendOrder, (bool)o.Processed);

                            string ordLineItem = obj["ordNo"].ToString();

                            JArray dataArrayLines = (JArray)obj["orderLines"];

                            int itemCount = 0;
                            foreach (JObject objLines in dataArrayLines)
                            {
                                itemCount++;

                                if (ordLineItem == obj["ordNo"].ToString() && itemCount == Convert.ToInt32(objLines["ordLn"]))
                                {
                                    ol.OrdLn = objLines["ordLn"].ToString();
                                    ol.OrdNo = obj["ordNo"].ToString();
                                    ol.ItemNo = objLines["itemNo"].ToString();
                                    ol.ItemDesc = objLines["itemDesc"].ToString();
                                    ol.MfrItem = objLines["mfrItem"].ToString();
                                    ol.QtyConv = objLines["qtyConv"].ToString();
                                    ol.OrdQty = objLines["ordQty"].ToString();
                                    ol.PurcUom = objLines["purcUom"].ToString();
                                    ol.PurcUomConv = objLines["purcUomConv"].ToString();
                                    ol.TaxCde = objLines["taxCde"].ToString();
                                    ol.TaxRate = objLines["taxRate"].ToString();
                                    ol.UnitPrc = objLines["unitPrc"].ToString();
                                    ol.LineTotExcl = objLines["lineTotExcl"].ToString();
                                    ol.LineTotTax = objLines["lineTotTax"].ToString();
                                    ol.LineTotVal = objLines["lineTotVal"].ToString();

                                    dataHandler.CreateOrderLines(ol.OrdLn, ol.OrdNo, ol.ItemNo, ol.ItemDesc, ol.MfrItem
                                                                , ol.QtyConv, ol.OrdQty, ol.PurcUom, ol.PurcUomConv, ol.TaxCde
                                                                , ol.TaxRate, ol.UnitPrc, ol.LineTotExcl, ol.LineTotTax, ol.LineTotVal);


                                }

                            }
                        }
                    }

                    JArray jArray = JArray.Parse(dataObjects.Result);

                    XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"order\":" + jArray + "}", "orderMessage");
                    return doc.OuterXml;
                }

                else
                {
                    return response.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "CreateOrders");
                return ex.Message;
            }

        }

        public bool IsValidXml(string xml)
        {
            DataHandler dataHandler = new DataHandler();

            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "IsValidXml");
                return false;
            }
        }

        static async Task<string> GetJsonData()
        {
            string invoiceUrl = "https://example.com/api/data"; 

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(invoiceUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        return json;
                    }
                    else
                    {
                        return "Request failed with status code: " + response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    return "An error occurred: " + ex.Message;
                }
            }
        }

        private string ProcessInvoice()
        {
            DataHandler dataHandler = new DataHandler();
            RestHandler restHandler = new RestHandler();
            HttpClient client;

            try
            {
                //Get Json Invoice file from Biz
                string jSonData = GetJsonData().ToString();

                //Pass through invoice file as body on the post request to Woerman (with key)

                client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                TransactionType = "Invoices";

                dataHandler.GetApiData(TransactionType);

                dtApiData = dataHandler.GetApiData(TransactionType);

                Url = dtApiData.Rows[0]["EndPoint"].ToString();

                var content = new StringContent(jSonData, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(Url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return content.ToString();
                }

                else
                {
                    return response.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "ProcessInvoice");
                return ex.Message;
            }
        }
    }

}
