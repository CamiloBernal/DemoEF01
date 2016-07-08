using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;

namespace EfDemo.Core.Repositories
{
    public interface IConfidentialUserInfoRepository
    {
        Task RegisterConfidentialInfoAsync(ConfidentialUserInfo confidentialUserInfo, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<ConfidentialUserInfo>> GetUserConfidentialInfoAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}