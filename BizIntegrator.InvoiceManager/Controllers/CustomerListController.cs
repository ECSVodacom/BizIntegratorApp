using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;
using BizIntegrator.InvoiceManager.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using BizIntegrator.InvoiceManager.Repository;
using BizIntegrator.Data;
using System.Net.NetworkInformation;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.Extensions;

namespace BizIntegrator.InvoiceManager.Controllers
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

        DataTable dtApiData;

        private readonly ILogger<CustomerListController> _logger;

        public CustomerListController(ILogger<CustomerListController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "CustomerList")]
        public ActionResult Get()
        {
            CustomerList c = new CustomerList();
            try
            {
                TransactionType = "GetCustomerList";

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

                        if (obj["BranchCode"] != null)
                        {
                            c.BranchCode = obj["BranchCode"].ToString();
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

                        dataHandler.CreateCustomerList(c.CustomerCode, c.CustomerName, c.PhysicalAddress, c.Email
                            , c.BranchCode, Convert.ToDateTime(c.DateTimeStamp), c.GroupCode, c.GroupDescription, c.Area, c.AreaDescription);
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
