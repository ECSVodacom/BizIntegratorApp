using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using BizIntegrator.Data;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using BizIntegrator.Service.Repository;
using Microsoft.AspNetCore.Http.Extensions;
using BizIntegrator.Models;
using System.Security.Cryptography;
using System.Xml.Linq;
using BizIntegrator.PostToBiz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace BizIntegrator.Service.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
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

        private readonly ILogger<CustomerListController> _logger;
        private readonly IConfiguration _configuration;

        public OrderController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "Order")]
        [Consumes("application/json")]
        public ActionResult Post([FromBody] JsonElement orders)
        {
            string jsonString = orders.ToString();
            string errorMessage = "Errors encountered";
            HttpClient client;
            DataHandler dataHandler = new DataHandler();

            try
            {
                TransactionType = "PostOrders";

                dtApiData = dataHandler.GetApiData(TransactionType);

                if (dtApiData.Rows.Count > 0)
                {
                    foreach (DataRow r in dtApiData.Rows)
                    {
                        Id = r["Id"].ToString();
                        ApiKey = r["AccountKey"].ToString();
                        Name = r["Name"].ToString();
                        Url = r["EndPoint"].ToString();
                        PrivateKey = r["PrivateKey"].ToString();
                        Username = r["Username"].ToString();
                        Password = r["Password"].ToString();
                        AuthenticationType = r["AuthenticationType"].ToString();
                        UseAPIKey = r["UseAPIKey"].ToString();

                        if (Name == "Diageo_Vodacom")
                        {
                            Orders o = new Orders();
                            OrderLines ol = new OrderLines();

                            RestHandler restHandler = new RestHandler();

                            client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                            client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));

                            string postDataUrl = Url;

                            dynamic jObject = JsonConvert.DeserializeObject(jsonString);

                            foreach (JObject obj in jObject)
                            {
                                //string branchCode = string.Empty;
                                //string customerCode = string.Empty;

                                o.OrdNo = obj["orderNumber"].ToString();
                                o.OrdDate = obj["orderDate"].ToString();
                                o.OrdDesc = obj["description"].ToString();
                                o.OrdType = "";
                                o.OrdTerm = "";
                                o.OrdTermDesc = "";
                                o.OrdStat = "0";
                                o.OrderStatus = "";
                                o.Origin = "";
                                //o.PromDate = obj["promDate"].ToString();
                                o.CompName = "";
                                o.BranchNo = obj["branchCode"].ToString();
                                o.BranchName = "";
                                o.BranchAddr1 = "";
                                o.BranchAddr2 = "";
                                o.BranchTel = "";
                                o.BranchFax = "";
                                o.BranchEmail = "";
                                o.BranchVat = "";
                                o.VendorRef = "";
                                o.VendorNo = "";
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
                                                        , o.CompID, (bool)o.ResendOrder, (bool)o.Processed, Id);


                                JArray dataArrayLines = (JArray)obj["lines"];

                                int itemCount = 0;

                                foreach (JObject objLines in dataArrayLines)
                                {
                                    itemCount++;
                                    ol.OrdLn = itemCount.ToString();
                                    ol.OrdNo = obj["orderNumber"].ToString();
                                    ol.ItemNo = objLines["itemCode"].ToString();
                                    ol.ItemDesc = "";
                                    ol.QtyConv = "0";
                                    ol.OrdQty = objLines["orderQuantity"].ToString();
                                    ol.PurcUom = "0";
                                    ol.PurcUomConv = "0";
                                    ol.TaxCde = "";
                                    ol.TaxRate = "0";
                                    ol.UnitPrc = objLines["unitPriceExcl"].ToString();
                                    ol.LineTotExcl = "0";
                                    ol.LineTotTax = "0";
                                    ol.LineTotVal = "0";

                                    dataHandler.CreateOrderLines(ol.OrdLn, ol.OrdNo, ol.ItemNo, ol.ItemDesc, ol.MfrItem
                                                                , ol.QtyConv, ol.OrdQty, ol.PurcUom, ol.PurcUomConv, ol.TaxCde
                                                                , ol.TaxRate, ol.UnitPrc, ol.LineTotExcl, ol.LineTotTax, ol.LineTotVal);

                                    var content = new StringContent(jObject.ToString(), System.Text.Encoding.UTF8, "application/json");

                                    HttpResponseMessage response = client.PostAsync(postDataUrl, content).Result;
                                }

                            }
                        }

                        else if (Name == "PLASTIC")
                        {
                            OrderHandler orderHandler = new OrderHandler();

                            Orders ord = new Orders();

                            try
                            {
                                string outputXtraEdit = string.Empty;

                                string Response = orderHandler.CreateOrders(Id, ApiKey, Name, Url, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                            }
                            catch (Exception ex)
                            {
                                dataHandler.WriteException(ex.InnerException.ToString(), "Get Orders");
                            }
                        }
                    }
                }

                else
                {
                    return BadRequest(new { Message = errorMessage });
                }



                return Created(Request.GetDisplayUrl(), "Orders has been successfully posted");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
            }
        }
    }

}
