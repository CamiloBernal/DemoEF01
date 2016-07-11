using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EfDemo.Application.Services.CriptoServices.Symmetric.Rijndael
{
    public class RijndaelCryptoProvider : ISymmetricCryptoProvider
    {
        public string DecryptString(string encryptedData, byte[] key, byte[] iv)
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedData);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                rijndael.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }

        public string DecryptString(string encryptedData, string key, string iv)
        {
            var cryptBytes = GetCryptBytes(key, iv);
            return DecryptString(encryptedData, cryptBytes.Item1, cryptBytes.Item2);
        }

        public async Task<string> DecryptStringAsync(string encryptedData, byte[] key, byte[] iv, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedData);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                var decryptedByteCount = await cryptoStream.ReadAsync(plainTextBytes, 0, plainTextBytes.Length, cancellationToken).ConfigureAwait(false);
                memoryStream.Close();
                cryptoStream.Close();
                rijndael.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }
        public async Task<string> DecryptStringAsync(string encryptedData, string key, string iv, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cryptBytes = GetCryptBytes(key, iv);
            return await DecryptStringAsync(encryptedData, cryptBytes.Item1, cryptBytes.Item2, cancellationToken).ConfigureAwait(false);
        }

        public string EncryptString(string plainData, byte[] key, byte[] iv)
        {
            byte[] cipherMessageBytes;
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var plainDataInBytes = Encoding.UTF8.GetBytes(plainData);
                cryptoStream.Write(plainDataInBytes, 0, plainDataInBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherMessageBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                rijndael.Clear();
            }
            return Convert.ToBase64String(cipherMessageBytes);
        }

        public string EncryptString(string plainData, string key, string iv)
        {
            var cryptBytes = GetCryptBytes(key, iv);
            return EncryptString(plainData, cryptBytes.Item1, cryptBytes.Item2);
        }

        public async Task<string> EncryptStringAsync(string plainData, byte[] key, byte[] iv, CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] cipherMessageBytes;
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var plainDataInBytes = Encoding.UTF8.GetBytes(plainData);
                await cryptoStream.WriteAsync(plainDataInBytes, 0, plainDataInBytes.Length, cancellationToken).ConfigureAwait(false);
                cryptoStream.FlushFinalBlock();
                cipherMessageBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                rijndael.Clear();
            }
            return Convert.ToBase64String(cipherMessageBytes);
        }
        public async Task<string> EncryptStringAsync(string plainData, string key, string iv, CancellationToken cancellationToken = default(CancellationToken))
        {
            var cryptBytes = GetCryptBytes(key, iv);
            return await EncryptStringAsync(plainData, cryptBytes.Item1, cryptBytes.Item2, cancellationToken).ConfigureAwait(false);
        }

        public Tuple<byte[], byte[]> GenerateCryptoKeys()
        {
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            {
                var keys = Tuple.Create(rijndael.Key, rijndael.IV);
                rijndael.Clear();
                return keys;
            }
        }

        public string GetByteToken(int size)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[size];
                rng.GetBytes(tokenData);
                var token = Convert.ToBase64String(tokenData);
                return token;
            }
        }

        private static Tuple<byte[], byte[]> GetCryptBytes(string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            const int keySize = 32;
            const int ivSize = 16;
            Array.Resize(ref keyBytes, keySize);
            Array.Resize(ref ivBytes, ivSize);
            return Tuple.Create(keyBytes, ivBytes);
        }
    }
}