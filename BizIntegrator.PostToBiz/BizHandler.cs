using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BizIntegrator.PostToBiz
{
    public class PostResult
    {
        /// <summary>
        /// Indicates whether or not the post was successful
        /// </summary>
        public bool IsPosted { get; set; }

        /// <summary>
        /// Custom or system message returned from posting 
        /// </summary>
        public string Message { get; set; }
    }

    public class BizHandler
    {
        public BizHandler()
        {
            PostResult = new PostResult { IsPosted = false, Message = string.Empty };
        }

        public PostResult PostResult { get; set; }

        string bizlinkString = String.Empty;
        public string SetBizLinkString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            bizlinkString = configuration.GetSection("Settings:BizLinkEndPoint").Value;

            return bizlinkString;
        }

        public bool PostToBiz(string fContent, string fileName, string from, string to)
        {
            string fileContent = fContent;

            string contentType = "text/xml";
            byte[] bytes = null;

            string config = SetBizLinkString();

            string uriBiz =
                string.Format("{0}?from={1}&to={3}&filename={2}.{1}.{3}",
                    config, from, fileName, to);

            HttpWebRequest webRequest = WebRequest.Create(new Uri(uriBiz)) as HttpWebRequest;
            webRequest.Proxy = WebRequest.DefaultWebProxy;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

            webRequest.ContentType = contentType;
            webRequest.Method = "POST";

            if (bytes == null)
            {
                bytes = Encoding.ASCII.GetBytes(fileContent);
            }
            webRequest.ContentLength = bytes.Length;

            try
            {
                Stream stream = webRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
                if (webResponse == null)
                {
                    PostResult.Message = "Web response return a null. Please try again or contact support";
                    PostResult.IsPosted = false;
                    return false;
                }

                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    PostResult.Message = streamReader.ReadToEnd().Trim();
                    PostResult.IsPosted = true;
                    return true;
                }
            }
            catch (Exception exception)
            {
                PostResult.Message = string.Format("Exception occured sending to Biz. {0}. {1}. {2}", fileName, exception.Message, exception.InnerException);
                PostResult.IsPosted = false;

                return false;
            }
        }

    }
}
