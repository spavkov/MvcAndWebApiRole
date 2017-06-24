using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CloudMockApi.RoutingHelpers;

namespace CloudMockApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Wildcard",
                routeTemplate: "{*uri}",
                constraints: new { controller = new ListRouteConstraint(ListConstraintType.Exclude, "admin") },
                defaults: new { controller = "wildcard", action = "catchall", uri = RouteParameter.Optional });
        }
    }
}
