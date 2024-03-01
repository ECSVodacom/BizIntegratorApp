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

                    }
                }


                if (Name == "Diageo_Vodacom")
                {
                    RestHandler restHandler = new RestHandler();

                    client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    string postDataUrl = Url;

                    dynamic jObject = JsonConvert.DeserializeObject(jsonString);

                    foreach (JObject obj in jObject)
                    {
                        string branchCode = string.Empty;
                        string customerCode = string.Empty;

                        if (obj["branchCode"] != null)
                        {
                            branchCode = obj["branchCode"].ToString();

                            //Lookup customer data from json provided
                            customerCode = dataHandler.GetCustomerCode(branchCode);

                            obj["customerCode"] = customerCode;
                        }

                        else
                        {
                            branchCode = "";
                            customerCode = "";
                        }

                        if (obj["lines"] is JArray linesArray)
                        {
                            foreach (JObject objLine in linesArray)
                            {
                                string test = "";
                                test = "hello";
                                // Lookup stock data from json provided
                            }
                        }

                        var content = new StringContent(jObject.ToString(), System.Text.Encoding.UTF8, "application/json");

                        HttpResponseMessage response = client.PostAsync(postDataUrl, content).Result;
                    }



                }

                return Created(Request.GetDisplayUrl(), null);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet(Name = "Order")]
        [Consumes("application/json")]
        public ActionResult Get()
        {
            DataHandler dataHandler = new DataHandler();

            try
            {
                TransactionType = "GetOrders";
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

                if (Name == "PLASTIC")
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
                return Created(Request.GetDisplayUrl(), null);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

}
