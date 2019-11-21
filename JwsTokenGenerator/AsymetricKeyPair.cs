using System;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace JwsTokenGenerator
{
    public class AsymmetricKeyPair
    {
        public AsymmetricKeyPair(
            string privateExponent,
            string exponentOne,
            string exponentTwo,
            string publicExponent,
            string coefficient,
            string modulus,
            string primeOne,
            string primeTwo)
        {
            this.PrivateExponent = privateExponent;
            this.ExponentOne = exponentOne;
            this.ExponentTwo = exponentTwo;
            this.PublicExponent = publicExponent;
            this.Coefficient = coefficient;
            this.Modulus = modulus;
            this.PrimeOne = primeOne;
            this.PrimeTwo = primeTwo;
        }

        public string PrivateExponent { get; }

        public string ExponentOne { get; }

        public string ExponentTwo { get; }

        public string PublicExponent { get; }

        public string Coefficient { get; }

        public string Modulus { get; }

        public string PrimeOne { get; }

        public string PrimeTwo { get; }

        public static AsymmetricKeyPair FromJson(JObject json)
        {
            var privateKeyChild = json["privateKey"];
            if (json == null)
            {
                throw new System.ArgumentNullException(nameof(json));
            }

            return new AsymmetricKeyPair(
                privateExponent: (string)privateKeyChild["privateExponent"],
                exponentOne: (string)privateKeyChild["exponentOne"],
                exponentTwo: (string)privateKeyChild["exponentTwo"],
                publicExponent: (string)privateKeyChild["publicExponent"],
                coefficient: (string)privateKeyChild["coefficient"],
                modulus: (string)privateKeyChild["modulus"],
                primeOne: (string)privateKeyChild["primeOne"],
                primeTwo: (string)privateKeyChild["primeTwo"]);
        }

        public RSAParameters ToRSAParameters()
        {
            return new RSAParameters()
            {
                D = Convert.FromBase64String(this.PrivateExponent),
                DP = Convert.FromBase64String(this.ExponentOne),
                DQ = Convert.FromBase64String(this.ExponentTwo),
                Exponent = Convert.FromBase64String(this.PublicExponent),
                InverseQ = Convert.FromBase64String(this.Coefficient),
                Modulus = Convert.FromBase64String(this.Modulus),
                P = Convert.FromBase64String(this.PrimeOne),
                Q = Convert.FromBase64String(this.PrimeTwo)
            };
        }
    }
}
