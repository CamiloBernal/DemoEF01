using System.Web;
using EfDemo.Application.Services.Security;
using EfDemo.Crosscutting.IoC.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace EfDemo.Presentation.Web.DefaultSite.CodeBase
{
    public abstract class AccountControllerBase : BaseController, IExcludedIoCController
    {
        protected const string XsrfKey = "XsrfId";
        private ApplicationSignInManager _signInManager;

        protected AccountControllerBase()
        {
            //Default Ctor
        }

        protected AccountControllerBase(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}