using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Application.Services.CriptoModels;

namespace EfDemo.Application.Services.CriptoServices
{
    public class EntitiesEncryptionService : IEntitiesEncryptionService
    {
        private readonly ISymmetricCryptoProvider _cryptoProvider;

        public EntitiesEncryptionService(ISymmetricCryptoProvider cryptoProvider)
        {
            if (cryptoProvider == null) throw new ArgumentNullException(nameof(cryptoProvider));
            _cryptoProvider = cryptoProvider;
        }

        public void DecryptEntity<TEntity>(TEntity entity, string key, string iv, Action<TEntity, string, string> decryptCallBack) where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));
            foreach (var property in encryptedProperties)
            {
                var encryptedValue = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(encryptedValue)) continue;
                var value = _cryptoProvider.DecryptString(encryptedValue, entity.DecryptionPrivateKey, iv);
                decryptCallBack.Invoke(entity, property.Name, value);
            }
        }

        public async Task DecryptEntityAsync<TEntity>(TEntity entity, string key, string iv, Action<TEntity, string, string> decryptCallBack,
            CancellationToken cancellationToken = new CancellationToken()) where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));
            foreach (var property in encryptedProperties)
            {
                var encryptedValue = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(encryptedValue)) continue;
                var value = await _cryptoProvider.DecryptStringAsync(encryptedValue, entity.DecryptionPrivateKey, iv, cancellationToken).ConfigureAwait(false);
                decryptCallBack.Invoke(entity, property.Name, value);
            }
        }

        public void EncryptEntity<TEntity>(TEntity entity, string key, string iv) where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));

            foreach (var property in encryptedProperties)
            {
                var value = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(value)) continue;
                if (string.IsNullOrEmpty(key))
                {
                    key = _cryptoProvider.GetByteToken(32);
                }
                var encryptedValue = _cryptoProvider.EncryptString(value, key, iv);
                property.SetValue(entity, encryptedValue);
                entity.DecryptionPrivateKey = key;
            }
        }

        public async Task EncryptEntityAsync<TEntity>(TEntity entity, string key, string iv,
            CancellationToken cancellationToken = new CancellationToken()) where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties()
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));

            foreach (var property in encryptedProperties)
            {
                var value = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(value)) continue;
                if (string.IsNullOrEmpty(key))
                {
                    key = _cryptoProvider.GetByteToken(32);
                }
                var encryptedValue = await _cryptoProvider.EncryptStringAsync(value, key, iv, cancellationToken).ConfigureAwait(false);
                property.SetValue(entity, encryptedValue);
                entity.DecryptionPrivateKey = key;
            }
        }
    }
}