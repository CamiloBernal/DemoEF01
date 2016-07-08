using System;
using EfDemo.Application.Services.CriptoModels;

namespace EfDemo.Application.Services.CriptoServices
{
    public interface IEntitiesEncryptionService
    {
        void DecryptEntity<TEntity>(TEntity entity, string publicKey, Action<TEntity, string, string> decryptCallBack) where TEntity : class, IEncryptedEntity;
        void EncryptEntity<TEntity>(TEntity entity, string publicKey) where TEntity : class, IEncryptedEntity;
    }
}