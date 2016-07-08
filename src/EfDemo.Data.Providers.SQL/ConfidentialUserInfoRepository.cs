using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;

namespace EfDemo.Data.Providers.SQL
{
    public class ConfidentialUserInfoRepository : IConfidentialUserInfoRepository
    {
        public Task<IEnumerable<ConfidentialUserInfo>> GetUserConfidentialInfoAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task RegisterConfidentialInfoAsync(ConfidentialUserInfo confidentialUserInfo, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}