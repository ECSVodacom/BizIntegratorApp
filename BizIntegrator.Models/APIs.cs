using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Models
{
    public class APIs
    {
        public string Name { get; set; }
        public string AccountKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string EndPointBase { get; set; }
        public string AuthenticationType { get; set; }
        public string UseAPIKey { get; set; }
        public string EanCode { get; set; }
        public string IsActive { get; set; }
    }
}
