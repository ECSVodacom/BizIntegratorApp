using BizIntegrator.Data;
using BizIntegrator.Models;
using BizIntegrator.PostToBiz;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Data.Entity.Infrastructure.Design;
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
        string Method { get; set; }

        DataTable dtApiData;

        public void ProcessOrders()
        {
            DataHandler dataHandler = new DataHandler();
            try
            {
                TransactionType = "GetOrders";
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

                string Response = CreateOrders(_Id, _apiKey, _name, _url, _privateKey, _username, _Password, _authenticationType, _useAPIKey);
                
                return "";
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

                    dataArray = jObject;

                    string ordNo = string.Empty;

                    foreach (var obj in dataArray)
                    {
                        o.OrdNo = obj["ordNo"].ToString();
                        ordNo = o.OrdNo;
                        o.OrdDate = obj["ordDate"].ToString();
                        o.OrdDesc = obj["ordDesc"].ToString();
                        o.OrdType = obj["ordType"].ToString();
                        o.OrdTerm = obj["ordTerm"].ToString();
                        o.OrdTermDesc = obj["ordTermDesc"].ToString();
                        o.OrdStat = obj["ordStat"].ToString();
                        o.OrderStatus = obj["orderStatus"].ToString();
                        o.Origin = obj["origin"].ToString();
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

                        DataTable dtEancodes = dataHandler.GetEanCodes(obj["vendorNo"].ToString());

                        if (dtEancodes.Rows.Count > 0)
                        {
                            senderEanCode = dtEancodes.Rows[0]["SenderGln"].ToString();
                            recieverEanCode = dtEancodes.Rows[0]["RecieverGln"].ToString();
                        }

                        else
                        {
                            senderEanCode = "6004930994136";
                            recieverEanCode = "0000000000000";
                        }

                        dataHandler.CreateOrders(o.OrdNo, o.OrdDate, o.OrdDesc, o.OrdType, o.OrdTerm
                                                , o.OrdTermDesc, o.OrdStat, o.OrderStatus, o.Origin, o.PromDate
                                                , o.CompName, o.BranchNo, o.BranchName, o.BranchAddr1, o.BranchAddr2
                                                , o.BranchTel, o.BranchFax, o.BranchEmail, o.BranchVat, o.VendorRef
                                                , o.VendorNo, o.VendorName, o.VendorAddr1, o.VendorAddr2, o.VendorSuburb
                                                , o.VendorCity, o.VendorContact, o.TotLines, o.TotQty
                                                , o.TotExcl, o.TotTax, o.TotVal, o.DelivAddr1
                                                , o.DelivAddr2, o.DelivSuburb, o.DelivCity, o.BuyerNote, (bool)o.ConfirmInd
                                                , o.CompID, (bool)o.ResendOrder, (bool)o.Processed, _id);

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

                        DateTime date = DateTime.Now;
                        string formattedDate = date.ToString("yyyyMMdd");

                        fileName = "PP" + "-ORDER-" + ordNo + "-" + formattedDate + ".json";

                        BizHandler bizHandler = new BizHandler();

                        bool orderProcessed = dataHandler.CheckOrders(ordNo);

                       if (!orderProcessed)
                        {
                            bizHandler.PostToBiz(obj.ToString(), fileName, senderEanCode, recieverEanCode);
                            dataHandler.UpdateProcessedOrder(ordNo);

                            obj["confirmInd"] = true;

                            ConfirmOrders(ordNo, obj.ToString(), _id, _name, _url, _apiKey, _privateKey, _username, _Password, _authenticationType, _useAPIKey);
                        }
                    }


                    return response.RequestMessage.ToString();
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

        private void ConfirmOrders(string ordNo, string ordContent, string id, string name, string url, string apiKey, string privateKey, string username, string password, string authenticationType, string useAPIKey)
        {
            RestHandler restHandler = new RestHandler();
            DataHandler dataHandler = new DataHandler();
            HttpClient client;

            string transactionType = "GetOrders";

            try
            {
                client = restHandler.SetClient(id, name, url, apiKey, privateKey, username, password, authenticationType, useAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                DataTable dtPostDataUrl = dataHandler.GetApiData(transactionType);
                DataRow row = dtPostDataUrl.Rows[0];
                string postDataUrl = row["EndPoint"].ToString() + "/"+ordNo;

                var content = new StringContent(ordContent, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(postDataUrl, content).Result;
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "ConfirmOrders");
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
    }

}
