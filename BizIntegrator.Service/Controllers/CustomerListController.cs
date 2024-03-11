using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using BizIntegrator.Service.Repository;
using BizIntegrator.Data;
using System.Net.NetworkInformation;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using BizIntegrator.Models;
using System.Reflection;
using System.Net;

namespace BizIntegrator.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerListController : ControllerBase
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

        public class MyDataModel
        {
            public string customerListData { get; set; }
        }

        public CustomerListController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "CustomerList")]
        public ActionResult Post()
        {
            DataTable dtApiData;

            string errorMessage = "Errors encountered";
            string resp = string.Empty;
            CustomerList c = new CustomerList();
            try
            {
                TransactionType = "GetCustList";

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
                                bool dataExists = dataHandler.CheckCustomerList(obj["code"].ToString());

                                if (!dataExists)
                                {
                                    if (obj["code"] != null)
                                    {
                                        c.CustomerCode = obj["code"].ToString();
                                    }

                                    else
                                    {
                                        c.CustomerCode = "";
                                    }

                                    if (obj["name"] != null)
                                    {
                                        c.CustomerName = obj["name"].ToString();
                                    }

                                    else
                                    {
                                        c.CustomerName = "";
                                    }

                                    if (obj["physicalAddress1"] != null ||
                                        obj["physicalAddress2"] != null ||
                                        obj["physicalAddress3"] != null ||
                                        obj["physicalAddress4"] != null ||
                                        obj["physicalAddress5"] != null)
                                    {
                                        c.PhysicalAddress = obj["physicalAddress1"].ToString() + ""
                                        + obj["physicalAddress2"].ToString() + ""
                                        + obj["physicalAddress3"].ToString() + ""
                                        + obj["physicalAddress4"].ToString() + ""
                                        + obj["physicalAddress5"].ToString();
                                    }

                                    else
                                    {
                                        c.PhysicalAddress = "";
                                    }

                                    if (obj["eMail"] != null)
                                    {
                                        c.Email = obj["eMail"].ToString();
                                    }

                                    else
                                    {
                                        c.Email = "";
                                    }

                                    if (obj["ucARBrnchNo"] != null)
                                    {
                                        c.UcARBrnchNo = obj["ucARBrnchNo"].ToString();
                                    }

                                    else
                                    {
                                        c.UcARBrnchNo = "";
                                    }

                                    if (obj["branchCode"] != null)
                                    {
                                        c.BranchCode = obj["branchCode"].ToString();
                                    }

                                    else
                                    {
                                        c.BranchCode = "";
                                    }

                                    if (obj["dTimeStamp"] != null)
                                    {
                                        c.DateTimeStamp = obj["dTimeStamp"].ToString();
                                    }

                                    else
                                    {
                                        c.DateTimeStamp = "";
                                    }

                                    if (obj["group"] != null)
                                    {
                                        c.GroupCode = obj["group"].ToString();
                                    }
                                    else
                                    {
                                        c.GroupCode = "";
                                    }

                                    if (obj["groupDescription"] != null)
                                    {
                                        c.GroupDescription = obj["groupDescription"].ToString();
                                    }
                                    else
                                    {
                                        c.GroupDescription = "";
                                    }

                                    if (obj["area"] != null)
                                    {
                                        c.Area = obj["area"].ToString();
                                    }
                                    else
                                    {
                                        c.Area = "";
                                    }

                                    if (obj["areaDescription"] != null)
                                    {
                                        c.AreaDescription = obj["areaDescription"].ToString();
                                    }
                                    else
                                    {
                                        c.AreaDescription = "";
                                    }

                                    if (obj["ulARWHLinked"] != null)
                                    {
                                        c.UlARWHLinked = obj["ulARWHLinked"].ToString();
                                    }
                                    else
                                    {
                                        c.UlARWHLinked = "";
                                    }


                                    dataHandler.CreateCustomerList(c.CustomerCode, c.CustomerName, c.PhysicalAddress, c.Email
                                        , c.UcARBrnchNo, c.BranchCode, c.DateTimeStamp, c.GroupCode, c.GroupDescription, c.Area
                                        , c.AreaDescription, c.UlARWHLinked, Id);
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
