using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace EfDemo.Data.Presentation.Web.DefaultSite.CodeBase
{
    public abstract class BaseController : Controller
    {
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

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