using BizIntegrator.Data;
using BizIntegrator.InvoiceManager.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Xml.Linq;
using System;
using BizIntegrator.InvoiceManager.Repository;
using BizIntegrator.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace BizIntegrator.InvoiceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
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

        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(ILogger<InvoiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Invoice")]
        public ActionResult Get()
        {
            Invoice i = new Invoice();
            InvoiceLine il = new InvoiceLine();

            try
            {
                TransactionType = "GetInvoices";

                DataHandler dataHandler = new DataHandler();

                dtApiData = dataHandler.GetApiData(TransactionType);

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
                }

                HttpClient client;
                RestHandler restHandler = new RestHandler();

                client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("").Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync();

                    dynamic jObject = JsonConvert.DeserializeObject(dataObjects.Result);

                    foreach (JObject obj in jObject["data"])
                    {
                        if (obj["invoiceNumber"] != null)
                        {
                            i.InvoiceNumber = obj["invoiceNumber"].ToString();
                        }

                        else
                        {
                            i.InvoiceNumber = "";
                        }

                        if (obj["invoiceId"] != null)
                        {
                            i.InvoiceId = obj["invoiceId"].ToString();
                        }

                        else
                        {
                            i.InvoiceId = "";
                        }

                        if (obj["invoiceDate"] != null)
                        {
                            i.InvoiceDate = obj["invoiceDate"].ToString();
                        }

                        else
                        {
                            i.InvoiceDate = "";
                        }

                        if (obj["documentState"] != null)
                        {
                            i.DocumentState = Convert.ToInt32(obj["documentState"].ToString());
                        }

                        else
                        {
                            i.DocumentState = 0;
                        }

                        if (obj["orderNumber"] != null)
                        {
                            i.OrderNumber = obj["orderNumber"].ToString();
                        }

                        else
                        {
                            i.OrderNumber = "";
                        }

                        if (obj["externalOrderNumber"] != null)
                        {
                            i.ExternalOrderNumber = obj["externalOrderNumber"].ToString();
                        }

                        else
                        {
                            i.ExternalOrderNumber = "";
                        }

                        if (obj["customerCode"] != null)
                        {
                            i.CustomerCode = obj["customerCode"].ToString();
                        }

                        else
                        {
                            i.CustomerCode = "";
                        }

                        if (obj["grossTotalInVat"] != null)
                        {
                            i.GrossTotalInVat = Convert.ToDouble(obj["grossTotalInVat"].ToString());
                        }

                        else
                        {
                            i.GrossTotalInVat = 0;
                        }

                        if (obj["grossTotalExVat"] != null)
                        {
                            i.GrossTotalExVat = Convert.ToDouble(obj["grossTotalExVat"].ToString());
                        }

                        else
                        {
                            i.GrossTotalExVat = 0;
                        }

                        if (obj["grossTaxTotal"] != null)
                        {
                            i.GrossTaxTotal = Convert.ToDouble(obj["grossTaxTotal"].ToString());
                        }

                        else
                        {
                            i.GrossTaxTotal = 0;
                        }

                        if (obj["discountAmountInVat"] != null)
                        {
                            i.DiscountAmountInVat = Convert.ToDouble(obj["discountAmountInVat"].ToString());
                        }

                        else
                        {
                            i.DiscountAmountInVat = 0;
                        }

                        if (obj["discountAmountExVat"] != null)
                        {
                            i.DiscountAmountExVat = Convert.ToDouble(obj["discountAmountExVat"].ToString());
                        }

                        else
                        {
                            i.DiscountAmountExVat = 0;
                        }

                        if (obj["netTotalExVat"] != null)
                        {
                            i.NetTotalExVat = Convert.ToDouble(obj["netTotalExVat"].ToString());
                        }

                        else
                        {
                            i.NetTotalExVat = 0;
                        }

                        if (obj["netTaxTotal"] != null)
                        {
                            i.NetTaxTotal = Convert.ToDouble(obj["netTaxTotal"].ToString());
                        }

                        else
                        {
                            i.NetTaxTotal = 0;
                        }

                        if (obj["totalInvoiceRounding"] != null)
                        {
                            i.TotalInvoiceRounding = Convert.ToInt32(obj["totalInvoiceRounding"].ToString());
                        }

                        else
                        {
                            i.TotalInvoiceRounding = 0;
                        }

                        if (obj["netTotalInVat"] != null)
                        {
                            i.NetTotalInVat = Convert.ToDouble(obj["netTotalInVat"].ToString());
                        }

                        else
                        {
                            i.NetTotalInVat = 0;
                        }

                        dataHandler.CreateInvoice(i.InvoiceNumber,i.InvoiceId,i.InvoiceDate,i.DocumentState,i.OrderNumber,i.ExternalOrderNumber
                            ,i.CustomerCode,i.GrossTotalInVat,i.GrossTotalExVat,i.GrossTaxTotal,i.DiscountAmountInVat
                            ,i.DiscountAmountExVat,i.NetTotalExVat,i.NetTaxTotal,i.TotalInvoiceRounding,i.NetTotalInVat);

                        JArray dataArrayLines = (JArray)obj["Lines"];

                        int itemCount = 0;

                        foreach (JObject objLines in dataArrayLines)
                        {
                            itemCount++;

                            if (il.InvoiceNumber == obj["invoiceNumber"].ToString() && il.InvoiceId == Convert.ToInt32(objLines["invoiceId"]))
                            {
                                if (obj["invoiceNumber"] != null)
                                {
                                    il.InvoiceNumber = obj["invoiceNumber"].ToString();
                                }

                                else
                                {
                                    il.InvoiceNumber = "";
                                }

                                if (obj["invoiceId"] != null)
                                {
                                    il.InvoiceId = Convert.ToInt32(obj["invoiceId"].ToString());
                                }

                                else
                                {
                                    il.InvoiceId = 0;
                                }

                                if (obj["warehouseCode"] != null)
                                {
                                    il.WarehouseCode = obj["warehouseCode"].ToString();
                                }

                                else
                                {
                                    il.WarehouseCode = "";
                                }

                                if (obj["itemCode"] != null)
                                {
                                    il.ItemCode = obj["itemCode"].ToString();
                                }

                                else
                                {
                                    il.ItemCode = "";
                                }

                                if (obj["moduleCode"] != null)
                                {
                                    il.ModuleCode = obj["moduleCode"].ToString();
                                }

                                else
                                {
                                    il.ModuleCode = "";
                                }

                                if (obj["lineDescription"] != null)
                                {
                                    il.LineDescription = obj["lineDescription"].ToString();
                                }

                                else
                                {
                                    il.LineDescription = "";
                                }

                                if (obj["unitPriceExcl"] != null)
                                {
                                    il.UnitPriceExcl = Convert.ToDouble(obj["unitPriceExcl"].ToString());
                                }

                                else
                                {
                                    il.UnitPriceExcl = 0;
                                }

                                if (obj["unitPriceIncl"] != null)
                                {
                                    il.UnitPriceIncl = Convert.ToDouble(obj["unitPriceIncl"].ToString());
                                }

                                else
                                {
                                    il.UnitPriceIncl = 0;
                                }

                                if (obj["quantity"] != null)
                                {
                                    il.Quantity = Convert.ToInt32(obj["quantity"].ToString());
                                }

                                else
                                {
                                    il.Quantity = 0;
                                }

                                if (obj["unitOfMeasure"] != null)
                                {
                                    il.UnitOfMeasure = obj["unitOfMeasure"].ToString();
                                }

                                else
                                {
                                    il.UnitOfMeasure = "";
                                }

                                if (obj["lineNetTotalOrderedInVat"] != null)
                                {
                                    il.LineNetTotalOrderedInVat = Convert.ToDouble(obj["lineNetTotalOrderedInVat"].ToString());
                                }

                                else
                                {
                                    il.LineNetTotalOrderedInVat = 0;
                                }

                                if (obj["lineNetTotalOrderedExVat"] != null)
                                {
                                    il.LineNetTotalOrderedExVat = Convert.ToDouble(obj["lineNetTotalOrderedExVat"].ToString());
                                }

                                else
                                {
                                    il.LineNetTotalOrderedExVat = 0;
                                }

                                if (obj["lineNetTotalProcessedInVat"] != null)
                                {
                                    il.LineNetTotalProcessedInVat = Convert.ToDouble(obj["lineNetTotalProcessedInVat"].ToString());
                                }

                                else
                                {
                                    il.LineNetTotalProcessedInVat = 0;
                                }

                                if (obj["lineNetTotalProcessedExVat"] != null)
                                {
                                    il.LineNetTotalProcessedExVat = Convert.ToDouble(obj["lineNetTotalProcessedExVat"].ToString());
                                }

                                else
                                {
                                    il.LineNetTotalProcessedExVat = 0;
                                }

                                if (obj["lineNotes"] != null)
                                {
                                    il.LineNotes = obj["lineNotes"].ToString();
                                }

                                else
                                {
                                    il.LineNotes = "";
                                }

                                dataHandler.CreateInvoiceLines(il.InvoiceNumber,il.InvoiceId,il.WarehouseCode,il.ItemCode
                                                                ,il.ModuleCode,il.LineDescription,il.UnitPriceExcl
                                                                ,il.UnitPriceIncl,il.Quantity,il.UnitOfMeasure
                                                                ,il.LineNetTotalOrderedInVat,il.LineNetTotalOrderedExVat
                                                                ,il.LineNetTotalProcessedInVat,il.LineNetTotalProcessedExVat,il.LineNotes);


                            }

                        }

                    }


                    return Created(Request.GetDisplayUrl(), null);
                }

                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
