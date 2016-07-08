using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using EfDemo.Crosscutting.IoC.IoCFactory;
using EfDemo.Crosscutting.IoC.Mvc;

namespace EfDemo.Presentation.Web.DefaultSite
{
    public partial class Startup
    {
        private static void ConfigureIoC()
        {
            var container = IoCService.Container;
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new IoCFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new IoCDependencyResolver(container));            
        }
    }
}