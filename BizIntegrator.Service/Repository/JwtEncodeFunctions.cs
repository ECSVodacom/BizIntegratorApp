using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BizIntegrator.Service.Repository
{
    public static class JwtEncodeFunctions
    {
        public static async Task<string> EncodeTokenAsync(this TokenPayload tokenPayload, Func<Task<object>> privateKeyProviderAsync)
        {
            JwtDates jwtDates = new JwtDates(DateTime.UtcNow, tokenPayload.Exp);
            object payload = (object)new Dictionary<string, object>{
        {
          "iat",
          (object) jwtDates.IssuedAt
        },
        {
          "exp",
          (object) jwtDates.ExpiresAt
        },
        {
          "jti",
          (object) tokenPayload.Jti
        },
        {
          "api-key",
          (object) tokenPayload.ApiKey
        }
      };
            return JWT.Encode(payload, await privateKeyProviderAsync(), JwsAlgorithm.RS256);
        }
    }
}
