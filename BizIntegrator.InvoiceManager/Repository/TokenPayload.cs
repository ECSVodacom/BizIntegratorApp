using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BizIntegrator.InvoiceManager.Repository
{
    public class TokenPayload
    {
        [JsonPropertyName("api-key")]
        [JsonProperty("api-key")]
        public string ApiKey { get; set; }

        public int Exp { get; set; }

        public string Jti { get; set; }
    }
}
