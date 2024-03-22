using BizIntegrator.Data;
using BizIntegrator.Models;
using BizIntegrator.PostToBiz;
using BizIntegrator.Service.Repository;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace BizIntegrator.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderGETController : ControllerBase
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

        private readonly ILogger<OrderGETController> _logger;
        private readonly IConfiguration _configuration;

        public OrderGETController(ILogger<OrderGETController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "OrderGET")]
        public ActionResult Post()
        {
            DataTable dtApiData;

            string errorMessage = "Errors encountered";
            DataHandler dataHandler = new DataHandler();
            string resp = string.Empty;
            string headerName = "APIName";
            TransactionType = "Orders";
            try
            {

                if (HttpContext.Request.Headers.TryGetValue(headerName, out var headerValues))
                {
                    string headerValue = headerValues.FirstOrDefault();

                    if (!string.IsNullOrEmpty(headerValue) && headerValue.Equals("PLASTIC", StringComparison.OrdinalIgnoreCase))
                    {
                        dtApiData = dataHandler.GetApiDataPerName(headerValue, TransactionType);

                        if (dtApiData.Rows.Count > 0)
                        {
                            Name = headerValue;
                            Id = dtApiData.Rows[0]["Id"].ToString();
                            ApiKey = dtApiData.Rows[0]["AccountKey"].ToString();
                            Url = dtApiData.Rows[0]["EndPoint"].ToString();
                            PrivateKey = dtApiData.Rows[0]["PrivateKey"].ToString();
                            Username = dtApiData.Rows[0]["Username"].ToString();
                            Password = dtApiData.Rows[0]["Password"].ToString();
                            AuthenticationType = dtApiData.Rows[0]["AuthenticationType"].ToString();
                            UseAPIKey = dtApiData.Rows[0]["UseAPIKey"].ToString();

                            OrderHandler orderHandler = new OrderHandler();

                            try
                            {
                                string Response = orderHandler.CreateOrders(Id, ApiKey, Name, Url, PrivateKey, Username, Password, AuthenticationType, UseAPIKey);
                                resp = Response;

                            }
                            catch (Exception ex)
                            {
                                dataHandler.WriteException(ex.InnerException.ToString(), "Get Orders");
                            }
                        }

                        else
                        {
                            resp = "Error found - No APIs found";
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


    }
}
