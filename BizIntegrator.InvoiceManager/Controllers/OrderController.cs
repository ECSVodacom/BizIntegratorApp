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
using BizIntegrator.InvoiceManager.Repository;
using Microsoft.AspNetCore.Http.Extensions;


namespace BizIntegrator.InvoiceManager.Controllers
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

        DataTable dtApiData;

        [HttpPost(Name = "Order")]
        public ActionResult Post([FromBody] JsonElement orders)
        {
            string jsonString = orders.ToString();

            HttpClient client;
            DataHandler dataHandler = new DataHandler();

            try
            {
                TransactionType = "PostOrders";

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

                RestHandler restHandler = new RestHandler();

                client = restHandler.SetClient(Id, Name, Url, ApiKey, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                DataTable dtPostDataUrl = dataHandler.GetApiData(TransactionType);
                DataRow row = dtPostDataUrl.Rows[0];
                string postDataUrl = row["EndPoint"].ToString();

                dynamic jObject = JsonConvert.DeserializeObject(jsonString);

                JArray dataArray;

                dataArray = jObject;

                var content = new StringContent(dataArray.ToString(), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(postDataUrl, content).Result;

                return Created(Request.GetDisplayUrl(), null);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

}
