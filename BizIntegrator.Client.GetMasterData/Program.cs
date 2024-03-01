using BizIntegrator.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Client.GetMasterData
{
    internal class Program
    {
        static async Task Main()
        {
            await GetStockBarcodeData();

            await GetCustomerData();
        }

        static async Task GetStockBarcodeData()
        {
            string apiUrl = string.Empty;

            DataHandler dataHandler = new DataHandler();

            string TransactionType = "GetStockBarcode";


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
                                Console.WriteLine("Stock List data has been successfully imported");
                            }
                            else
                            {
                                Console.WriteLine("Stock List data import failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred: {ex.InnerException}");
                        }
                    }
                }
            }



        }

        static async Task GetCustomerData()
        {
            string apiUrl = string.Empty;
            DataHandler dataHandler = new DataHandler();

            string TransactionType = "GetCustList";

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
