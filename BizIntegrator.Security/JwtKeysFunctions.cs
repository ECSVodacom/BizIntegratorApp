using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Security
{
    public static class JwtKeysFunctions
    {
        private static string KeyFileName(Guid apiKey) => string.Format("{0}.xml", (object)apiKey);

        public static async Task<object> ReadKeyFromFileAsync(String PrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider cryptoServiceProvider = rsa;
            cryptoServiceProvider.FromXmlString(PrivateKey);
            object obj = (object)rsa;
            return obj;
        }

        public static async Task<object> ReadKeyAsync()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            object obj = (object)rsa;
            return obj;
        }
    }
}
