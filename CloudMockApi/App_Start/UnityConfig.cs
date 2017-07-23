using Microsoft.Practices.Unity;
using System.Web.Http;
using CloudMockApi.Library.Configuration;
using CloudMockApi.Library.Services.Configuration;
using CloudMockApi.Library.Services.Storage;
using Unity.WebApi;

namespace CloudMockApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<ITenantsRepository, TenantsRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IConfigurationHelper, ConfigurationHelper>();
            container.RegisterType<ICloudMockApiStorageConfiguration, CloudMockApiStorageConfiguration>(new ContainerControlledLifetimeManager());
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}