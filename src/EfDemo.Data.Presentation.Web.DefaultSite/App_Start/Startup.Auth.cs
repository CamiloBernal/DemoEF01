using EfDemo.Data.Presentation.Web.DefaultSite.Extensions;
using Owin;

namespace EfDemo.Data.Presentation.Web.DefaultSite
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.ConfigureAppIdentityContext();
            app.ConfigureCookieAuthentication();
            app.ConfigureSignIn();
        }
    }
}