using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;

namespace EfDemo.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveCategoryAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));
    }
}