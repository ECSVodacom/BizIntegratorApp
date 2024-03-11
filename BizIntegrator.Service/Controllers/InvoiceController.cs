using BizIntegrator.Data;
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
using BizIntegrator.Service.Repository;
using BizIntegrator.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using BizIntegrator.PostToBiz;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace BizIntegrator.Service.Controllers
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
        string Method { get; set; }



        private readonly ILogger<CustomerListController> _logger;
        private readonly IConfiguration _configuration;

        public InvoiceController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "Invoice")]
        public ActionResult Post()
        {
            DataTable dtApiData;

            Invoice i = new Invoice();
            InvoiceLine il = new InvoiceLine();
            string resp = string.Empty;

            string errorMessage = "Errors encountered";

            TransactionType = "GetInvoices";

            DataHandler dataHandler = new DataHandler();

            dtApiData = dataHandler.GetApiData(TransactionType);

            if (dtApiData.Rows.Count > 0)
            {

                Id = dtApiData.Rows[0]["Id"].ToString();
                ApiKey = dtApiData.Rows[0]["AccountKey"].ToString();
                Name = dtApiData.Rows[0]["Name"].ToString();
                Url = dtApiData.Rows[0]["EndPoint"].ToString();
                PrivateKey = dtApiData.Rows[0]["PrivateKey"].ToString();
                Username = dtApiData.Rows[0]["Username"].ToString();
                Password = dtApiData.Rows[0]["Password"].ToString();
                AuthenticationType = dtApiData.Rows[0]["AuthenticationType"].ToString();
                UseAPIKey = dtApiData.Rows[0]["UseAPIKey"].ToString();

                HttpClient client;
                RestHandler restHandler = new RestHandler();

                var dateString = DateTime.Now.ToString("yyyy-MM-dd");

                var postedUrl = Url + dateString;

                client = restHandler.SetClient(Id, Name, postedUrl, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("").Result;

                resp = response.ToString();

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync();

                    dynamic jObject = JsonConvert.DeserializeObject(dataObjects.Result);

                    try
                    {
                        foreach (JObject obj in jObject["data"])
                        {
                            bool dataExists = dataHandler.CheckInvoice(obj["invoiceId"].ToString());

                            if (!dataExists)
                            {
                                obj.Add("GlnNumber", new JValue("6001651190618"));

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
                                    i.InvoiceId = Convert.ToInt32(obj["invoiceId"].ToString());
                                }

                                else
                                {
                                    i.InvoiceId = 0;
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

                                i.Processed = false;

                                dataHandler.CreateInvoice(i.InvoiceNumber, Convert.ToInt32(i.InvoiceId), i.InvoiceDate, i.DocumentState, i.OrderNumber, i.ExternalOrderNumber
                                    , i.CustomerCode, i.GrossTotalInVat, i.GrossTotalExVat, i.GrossTaxTotal, i.DiscountAmountInVat
                                    , i.DiscountAmountExVat, i.NetTotalExVat, i.NetTaxTotal, i.TotalInvoiceRounding, i.NetTotalInVat, (bool)i.Processed, Id);

                                JArray dataArrayLines = (JArray)obj["Lines"];

                                int itemCount = 0;

                                foreach (JObject objLines in dataArrayLines)
                                {
                                    itemCount++;

                                    i.InvoiceId = Convert.ToInt32(obj["invoiceId"].ToString());

                                    if (i.InvoiceId == Convert.ToInt32(objLines["invoiceId"]))
                                    {
                                        if (objLines["invoiceNumber"] != null)
                                        {
                                            il.InvoiceNumber = objLines["invoiceNumber"].ToString();
                                        }

                                        else
                                        {
                                            il.InvoiceNumber = "";
                                        }

                                        if (objLines["invoiceId"] != null)
                                        {
                                            il.InvoiceId = Convert.ToInt32(objLines["invoiceId"].ToString());
                                        }

                                        else
                                        {
                                            il.InvoiceId = 0;
                                        }

                                        if (objLines["warehouseCode"] != null)
                                        {
                                            il.WarehouseCode = objLines["warehouseCode"].ToString();
                                        }

                                        else
                                        {
                                            il.WarehouseCode = "";
                                        }

                                        if (objLines["itemCode"] != null)
                                        {
                                            il.ItemCode = objLines["itemCode"].ToString();
                                        }

                                        else
                                        {
                                            il.ItemCode = "";
                                        }

                                        if (objLines["moduleCode"] != null)
                                        {
                                            il.ModuleCode = objLines["moduleCode"].ToString();
                                        }

                                        else
                                        {
                                            il.ModuleCode = "";
                                        }

                                        if (objLines["lineDescription"] != null)
                                        {
                                            il.LineDescription = objLines["lineDescription"].ToString();
                                        }

                                        else
                                        {
                                            il.LineDescription = "";
                                        }

                                        if (objLines["unitPriceExcl"] != null)
                                        {
                                            il.UnitPriceExcl = Convert.ToDouble(objLines["unitPriceExcl"].ToString());
                                        }

                                        else
                                        {
                                            il.UnitPriceExcl = 0;
                                        }

                                        if (objLines["unitPriceIncl"] != null)
                                        {
                                            il.UnitPriceIncl = Convert.ToDouble(objLines["unitPriceIncl"].ToString());
                                        }

                                        else
                                        {
                                            il.UnitPriceIncl = 0;
                                        }

                                        if (objLines["quantity"] != null)
                                        {
                                            il.Quantity = Convert.ToInt32(objLines["quantity"].ToString());
                                        }

                                        else
                                        {
                                            il.Quantity = 0;
                                        }

                                        if (objLines["unitOfMeasure"] != null)
                                        {
                                            il.UnitOfMeasure = objLines["unitOfMeasure"].ToString();
                                        }

                                        else
                                        {
                                            il.UnitOfMeasure = "";
                                        }

                                        if (objLines["lineNetTotalOrderedInVat"] != null)
                                        {
                                            il.LineNetTotalOrderedInVat = Convert.ToDouble(objLines["lineNetTotalOrderedInVat"].ToString());
                                        }

                                        else
                                        {
                                            il.LineNetTotalOrderedInVat = 0;
                                        }

                                        if (objLines["lineNetTotalOrderedExVat"] != null)
                                        {
                                            il.LineNetTotalOrderedExVat = Convert.ToDouble(objLines["lineNetTotalOrderedExVat"].ToString());
                                        }

                                        else
                                        {
                                            il.LineNetTotalOrderedExVat = 0;
                                        }

                                        if (objLines["lineNetTotalProcessedInVat"] != null)
                                        {
                                            il.LineNetTotalProcessedInVat = Convert.ToDouble(objLines["lineNetTotalProcessedInVat"].ToString());
                                        }

                                        else
                                        {
                                            il.LineNetTotalProcessedInVat = 0;
                                        }

                                        if (objLines["lineNetTotalProcessedExVat"] != null)
                                        {
                                            il.LineNetTotalProcessedExVat = Convert.ToDouble(objLines["lineNetTotalProcessedExVat"].ToString());
                                        }

                                        else
                                        {
                                            il.LineNetTotalProcessedExVat = 0;
                                        }

                                        if (objLines["lineNotes"] != null)
                                        {
                                            il.LineNotes = objLines["lineNotes"].ToString();
                                        }

                                        else
                                        {
                                            il.LineNotes = "";
                                        }

                                        dataHandler.CreateInvoiceLines(il.InvoiceNumber, il.InvoiceId, il.WarehouseCode, il.ItemCode
                                                                        , il.ModuleCode, il.LineDescription, il.UnitPriceExcl
                                                                        , il.UnitPriceIncl, il.Quantity, il.UnitOfMeasure
                                                                        , il.LineNetTotalOrderedInVat, il.LineNetTotalOrderedExVat
                                                                        , il.LineNetTotalProcessedInVat, il.LineNetTotalProcessedExVat, il.LineNotes);


                                    }

                                }
                                string fileName = string.Empty;

                                DateTime date = DateTime.Now;
                                string formattedDate = date.ToString("yyyyMMdd");

                                fileName = "DANNIC" + "-INVOICE-" + obj["invoiceId"].ToString() + "-" + formattedDate + ".json";

                                BizHandler bizHandler = new BizHandler();

                                bool invoiceProcessed = dataHandler.CheckProcessedInvoice(obj["invoiceId"].ToString());

                                if (!invoiceProcessed)
                                {
                                    bizHandler.PostToBiz(obj.ToString(), fileName, "6001651190618", "6001001030007");
                                    dataHandler.UpdateProcessedInvoice(obj["invoiceId"].ToString());
                                }
                            }
                        }

                        return Created(Request.GetDisplayUrl(), resp);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
                    }
                }

                else
                {
                    return BadRequest(new { Message = errorMessage, ExceptionDetails = response.StatusCode });
                }
            }

            else
            {
                return BadRequest(new { Message = errorMessage });
            }


        }


    }
}
