using BizIntegrator.Data;
using BizIntegrator.Security;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace BizIntegrator.OrderManager
{
    public class RestHandler
    {
        public string JwtToken { get; set; }
        public string CompNo { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public HttpClient SetClient(string id, string name, string url, string apiKey, string privateKey, string username, string password, string authenticationType, string useAPIKey)
        {
            DataHandler dataHandler = new DataHandler();


            HttpClient client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri(url);

                if (authenticationType == "OAuth")
                {
                    JwtToken = GenerateJwtAsync(id, name, username, password).Result;
                }

                else if (authenticationType == "APIKey")
                {
                    JwtToken = GenerateSignedJwtAsync(apiKey, privateKey).Result;
                }

                string sAuthorization = "Bearer " + JwtToken;

                client.DefaultRequestHeaders.Add("Authorization", sAuthorization);


                if (useAPIKey == "1")
                {
                    client.DefaultRequestHeaders.Add("ns-api-key", apiKey);
                }

                return client;
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "SetClient");

                return client;
            }
        }

        private static async Task<string> GenerateSignedJwtAsync(string apiKey, string privateKey)
        {
            DataHandler dataHandler = new DataHandler();

            try
            {
                TokenPayload tokenPayload = new TokenPayload();
                tokenPayload.Exp = 15;
                Guid guid = Guid.NewGuid();
                tokenPayload.Jti = guid.ToString();
                tokenPayload.ApiKey = apiKey;
                string text = "";

                text = await tokenPayload.EncodeTokenAsync((Func<Task<object>>)(() => JwtKeysFunctions.ReadKeyFromFileAsync(privateKey)));

                string jwt = text;

                return jwt;
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.Message, "GenerateSignedJwtAsync");

                return ex.Message;
            }

        }

        private static async Task<string> GenerateJwtAsync(string _id, string _compNo, string _username, string _password)
        {
            DataHandler dataHandler = new DataHandler();

            string transactionType = "PostLogin";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    DataTable dtLoginUrl = dataHandler.GetApiData(transactionType);
                    DataRow row = dtLoginUrl.Rows[0];
                    string loginUrl = row["EndPoint"].ToString();

                    var credentials = new
                    {
                        compno = _compNo,
                        userId = _username,
                        password = _password
                    };

                    string json = JsonConvert.SerializeObject(credentials);

                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(loginUrl, content);

                    string jwt = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        string tokenJson = await response.Content.ReadAsStringAsync();
                        var tokenData = JsonConvert.DeserializeAnonymousType(tokenJson, new { accessToken = "" });

                        jwt = tokenData.accessToken;

                    }
                    return jwt;
                }
            }
            catch (Exception ex)
            {
                dataHandler.WriteException(ex.InnerException.Message, "GenerateJwtAsync");

                return ex.Message;
            }

        }


    }

}
