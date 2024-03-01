using BizIntegrator.OrderManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BizIntegrator.Data;
using System.Security.Policy;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BizIntegrator.Client
{
    public static class Program
    {
        static async Task Main()
        {
            await PostOrders();

            await GetOrders();

            await GetInvoices();
        }

        static async Task PostOrders()
        {
            string apiUrl = string.Empty;
            DataHandler dataHandler = new DataHandler();

            string TransactionType = "PostOrders";

            DataTable dtApiData = dataHandler.GetInternalAPIData(TransactionType);

            foreach (DataRow row in dtApiData.Rows)
            {
                apiUrl = row["EndPoint"].ToString();
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                    var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Order has been successfully created");
                    }
                    else
                    {
                        Console.WriteLine("Order creation failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static async Task GetOrders()
        {
            string apiUrl = string.Empty;
            DataHandler dataHandler = new DataHandler();

            string TransactionType = "GetOrders";

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
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Customer List data has been successfully imported");
                            }
                            else
                            {
                                Console.WriteLine("Customer List data import failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }



        }

        static async Task GetInvoices()
        {
            string apiUrl = string.Empty;
            DataHandler dataHandler = new DataHandler();

            string TransactionType = "GetInvoices";

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
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Customer List data has been successfully imported");
                            }
                            else
                            {
                                Console.WriteLine("Customer List data import failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }



        }
    }
}
