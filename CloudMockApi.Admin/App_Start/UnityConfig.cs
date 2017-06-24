using System;
using CloudMockApi.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace CloudMockApi.Admin.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main lazyContainer.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> LazyContainer = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity lazyContainer.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return LazyContainer.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity lazyContainer.</summary>
        /// <param name="container">The unity lazyContainer to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // lazyContainer.LoadConfiguration();

            container.RegisterType<ITenantsRepository, TenantsRepository>();

            // TODO: Register your types here
            // lazyContainer.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}
