using System;
using System.Configuration;
using EfDemo.Application.Services.CriptoServices;
using EfDemo.Application.Services.CriptoServices.Asymmetric.RSA;
using EfDemo.Application.Services.CriptoServices.Symmetric.Rijndael;
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
            const string sharedCryptoIv = "21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV";
            var defaultConstructor = new InjectionConstructor(connectionString);
            container.RegisterType<ICategoryRepository, CategoryRepository>(defaultConstructor);
            container.RegisterType<IGoalRepository, GoalRepository>(defaultConstructor);
            container.RegisterType<ISymmetricCryptoProvider, RijndaelCryptoProvider>();
            container.RegisterType<IEntitiesEncryptionService, EntitiesEncryptionService>(new InjectionConstructor( new ResolvedParameter<ISymmetricCryptoProvider>()));
            container.RegisterType<IConfidentialUserInfoRepository, ConfidentialUserInfoRepository>(new InjectionConstructor(connectionString, new ResolvedParameter<IEntitiesEncryptionService>(), sharedCryptoIv));
        }
    }
}