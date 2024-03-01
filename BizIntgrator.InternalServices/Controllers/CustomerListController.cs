using BizIntegrator.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace BizIntgrator.InternalServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerListController : ControllerBase
    {
        private readonly ILogger<CustomerListController> _logger;
        private readonly IConfiguration _configuration;

        public CustomerListController(ILogger<CustomerListController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "CustomerList")]
        public async Task<ActionResult> Post()
        {
            string errorMessage = "Errors encountered";

            string apiUrl = string.Empty;

            DataHandler dataHandler = new DataHandler();

            string TransactionType = "PostCustList";

            try
            {
                DataTable dtApiData = dataHandler.GetInternalAPIData(TransactionType);

                if (dtApiData.Rows.Count > 0)
                {
                    foreach (DataRow row in dtApiData.Rows)
                    {
                        apiUrl = row["EndPoint"].ToString();

                        using (HttpClient client = new HttpClient())
                        {
                            try
                            {
                                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                                HttpResponseMessage response = await client.GetAsync(apiUrl);

                                Thread.Sleep(5000);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.InnerException}");
                            }
                        }
                    }


                }
                return Created(Request.GetDisplayUrl(), "Invoices has been successfully imported and sent to biz");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = errorMessage, ExceptionDetails = ex.Message });
            }

        }
    }
}
