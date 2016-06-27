using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;

namespace EfDemo.Data.Providers.SQL
{
    public class GoalRepository: IGoalRepository
    {
        private readonly GoalsDbContext _goalsDbContext;
        public GoalRepository(string connectionString)
        {
            _goalsDbContext = new GoalsDbContext(connectionString);
        }

        public async Task<IEnumerable<Goal>> GetGoalsAsync(
            CancellationToken cancellationToken = default(CancellationToken))
            => await _goalsDbContext.Goals.ToListAsync(cancellationToken).ConfigureAwait(false);
        

        public async Task<int> InsertGoalAsync(Goal goal, CancellationToken cancellationToken = default(CancellationToken))
        {
            _goalsDbContext.Goals.Add(goal);
            return await _goalsDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}