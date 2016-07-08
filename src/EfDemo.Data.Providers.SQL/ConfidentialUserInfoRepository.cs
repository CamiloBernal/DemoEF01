using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Application.Services.CriptoServices;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;

namespace EfDemo.Data.Providers.SQL
{
    public class ConfidentialUserInfoRepository : IConfidentialUserInfoRepository
    {
        private readonly ConfidentialUserInfoDbContext _dbContext;

        public ConfidentialUserInfoRepository(string nameOrConnectionString, IEntitiesEncryptionService encryptionService, string encryptionPublicKey)
        {
            if (encryptionService == null) throw new ArgumentNullException(nameof(encryptionService));
            _dbContext = new ConfidentialUserInfoDbContext(nameOrConnectionString, encryptionService, encryptionPublicKey);
        }

        public async Task<IEnumerable<ConfidentialUserInfo>> GetUserConfidentialInfoAsync(CancellationToken cancellationToken = default(CancellationToken)) => await _dbContext.ConfidentialUserInfo.ToListAsync(cancellationToken).ConfigureAwait(false);

        public async Task RegisterConfidentialInfoAsync(ConfidentialUserInfo confidentialUserInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            _dbContext.ConfidentialUserInfo.Add(confidentialUserInfo);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}