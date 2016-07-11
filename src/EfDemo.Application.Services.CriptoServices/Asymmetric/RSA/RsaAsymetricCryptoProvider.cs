using System;
using System.Security.Cryptography;
using System.Text;

namespace EfDemo.Application.Services.CriptoServices.Asymmetric.RSA
{
    public class RsaAsymetricCryptoProvider : IAsymetricCryptoProvider
    {
        private readonly int _dwKeySize;

        public RsaAsymetricCryptoProvider(int dwKeySize = 1024)
        {
            _dwKeySize = dwKeySize;
        }

        public string GeneratePrivateKey(string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(_dwKeySize))
            {
                rsa.FromXmlString(publicKey);
                var keyInfo = rsa.ExportParameters(false);
                var privateKeyInfo = GeneratePrivateKey();
                privateKeyInfo.Modulus = keyInfo.Modulus;
                privateKeyInfo.Exponent = keyInfo.Exponent;
                return privateKeyInfo.ToXmlString();
            }
        }

        private RSAParameters GeneratePrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider(_dwKeySize))
            {
                var privateKey = rsa.ToXmlString(true);
                return rsa.ExportParameters(true);
            }
        }

        public string EncryptData(string data, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(_dwKeySize))
            {
                try
                {
                    var dataBase64 = Encoding.UTF8.GetBytes(data);
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(dataBase64, true);
                    return Convert.ToBase64String(encryptedData);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public string DecryptData(string encryptedData, string publicKey, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(_dwKeySize))
            {
                string decryptedData;
                try
                {
                    rsa.FromXmlString(publicKey);
                    rsa.FromXmlString(privateKey);
                    var resultBytes = Convert.FromBase64String(encryptedData);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
                return decryptedData;
            }
        }
    }
}