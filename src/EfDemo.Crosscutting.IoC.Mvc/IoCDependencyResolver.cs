using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace EfDemo.Crosscutting.IoC.Mvc
{
    public class IoCDependencyResolver : IDependencyResolver
    {
        private const string HttpContextKey = "perRequestContainer";

        private readonly IUnityContainer _container;

        public IoCDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (!typeof(IController).IsAssignableFrom(serviceType))
                return IsRegistered(serviceType) ? ChildContainer.Resolve(serviceType) : null;
            try
            {
                return ChildContainer.Resolve(serviceType);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (IsRegistered(serviceType))
            {
                yield return ChildContainer.Resolve(serviceType);
            }

            foreach (var service in ChildContainer.ResolveAll(serviceType))
            {
                yield return service;
            }
        }

        protected IUnityContainer ChildContainer
        {
            get
            {
                var childContainer = HttpContext.Current.Items[HttpContextKey] as IUnityContainer;

                if (childContainer == null)
                {
                    HttpContext.Current.Items[HttpContextKey] = childContainer = _container.CreateChildContainer();
                }

                return childContainer;
            }
        }

        public static void DisposeOfChildContainer()
        {
            var childContainer = HttpContext.Current.Items[HttpContextKey] as IUnityContainer;

            childContainer?.Dispose();
        }

        private bool IsRegistered(Type typeToCheck)
        {
            if (!typeToCheck.IsInterface && !typeToCheck.IsAbstract) return true;
            var isRegistered = ChildContainer.IsRegistered(typeToCheck);

            if (isRegistered || !typeToCheck.IsGenericType) return isRegistered;
            var openGenericType = typeToCheck.GetGenericTypeDefinition();

            isRegistered = ChildContainer.IsRegistered(openGenericType);

            return isRegistered;
        }
    }
}