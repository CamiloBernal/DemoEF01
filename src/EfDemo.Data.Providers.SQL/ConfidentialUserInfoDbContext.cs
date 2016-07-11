using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Application.Services.CriptoModels;
using EfDemo.Application.Services.CriptoServices;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class ConfidentialUserInfoDbContext : DbContext
    {
        private readonly string _encryptionIv;
        private readonly IEntitiesEncryptionService _encryptionService;

        public ConfidentialUserInfoDbContext(string nameOrConnectionString, IEntitiesEncryptionService encryptionService, string encryptionIv)
            : base(nameOrConnectionString)
        {
            if (encryptionService == null) throw new ArgumentNullException(nameof(encryptionService));
            _encryptionService = encryptionService;
            _encryptionIv = encryptionIv;
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ObjectMaterialized;
        }

        public IDbSet<ConfidentialUserInfo> ConfidentialUserInfo { get; set; }

        public override int SaveChanges()
        {
            var contextAdapter = ((IObjectContextAdapter)this);
            contextAdapter.ObjectContext.DetectChanges();
            var pendingEntities = GetPendingEntities(contextAdapter);
            var result = default(int);
            if (pendingEntities == null) return result;
            var objectStateEntries = pendingEntities as IList<ObjectStateEntry> ?? pendingEntities.ToList();
            foreach (var entry in objectStateEntries)
            {
                _encryptionService.EncryptEntity((IEncryptedEntity)entry.Entity, "", _encryptionIv);
            }
            result = base.SaveChanges();
            foreach (var entry in objectStateEntries)
            {
                _encryptionService.DecryptEntity((IEncryptedEntity)entry.Entity, ((IEncryptedEntity)entry.Entity).DecryptionPrivateKey, _encryptionIv, DecryptEntityCallBack);
            }
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var contextAdapter = ((IObjectContextAdapter)this);
            contextAdapter.ObjectContext.DetectChanges();

            var pendingEntities = GetPendingEntities(contextAdapter);
            var result = default(int);
            if (pendingEntities == null) return result;
            var objectStateEntries = pendingEntities as IList<ObjectStateEntry> ?? pendingEntities.ToList();
            foreach (var entry in objectStateEntries)
            {
                await _encryptionService.EncryptEntityAsync((IEncryptedEntity)entry.Entity, "", _encryptionIv, cancellationToken).ConfigureAwait(false);
            }
            result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            foreach (var entry in objectStateEntries)
            {
                await _encryptionService.DecryptEntityAsync((IEncryptedEntity)entry.Entity, ((IEncryptedEntity)entry.Entity).DecryptionPrivateKey, _encryptionIv, DecryptEntityCallBack, cancellationToken).ConfigureAwait(false);
            }
            return result;
        }

        private static IEnumerable<ObjectStateEntry> GetPendingEntities(IObjectContextAdapter contextAdapter) =>
                            contextAdapter?.ObjectContext?.ObjectStateManager?
                                                            .GetObjectStateEntries(EntityState.Added | EntityState.Modified)?
                                                            .Where(en => !en.IsRelationship)
                                                            .Where(e => e.Entity is IEncryptedEntity)
                                                            .ToList();

        private void DecryptEntityCallBack(IEncryptedEntity encryptedEntity, string propertyName, string propertyValue)
        {
            this.Entry(encryptedEntity).Property(propertyName).OriginalValue = propertyValue;
            this.Entry(encryptedEntity).Property(propertyName).IsModified = false;
        }

        private void ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            _encryptionService.DecryptEntity((IEncryptedEntity)e.Entity,
                ((IEncryptedEntity)e.Entity).DecryptionPrivateKey, _encryptionIv, DecryptEntityCallBack);
        }
    }
}