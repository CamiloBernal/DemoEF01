using System.Web;
using System.Web.Mvc;
using EfDemo.Application.Services.Security;
using EfDemo.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace EfDemo.Data.Presentation.Web.DefaultSite.CodeBase
{
    public abstract class BaseController : Controller
    {
        public ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        protected User GetCurrentUser() => Request.IsAuthenticated ? UserManager.FindById(User.Identity.GetUserId<long>()) : null;

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}