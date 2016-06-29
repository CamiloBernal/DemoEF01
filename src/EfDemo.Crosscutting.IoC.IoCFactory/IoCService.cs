using System;
using System.Configuration;
using EfDemo.Core.Repositories;
using EfDemo.Data.Providers.SQL;
using Microsoft.Practices.Unity;

namespace EfDemo.Crosscutting.IoC.IoCFactory
{
    public static class IoCService
    {
        private static readonly Lazy<IUnityContainer> InternalContainer = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            return container;
        });

        public static IUnityContainer Container => InternalContainer.Value;

        static IoCService()
        {
            RegisterTypes(InternalContainer.Value);
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString;
            var defaultConstructor = new InjectionConstructor(connectionString);
            container.RegisterType<ICategoryRepository, CategoryRepository>(defaultConstructor);
            container.RegisterType<IGoalRepository, GoalRepository>(defaultConstructor);
        }
    }
}