using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace EfDemo.Application.Services.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            //Default Ctor
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context, Action<ApplicationUserManager> configuration, string purposes)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationSecurityDbContext>()));
            configuration.Invoke(manager);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create(purposes));
            }
            return manager;
        }
    }
}