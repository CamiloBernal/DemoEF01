using System;
using System.Threading;
using System.Threading.Tasks;

namespace EfDemo.Application.Services.CriptoServices
{
    public interface ISymmetricCryptoProvider
    {
        string DecryptString(string encryptedData, string key, string iv);

        string DecryptString(string encryptedData, byte[] key, byte[] iv);

        Task<string> DecryptStringAsync(string encryptedData, string key, string iv, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> DecryptStringAsync(string encryptedData, byte[] key, byte[] iv, CancellationToken cancellationToken = default(CancellationToken));

        string EncryptString(string plainData, string key, string iv);

        string EncryptString(string plainData, byte[] key, byte[] iv);

        Task<string> EncryptStringAsync(string plainData, string key, string iv, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> EncryptStringAsync(string plainData, byte[] key, byte[] iv, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<byte[], byte[]> GenerateCryptoKeys();

        string GetByteToken(int size);
    }
}