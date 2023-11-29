using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Models
{
    public class APIEndPoints
    {
        public string API_Id { get; set; }
        public string EndPoint { get; set; }
        public string TransactionType { get; set; }
        public string HttpVerb { get; set; }
    }
}
