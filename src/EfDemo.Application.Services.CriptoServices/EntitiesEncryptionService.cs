using System;
using System.Linq;
using System.Reflection;
using EfDemo.Application.Services.CriptoModels;

namespace EfDemo.Application.Services.CriptoServices
{
    public class EntitiesEncryptionService : IEntitiesEncryptionService
    {
        private readonly ICryptoProvider _iCryptoProvider;

        public EntitiesEncryptionService(ICryptoProvider iCryptoProvider)
        {
            if (iCryptoProvider == null) throw new ArgumentNullException(nameof(iCryptoProvider));
            _iCryptoProvider = iCryptoProvider;
        }

        public void EncryptEntity<TEntity>(TEntity entity, string publicKey)
            where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties(BindingFlags.Public)
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));

            foreach (var property in encryptedProperties)
            {
                var value = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(value)) continue;
                var encryptedValue = _iCryptoProvider.EncryptData(value, publicKey);
                property.SetValue(entity, encryptedValue);
                if (string.IsNullOrEmpty(entity.DecryptionPrivateKey))
                {
                    entity.DecryptionPrivateKey = _iCryptoProvider.GeneratePrivateKey();
                }
            }
        }

        public void DecryptEntity<TEntity>(TEntity entity, string publicKey, Action<TEntity, string, string> decryptCallBack)
            where TEntity : class, IEncryptedEntity
        {
            var encryptedProperties = entity.GetType().GetProperties(BindingFlags.Public)
                    .Where(p => p.GetCustomAttributes(typeof(PropertyEncrypted), true).Any(a => p.PropertyType == typeof(string)));
            foreach (var property in encryptedProperties)
            {
                var encryptedValue = property.GetValue(entity) as string;
                if (string.IsNullOrEmpty(encryptedValue)) continue;
                var value = _iCryptoProvider.DecryptData(encryptedValue, publicKey, entity.DecryptionPrivateKey);
                decryptCallBack.Invoke(entity, property.Name, value);
            }
        }
    }
}