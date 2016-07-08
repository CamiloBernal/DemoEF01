using System.Web.Mvc;
using EfDemo.Crosscutting.IoC.IoCFactory;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace EfDemo.Presentation.Web.DefaultSite
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            IoCService.RegisterTypes(container);

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}