using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace EfDemo.Crosscutting.IoC.Mvc
{
    public class IoCDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public IoCDependencyResolver(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IExcludedIoCController).IsAssignableFrom(serviceType)) return null;
            if (typeof(IController).IsAssignableFrom(serviceType)) return _container.Resolve(serviceType);
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }
    }
}