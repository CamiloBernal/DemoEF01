using System.Threading;
using EfDemo.Crosscutting.IoC.IoCFactory;
using EfDemo.Presentation.Web.DefaultSite;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
[assembly: System.Web.PreApplicationStartMethod(typeof(Startup), "Start")]

namespace EfDemo.Presentation.Web.DefaultSite
{
    public partial class Startup
    {
        public static void Start()
        {
            ConfigureIoC();
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var context = new OwinContext(app.Properties);
            var token = context.Get<CancellationToken>("host.OnAppDisposing");
            if (token != CancellationToken.None)
            {
                token.Register(() =>
                {
                    //App shutdown code
                    var iocContainer = IoCService.Container;
                    iocContainer?.Dispose();
                });
            }
        }
    }
}