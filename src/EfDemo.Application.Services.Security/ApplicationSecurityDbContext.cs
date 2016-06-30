using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;

namespace EfDemo.Application.Services.Security
{
    public class ApplicationSecurityDbContext : DbContext
    {
        private readonly SecurityModelConfig _securityModelConfig;

        public ApplicationSecurityDbContext(string nameOrConnectionString, Action<SecurityModelConfig> modelConfig = null)
            : base(nameOrConnectionString)
        {
            _securityModelConfig = new SecurityModelConfig();
            modelConfig?.Invoke(_securityModelConfig);
        }

        public bool RequireUniqueEmail { get; set; }

        public virtual IDbSet<Core.Model.Role> Roles { get; set; }

        public virtual IDbSet<ApplicationUser> Users { get; set; }

        public static ApplicationSecurityDbContext Create(string nameOrConnectionString, Action<SecurityModelConfig> modelConfig = null)
        {
            return new ApplicationSecurityDbContext(nameOrConnectionString, modelConfig);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
            base.OnModelCreating(modelBuilder);

            // Needed to ensure subclasses share the same table
            var user = modelBuilder.Entity<Core.Model.User>()
                .ToTable(_securityModelConfig.ApplicationUserTableName);
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(_securityModelConfig.UserNameIndex) { IsUnique = true }));

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<Core.Model.UserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable(_securityModelConfig.ApplicationUserRoleTableName);

            modelBuilder.Entity<Core.Model.UserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable(_securityModelConfig.ApplicationUserLoginTableName);

            modelBuilder.Entity<Core.Model.UserClaim>()
                .ToTable(_securityModelConfig.ApplicationUserClaimTableName);

            var role = modelBuilder.Entity<Core.Model.Role>()
                .ToTable(_securityModelConfig.ApplicationRoleTableName);
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(_securityModelConfig.RoleNameIndex) { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry,
            IDictionary<object, object> items)
        {
            if (entityEntry == null || entityEntry.State != EntityState.Added)
                return base.ValidateEntity(entityEntry, items);
            var errors = new List<DbValidationError>();
            var user = entityEntry.Entity as Core.Model.User;
            //check for uniqueness of user name and email
            if (user != null)
            {
                if (Users.Any(u => string.Equals(u.UserName, user.UserName)))
                {
                    errors.Add(new DbValidationError("User",
                        string.Format(CultureInfo.CurrentCulture, SecurityResources.DuplicateUserName, user.UserName)));
                }
                if (RequireUniqueEmail && Users.Any(u => string.Equals(u.Email, user.Email)))
                {
                    errors.Add(new DbValidationError("User",
                        string.Format(CultureInfo.CurrentCulture, SecurityResources.DuplicateEmail, user.Email)));
                }
            }
            else
            {
                var role = entityEntry.Entity as Core.Model.Role;
                //check for uniqueness of role name
                if (role != null && Roles.Any(r => string.Equals(r.Name, role.Name)))
                {
                    errors.Add(new DbValidationError("Role",
                        string.Format(CultureInfo.CurrentCulture, SecurityResources.RoleAlreadyExists, role.Name)));
                }
            }
            return errors.Any() ? new DbEntityValidationResult(entityEntry, errors) : base.ValidateEntity(entityEntry, items);
        }
    }
}