using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JwsTokenGenerator
{
    public class JsonWebToken
    {
        private AsymmetricKeyPair privateServiceKey;

        public JsonWebToken(string account, DateTimeOffset expiration, AsymmetricKeyPair privateServiceKey)
        {
            this.Account = account ?? throw new ArgumentNullException(nameof(account));
            this.Expiration = expiration;
            this.privateServiceKey = privateServiceKey ?? throw new ArgumentNullException(nameof(privateServiceKey));
        }

        public string Account { get; }

        public DateTimeOffset Expiration { get; }

        public string Serialize()
        {
            var header = new JObject();
            header["typ"] = "JWT";
            header["alg"] = "RS256";

            var encodedHeader = this.Serialize(header);

            var claims = new JObject();
            claims["account"] = this.Account;
            claims["expiration"] = this.Expiration;

            var encodedClaims = this.Serialize(claims);

            var partialToken = $"{encodedHeader}.{encodedClaims}";
            var token = $"{partialToken}.{this.Sign(partialToken)}";

            return token;
        }

        private string Sign(string partialToken)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(this.privateServiceKey.ToRSAParameters());
                rsa.PersistKeyInCsp = false;
                return Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes(partialToken), "SHA256"));
            }
        }

        private string Serialize(JObject json)
        {
            using (var ms = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(ms, Encoding.UTF8))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        json.WriteTo(jsonWriter);

                        jsonWriter.Flush();
                        streamWriter.Flush();

                        // Remove BOM. Not required but more in line with other languagtes
                        return Convert.ToBase64String(ms.GetBuffer(), 3, (int)ms.Position - 3); // Exclude BOM
                    }
                }
            }
        }
    }
}
