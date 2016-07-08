using System;
using System.Security.Cryptography;
using System.Text;

namespace EfDemo.Application.Services.CriptoServices.RSA
{
    public class RsaCryptoProvider : ICryptoProvider
    {
        private readonly int _dwKeySize;

        public RsaCryptoProvider(int dwKeySize = 1024)
        {
            _dwKeySize = dwKeySize;
        }

        public string GeneratePrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider(_dwKeySize))
            {
                return rsa.ToXmlString(true);
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