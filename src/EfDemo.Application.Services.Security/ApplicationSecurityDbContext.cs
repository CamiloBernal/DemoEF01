using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EfDemo.Application.Services.Security
{
    public class ApplicationSecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly SecurityModelConfig _securityModelConfig;
        public ApplicationSecurityDbContext(string nameOrConnectionString, Action<SecurityModelConfig> modelConfig = null)
            : base(nameOrConnectionString)
        {
            _securityModelConfig = new SecurityModelConfig();
            modelConfig?.Invoke(_securityModelConfig);
        }

        public static ApplicationSecurityDbContext Create(string nameOrConnectionString, Action<SecurityModelConfig> modelConfig = null)
        {
            return new ApplicationSecurityDbContext(nameOrConnectionString, modelConfig);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable(_securityModelConfig.IdentityUserTableName).Property(p => p.Id).HasColumnName(_securityModelConfig.UserIdFieldName);
            modelBuilder.Entity<ApplicationUser>().ToTable(_securityModelConfig.ApplicationUserTableName).Property(p => p.Id).HasColumnName(_securityModelConfig.UserIdFieldName);
            modelBuilder.Entity<IdentityUserRole>().ToTable(_securityModelConfig.IdentityUserRoleTableName);
            modelBuilder.Entity<IdentityUserLogin>().ToTable(_securityModelConfig.IdentityUserLoginTableName);
            modelBuilder.Entity<IdentityUserClaim>().ToTable(_securityModelConfig.IdentityUserClaimTableName);
            modelBuilder.Entity<IdentityRole>().ToTable(_securityModelConfig.IdentityRoleTableName);

        }
    }
}