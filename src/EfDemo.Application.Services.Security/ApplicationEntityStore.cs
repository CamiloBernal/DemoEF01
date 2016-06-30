using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EfDemo.Application.Services.Security
{
    internal class ApplicationEntityStore<TEntity> where TEntity : class
    {
        public ApplicationEntityStore(DbContext context)
        {
            Context = context;
            DbEntitySet = context.Set<TEntity>();
        }

        public DbContext Context { get; private set; }

        public DbSet<TEntity> DbEntitySet { get; private set; }

        public IQueryable<TEntity> EntitySet => DbEntitySet;

        public void Create(TEntity entity)
        {
            DbEntitySet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            DbEntitySet.Remove(entity);
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return DbEntitySet.FindAsync(id);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}