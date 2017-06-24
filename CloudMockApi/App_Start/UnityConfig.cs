using Microsoft.Practices.Unity;
using System.Web.Http;
using CloudMockApi.Services;
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
            
            container.RegisterType<ITenantsRepository, TenantsRepository>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}