using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;

namespace EfDemo.Core.Repositories
{
    public interface IGoalRepository
    {

        Task<IEnumerable<Goal>> GetGoalsAsync(CancellationToken cancellation = default(CancellationToken));
        Task<int> InsertGoalAsync(Goal goal, CancellationToken cancellation = default(CancellationToken));

    }
}