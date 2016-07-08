namespace EfDemo.Application.Services.CriptoServices
{
    public interface ICryptoProvider
    {
        string DecryptData(string encryptedData, string publicKey, string privateKey);

        string EncryptData(string data, string publicKey);

        string GeneratePrivateKey();
    }
}