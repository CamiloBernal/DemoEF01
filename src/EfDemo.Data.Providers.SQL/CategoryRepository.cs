using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using EfDemo.Core.Model;
using EfDemo.Core.Repositories;

namespace EfDemo.Data.Providers.SQL
{
    public class CategoryRepository: ICategoryRepository
    {

        private readonly CategoriesDbContext _categoriesDbContext;

        public CategoryRepository(string connectionString)
        {
            _categoriesDbContext = new CategoriesDbContext(connectionString);
        }


        public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default(CancellationToken)) => await _categoriesDbContext.Categories.ToListAsync(cancellationToken).ConfigureAwait(false);

        public async Task<int> SaveCategoryAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
             _categoriesDbContext.Categories.Add(category);
            return await _categoriesDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}