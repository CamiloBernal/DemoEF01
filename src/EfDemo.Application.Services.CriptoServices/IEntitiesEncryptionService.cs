using System;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Application.Services.CriptoModels;

namespace EfDemo.Application.Services.CriptoServices
{
    public interface IEntitiesEncryptionService
    {
        void DecryptEntity<TEntity>(TEntity entity, string key, string iv, Action<TEntity, string, string> decryptCallBack) where TEntity : class, IEncryptedEntity;

        Task DecryptEntityAsync<TEntity>(TEntity entity, string key, string iv, Action<TEntity, string, string> decryptCallBack, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class, IEncryptedEntity;

        void EncryptEntity<TEntity>(TEntity entity, string key, string iv) where TEntity : class, IEncryptedEntity;

        Task EncryptEntityAsync<TEntity>(TEntity entity, string key, string iv, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class, IEncryptedEntity;
    }
}