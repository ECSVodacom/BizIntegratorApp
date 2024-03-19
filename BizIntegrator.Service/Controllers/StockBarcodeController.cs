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
using System.Net;
using System.Threading.Tasks;
using System.Threading;

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

        private readonly ILogger<CustomerListController> _logger;
        private readonly IConfiguration _configuration;

        public StockBarcodeController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "StockBarcode")]
        public ActionResult Post()
        {
            DataTable dtApiData;

            string resp = string.Empty;
            string errorMessage = "Errors encountered";
            StockBarcode s = new StockBarcode();
            try
            {
                TransactionType = "GetStockBarcode";

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

                    client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                    UriBuilder uriBuilder = new UriBuilder(Url);

                    uriBuilder.Query = "ItemCode";

                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(uriBuilder.Uri).Result;

                    resp = response.ToString();

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
                            return Created(Request.GetDisplayUrl(), resp);
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

                else
                {
                    return BadRequest(new { Message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
            }
        }

    }
}
