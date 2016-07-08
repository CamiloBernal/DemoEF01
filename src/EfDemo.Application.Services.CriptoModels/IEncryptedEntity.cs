namespace EfDemo.Application.Services.CriptoModels
{
    public interface IEncryptedEntity
    {
        string DecryptionPrivateKey { get; set; }
    }
}