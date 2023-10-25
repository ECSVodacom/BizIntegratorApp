using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BizInegrator.InvoiceManager
{
    public class InvoiceHandler
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

        public void GetJsonFile()
        {

        }

        public void GetAuth()
        {

        }

        //Send Json Invoice file from Biz with Auth request to Woerman API
        public void SendJsonFileToWoerman() 
        {
        
        }

        public void SendResponseToBiz() 
        { 
        
        }
    }
}
