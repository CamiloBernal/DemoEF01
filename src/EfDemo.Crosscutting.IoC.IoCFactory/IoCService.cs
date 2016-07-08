using System;
using System.Configuration;
using EfDemo.Application.Services.CriptoServices;
using EfDemo.Application.Services.CriptoServices.RSA;
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
            const string publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var defaultConstructor = new InjectionConstructor(connectionString);
            container.RegisterType<ICategoryRepository, CategoryRepository>(defaultConstructor);
            container.RegisterType<IGoalRepository, GoalRepository>(defaultConstructor);
            container.RegisterType<ICryptoProvider, RsaCryptoProvider>(new InjectionConstructor(1024));
            container.RegisterType<IEntitiesEncryptionService, EntitiesEncryptionService>(new InjectionConstructor( new ResolvedParameter<ICryptoProvider>()));
            container.RegisterType<IConfidentialUserInfoRepository, ConfidentialUserInfoRepository>(new InjectionConstructor(connectionString, new ResolvedParameter<IEntitiesEncryptionService>(), publicKey));
        }
    }
}