﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CloudMockApi.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityWebActivator.Start();

            //ElCamino - Added to create azure tables
            ApplicationUserManager.StartupAsync();
            //safe to remove after tables are created once.

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
