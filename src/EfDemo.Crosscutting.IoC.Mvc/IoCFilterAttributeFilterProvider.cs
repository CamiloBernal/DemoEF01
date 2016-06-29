using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace EfDemo.Crosscutting.IoC.Mvc
{
    public class IoCFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IUnityContainer _container;

        public IoCFilterAttributeFilterProvider(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var list = base.GetActionAttributes(controllerContext, actionDescriptor);
            var filterAttributes = list as IList<FilterAttribute> ?? list.ToList();
            if (list == null || !filterAttributes.Any()) return filterAttributes;
            foreach (var item in filterAttributes)
            {
                _container.BuildUp(item.GetType(), item);
            }
            return filterAttributes;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var list = base.GetControllerAttributes(controllerContext, actionDescriptor);
            var controllerAttributes = list as IList<FilterAttribute> ?? list.ToList();
            if (list == null || !controllerAttributes.Any()) return controllerAttributes;
            foreach (var item in controllerAttributes)
            {
                _container.BuildUp(item.GetType(), item);
            }
            return controllerAttributes;
        }
    }
}