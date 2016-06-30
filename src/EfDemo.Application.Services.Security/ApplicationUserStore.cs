using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using EfDemo.Core.Model;
using Microsoft.AspNet.Identity;

namespace EfDemo.Application.Services.Security
{
    internal class ApplicationUserStore :
        IUserLoginStore<ApplicationUser, long>,
        IUserClaimStore<ApplicationUser, long>,
        IUserRoleStore<ApplicationUser, long>,
        IUserPasswordStore<ApplicationUser, long>,
        IUserSecurityStampStore<ApplicationUser, long>,
        IQueryableUserStore<ApplicationUser, long>,
        IUserEmailStore<ApplicationUser, long>,
        IUserPhoneNumberStore<ApplicationUser, long>,
        IUserTwoFactorStore<ApplicationUser, long>,
        IUserLockoutStore<ApplicationUser, long>
    {
        private readonly IDbSet<UserLogin> _logins;
        private readonly ApplicationEntityStore<Role> _roleStore;
        private readonly IDbSet<UserClaim> _userClaims;
        private readonly IDbSet<UserRole> _userRoles;
        private bool _disposed;
        private ApplicationEntityStore<ApplicationUser> _userStore;

        public ApplicationUserStore(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Context = context;
            AutoSaveChanges = true;
            _userStore = new ApplicationEntityStore<ApplicationUser>(context);
            _roleStore = new ApplicationEntityStore<Role>(context);
            _logins = Context.Set<UserLogin>();
            _userClaims = Context.Set<UserClaim>();
            _userRoles = Context.Set<UserRole>();
        }

        public bool AutoSaveChanges { get; set; }
        public DbContext Context { get; private set; }

        public bool DisposeContext { get; set; }
        public IQueryable<ApplicationUser> Users => _userStore.EntitySet;

        public virtual Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (claim == null) throw new ArgumentNullException(nameof(claim));

            _userClaims.Add(new UserClaim { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            return Task.FromResult(0);
        }

        public virtual Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            _logins.Add(new UserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            });
            return Task.FromResult(0);
        }

        public virtual async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentException(SecurityResources.ValueCannotBeNullOrEmpty, nameof(roleName));
            var roleEntity = await _roleStore.DbEntitySet.SingleOrDefaultAsync(r => string.Equals(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase)).ConfigureAwait(false);
            if (roleEntity == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, SecurityResources.RoleNotFound, roleName));
            }
            var ur = new UserRole { UserId = user.Id, RoleId = roleEntity.Id };
            _userRoles.Add(ur);
        }

        public virtual async Task CreateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _userStore.Create(user);
            await SaveChanges().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _userStore.Delete(user);
            await SaveChanges().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (login == null) throw new ArgumentNullException(nameof(login));
            var provider = login.LoginProvider;
            var key = login.ProviderKey;
            var userLogin =
                await _logins.FirstOrDefaultAsync(l => l.LoginProvider == provider && l.ProviderKey == key).ConfigureAwait(false);
            if (userLogin == null) return null;
            var userId = userLogin.UserId;
            return await GetUserAggregateAsync(u => u.Id.Equals(userId)).ConfigureAwait(false);
        }

        public virtual Task<ApplicationUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper());
        }

        public virtual Task<ApplicationUser> FindByIdAsync(long userId)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.Id.Equals(userId));
        }

        public virtual Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            return GetUserAggregateAsync(u => u.UserName.ToUpper() == userName.ToUpper());
        }

        public virtual Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            await EnsureClaimsLoaded(user).ConfigureAwait(false);
            return user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public virtual Task<string> GetEmailAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Email);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            await EnsureLoginsLoaded(user).ConfigureAwait(false);
            return user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList();
        }

        public virtual Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var userId = user.Id;
            var query = from userRole in _userRoles
                        where userRole.UserId.Equals(userId)
                        join role in _roleStore.DbEntitySet on userRole.RoleId equals role.Id
                        select role.Name;
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public virtual Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public virtual Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentException(SecurityResources.ValueCannotBeNullOrEmpty, nameof(roleName));

            var role = await _roleStore.DbEntitySet.SingleOrDefaultAsync(r => string.Equals(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase)).ConfigureAwait(false);
            if (role == null) return false;
            var userId = user.Id;
            var roleId = role.Id;
            return await _userRoles.AnyAsync(ur => ur.RoleId.Equals(roleId) && ur.UserId.Equals(userId)).ConfigureAwait(false);
        }

        public virtual async Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (claim == null) throw new ArgumentNullException(nameof(claim));

            IEnumerable<UserClaim> claims;
            var claimValue = claim.Value;
            var claimType = claim.Type;
            if (AreClaimsLoaded(user))
            {
                claims = user.Claims.Where(uc => uc.ClaimValue == claimValue && uc.ClaimType == claimType).ToList();
            }
            else
            {
                var userId = user.Id;
                claims = await _userClaims.Where(uc => uc.ClaimValue == claimValue && uc.ClaimType == claimType && uc.UserId.Equals(userId)).ToListAsync().ConfigureAwait(false);
            }
            foreach (var c in claims)
            {
                _userClaims.Remove(c);
            }
        }

        public virtual async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(roleName)) throw new ArgumentException(SecurityResources.ValueCannotBeNullOrEmpty, nameof(roleName));

            var roleEntity = await _roleStore.DbEntitySet.SingleOrDefaultAsync(r => string.Equals(r.Name, roleName, StringComparison.CurrentCultureIgnoreCase)).ConfigureAwait(false);
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userId = user.Id;
                var userRole = await _userRoles.FirstOrDefaultAsync(r => roleId.Equals(r.RoleId) && r.UserId.Equals(userId)).ConfigureAwait(false);
                if (userRole != null)
                {
                    _userRoles.Remove(userRole);
                }
            }
        }

        public virtual async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (login == null) throw new ArgumentNullException(nameof(login));

            UserLogin entry;
            var provider = login.LoginProvider;
            var key = login.ProviderKey;
            if (AreLoginsLoaded(user))
            {
                entry = user.Logins.SingleOrDefault(ul => ul.LoginProvider == provider && ul.ProviderKey == key);
            }
            else
            {
                var userId = user.Id;
                entry = await _logins.SingleOrDefaultAsync(ul => ul.LoginProvider == provider && ul.ProviderKey == key && ul.UserId.Equals(userId)).ConfigureAwait(false);
            }
            if (entry != null)
            {
                _logins.Remove(entry);
            }
        }

        public virtual Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public virtual Task SetEmailAsync(ApplicationUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.Email = email;
            return Task.FromResult(0);
        }

        public virtual Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public virtual Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public virtual Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public virtual Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public virtual async Task UpdateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _userStore.Update(user);
            await SaveChanges().ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (DisposeContext && disposing)
            {
                Context?.Dispose();
            }
            _disposed = true;
            Context = null;
            _userStore = null;
        }

        protected virtual async Task<ApplicationUser> GetUserAggregateAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            long id;
            ApplicationUser user;
            if (FindByIdFilterParser.TryMatchAndGetId(filter, out id))
            {
                user = await _userStore.GetByIdAsync(id).ConfigureAwait(false);
            }
            else
            {
                user = await Users.FirstOrDefaultAsync(filter).ConfigureAwait(false);
            }
            if (user == null) return null;
            await EnsureClaimsLoaded(user).ConfigureAwait(false);
            await EnsureLoginsLoaded(user).ConfigureAwait(false);
            await EnsureRolesLoaded(user).ConfigureAwait(false);
            return user;
        }

        private bool AreClaimsLoaded(ApplicationUser user)
        {
            return Context.Entry(user).Collection(u => u.Claims).IsLoaded;
        }

        private bool AreLoginsLoaded(ApplicationUser user)
        {
            return Context.Entry(user).Collection(u => u.Logins).IsLoaded;
        }

        private async Task EnsureClaimsLoaded(ApplicationUser user)
        {
            if (!AreClaimsLoaded(user))
            {
                var userId = user.Id;
                await _userClaims.Where(uc => uc.UserId.Equals(userId)).LoadAsync().ConfigureAwait(false);
                Context.Entry(user).Collection(u => u.Claims).IsLoaded = true;
            }
        }

        private async Task EnsureLoginsLoaded(ApplicationUser user)
        {
            if (!AreLoginsLoaded(user))
            {
                var userId = user.Id;
                await _logins.Where(uc => uc.UserId.Equals(userId)).LoadAsync().ConfigureAwait(false);
                Context.Entry(user).Collection(u => u.Logins).IsLoaded = true;
            }
        }

        private async Task EnsureRolesLoaded(ApplicationUser user)
        {
            if (!Context.Entry(user).Collection(u => u.Roles).IsLoaded)
            {
                var userId = user.Id;
                await _userRoles.Where(uc => uc.UserId.Equals(userId)).LoadAsync().ConfigureAwait(false);
                Context.Entry(user).Collection(u => u.Roles).IsLoaded = true;
            }
        }

        private async Task SaveChanges()
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private static class FindByIdFilterParser
        {
            // expression pattern we need to match
            private static readonly Expression<Func<ApplicationUser, bool>> Predicate = u => u.Id.Equals(default(long));

            // method we need to match: Object.Equals()
            private static readonly MethodInfo EqualsMethodInfo = ((MethodCallExpression)Predicate.Body).Method;

            // property access we need to match: User.Id
            private static readonly MemberInfo UserIdMemberInfo = ((MemberExpression)((MethodCallExpression)Predicate.Body).Object).Member;

            internal static bool TryMatchAndGetId(Expression<Func<ApplicationUser, bool>> filter, out long id)
            {
                // default value in case we can’t obtain it
                id = default(long);

                // lambda body should be a call
                if (filter.Body.NodeType != ExpressionType.Call)
                {
                    return false;
                }

                // actually a call to object.Equals(object)
                var callExpression = (MethodCallExpression)filter.Body;
                if (callExpression.Method != EqualsMethodInfo)
                {
                    return false;
                }
                // left side of Equals() should be an access to User.Id
                if (callExpression.Object == null
                    || callExpression.Object.NodeType != ExpressionType.MemberAccess
                    || ((MemberExpression)callExpression.Object).Member != UserIdMemberInfo)
                {
                    return false;
                }

                // There should be only one argument for Equals()
                if (callExpression.Arguments.Count != 1)
                {
                    return false;
                }

                MemberExpression fieldAccess;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (callExpression.Arguments[0].NodeType)
                {
                    case ExpressionType.Convert:
                        // convert node should have an member access access node
                        // This is for cases when primary key is a value type
                        var convert = (UnaryExpression)callExpression.Arguments[0];
                        if (convert.Operand.NodeType != ExpressionType.MemberAccess)
                        {
                            return false;
                        }
                        fieldAccess = (MemberExpression)convert.Operand;
                        break;

                    case ExpressionType.MemberAccess:
                        // Get field member for when key is reference type
                        fieldAccess = (MemberExpression)callExpression.Arguments[0];
                        break;

                    default:
                        return false;
                }

                // and member access should be a field access to a variable captured in a closure
                if (fieldAccess.Member.MemberType != MemberTypes.Field
                    || fieldAccess.Expression.NodeType != ExpressionType.Constant)
                {
                    return false;
                }

                // expression tree matched so we can now just get the value of the id
                var fieldInfo = (FieldInfo)fieldAccess.Member;
                var closure = ((ConstantExpression)fieldAccess.Expression).Value;

                id = (long)fieldInfo.GetValue(closure);
                return true;
            }
        }
    }
}