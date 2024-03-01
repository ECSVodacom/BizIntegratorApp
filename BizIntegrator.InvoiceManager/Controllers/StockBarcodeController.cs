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
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using BizIntegrator.Models;

namespace BizIntegrator.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockBarcodeController : ControllerBase
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

        DataTable dtApiData;

        private readonly ILogger<CustomerListController> _logger;
        private readonly IConfiguration _configuration;

        public StockBarcodeController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "StockBarcode")]
        public ActionResult Get()
        {
            string errorMessage = "Errors encountered";
            StockBarcode s = new StockBarcode();
            try
            {
                TransactionType = "GetStockBarcode";

                DataHandler dataHandler = new DataHandler();

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
                    }
                }

                else
                {
                    return BadRequest(new { Message = errorMessage });
                }

                HttpClient client;
                RestHandler restHandler = new RestHandler();

                client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                UriBuilder uriBuilder = new UriBuilder(Url);

                uriBuilder.Query = "ItemCode";

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(uriBuilder.Uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync();

                    dynamic jObject = JsonConvert.DeserializeObject(dataObjects.Result);

                    try
                    {
                        foreach (JObject obj in jObject["data"])
                        {
                            bool dataExists = dataHandler.CheckStockBarcode(obj["itemCode"].ToString());

                            if (!dataExists)
                            {
                                if (obj["itemCode"] != null)
                                {
                                    s.ProductCode = obj["itemCode"].ToString();
                                }

                                else
                                {
                                    s.ProductCode = "";
                                }

                                if (obj["itemDescription"] != null)
                                {
                                    s.ProductDescription = obj["itemDescription"].ToString();
                                }

                                else
                                {
                                    s.ProductDescription = "";
                                }

                                if (obj["barcodeBottle"] != null)
                                {
                                    s.BottleBarcode = obj["barcodeBottle"].ToString();
                                }

                                else
                                {
                                    s.BottleBarcode = "";
                                }

                                if (obj["barcodeCase"] != null)
                                {
                                    s.CaseBarcode = obj["barcodeCase"].ToString();
                                }

                                else
                                {
                                    s.CaseBarcode = "";
                                }

                                if (obj["uomBottle"] != null)
                                {
                                    s.BottleUom = obj["uomBottle"].ToString();
                                }

                                else
                                {
                                    s.BottleUom = "";
                                }

                                if (obj["uomCase"] != null)
                                {
                                    s.CaseUom = obj["uomCase"].ToString();
                                }

                                else
                                {
                                    s.CaseUom = "";
                                }

                                if (obj["unitsPerCase"] != null)
                                {
                                    s.UnitsPerCase = Convert.ToInt32(obj["unitsPerCase"].ToString());
                                }
                                else
                                {
                                    s.UnitsPerCase = 0;
                                }

                                dataHandler.CreateStocBarcode(s.ProductCode, s.ProductDescription, s.BottleBarcode, s.CaseBarcode, s.BottleUom, s.CaseUom, s.UnitsPerCase, Id);

                            }
                        }
                        return Created(Request.GetDisplayUrl(), null);
                    }

                    catch (Exception ex)
                    {
                        return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
                    }

                }

                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
            }
        }
    }
}
