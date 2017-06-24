using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CloudMockApi.RoutingHelpers
{
    public enum ListConstraintType
    {
        Exclude,
        Include
    }

    public class ListRouteConstraint : IRouteConstraint
    {
        public ListConstraintType ListType { get; set; }
        public IList<string> List { get; set; }

        public ListRouteConstraint() : this(ListConstraintType.Include) { }
        public ListRouteConstraint(params string[] list) : this(ListConstraintType.Include, list) { }
        public ListRouteConstraint(ListConstraintType listType, params string[] list)
        {
            if (list == null) throw new ArgumentNullException("list");

            this.ListType = listType;
            this.List = new List<string>();
            foreach (var item in list)
            {
                this.List.Add(item.ToLowerInvariant());
            }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (this.ListType == ListConstraintType.Exclude)
            {
                foreach (var exclusion in this.List)
                {
                    if (httpContext.Request.Path.ToLowerInvariant().StartsWith($"/{exclusion}"))
                    {
                        return false;
                    }
                }
                return true;
            }

            foreach (var inclusion in this.List)
            {
                if (httpContext.Request.Path.ToLowerInvariant().StartsWith($"/{inclusion}"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}