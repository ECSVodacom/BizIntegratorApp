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

        DataTable dtApiData;

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

        [HttpGet(Name = "CustomerList")]
        public ActionResult Get()
        {
            string errorMessage = "Errors encountered";

            CustomerList c = new CustomerList();
            try
            {
                TransactionType = "GetCustList";

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

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("").Result;

                if (response.IsSuccessStatusCode)
                {
                    List<Task> tasks = new List<Task>();

                    var dataObjects = response.Content.ReadAsStringAsync();

                    dynamic jObject = JsonConvert.DeserializeObject(dataObjects.Result);

                    try {
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

                                //ucARBrnchNo
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

                                var sampleData = new MyDataModel { customerListData = obj.ToString() };
                            }
                        }
                        return Created(Request.GetDisplayUrl(), "Customer list has been successessfully imported ");
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

        [HttpGet(Name = "CustomerList")]
        [HttpPost]
        public ActionResult Post()
        {
            try
            {
                DataHandler dataHandler = new DataHandler();

                DataTable dtCust = dataHandler.GetCustomerData();

                // Return the data in the response
                return Ok(dtCust);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        //private DataTable GetDataFromDatabase(DataTable request)
        //{
        //    DataHandler dataHandler = new DataHandler();

        //    DataTable dtCust = dataHandler.GetCustomerData();

        //    List<CustomerList> custModelList = new List<CustomerList>();

        //    foreach (DataRow row in dtCust.Rows)
        //    {
        //        CustomerList model = new CustomerList
        //        {
        //            CustomerCode = row["Originator"].ToString(),
        //            CustomerName = row["Recipient"].ToString(),
        //            PhysicalAddress = row["Type"].ToString(),
        //            Email = row["SubType"].ToString(),
        //            UcARBrnchNo = row["Total"].ToString(),
        //            BranchCode = row["Total"].ToString(),
        //            DateTimeStamp = row["Total"].ToString(),
        //            GroupCode = row["Total"].ToString(),
        //            GroupDescription = row["Total"].ToString(),
        //            Area = row["Total"].ToString(),
        //            AreaDescription = row["Total"].ToString(),
        //            UlARWHLinked = row["Total"].ToString()
        //        };

        //        custModelList.Add(model);
        //    }

        //    return dtCust;


        //}
    }
}
