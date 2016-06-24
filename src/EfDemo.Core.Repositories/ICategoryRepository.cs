using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;

namespace EfDemo.Core.Repositories
{
    public interface ICategoryRepository
    {

        Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellation = default(CancellationToken));
        Task<int> SaveCategoriesAsync(Category category, CancellationToken cancellation = default(CancellationToken));




    }
}
