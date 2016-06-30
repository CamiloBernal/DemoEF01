using System.Security.Claims;
using System.Threading.Tasks;
using EfDemo.Core.Model;
using Microsoft.AspNet.Identity;

namespace EfDemo.Application.Services.Security
{
    public class ApplicationUser : User, IUser<long>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}