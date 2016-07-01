using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EfDemo.Core.Model;

namespace EfDemo.Data.Providers.SQL
{
    public class CategoryMappings : EntityTypeConfiguration<Category>
    {
        public CategoryMappings()
        {
            HasRequired<User>(c => c.CreatedBy)
                .WithMany(u => u.UserCategories)
                .HasForeignKey(u => u.CreatedById);            
        }
    }
}