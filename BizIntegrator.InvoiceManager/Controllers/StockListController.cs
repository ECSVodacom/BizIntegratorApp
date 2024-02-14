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
using Microsoft.AspNetCore.Http.Extensions;

namespace BizIntegrator.InvoiceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockListController : ControllerBase
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

        private readonly ILogger<StockListController> _logger;

        public StockListController(ILogger<StockListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "StockList")]
        public ActionResult Get()
        {
            StockList s = new StockList();
            try
            {
                TransactionType = "GetStockList";

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
                        if (obj["code"] != null)
                        {
                            s.ProductCode = obj["code"].ToString();
                        }

                        else
                        {
                            s.ProductCode = "";
                        }

                        if (obj["name"] != null)
                        {
                            s.ProductName = obj["name"].ToString();
                        }

                        else
                        {
                            s.ProductName = "";
                        }

                        if (obj["alternateName"] != null)
                        {
                            s.AlternateName = obj["alternateName"].ToString();
                        }

                        else
                        {
                            s.AlternateName = "";
                        }

                        if (obj["priceInVat"] != null)
                        {
                            s.PriceInVat = Convert.ToDouble(obj["priceInVat"].ToString());
                        }

                        else
                        {
                            s.PriceInVat = 0;
                        }

                        if (obj["priceExVat"] != null)
                        {
                            s.PriceExVat = Convert.ToDouble(obj["priceExVat"].ToString());
                        }

                        else
                        {
                            s.PriceExVat = 0;
                        }

                        if (obj["bottle_Barcode"] != null)
                        {
                            s.BottleBarcode = obj["bottle_Barcode"].ToString();
                        }

                        else
                        {
                            s.BottleBarcode = "";
                        }

                        if (obj["case_Barcode"] != null)
                        {
                            s.CaseBarcode = obj["case_Barcode"].ToString();
                        }
                        else
                        {
                            s.CaseBarcode = "";
                        }

                        dataHandler.CreateStockList(s.ProductCode,s.ProductName,s.AlternateName,s.PriceInVat,s.PriceExVat,s.BottleBarcode,s.CaseBarcode );
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
