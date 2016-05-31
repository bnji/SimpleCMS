using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleCMS
{
    public class SimpleCMSRouteConfig
    {
        public static void Setup(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            "PermaLinkRoute", //name of route
            "{*permalinkName}", //url - this pretty much catches everything
            new { controller = "Page", action = "PermaLink", permalinkName = UrlParameter.Optional },
            new { permalinkName = new PermaLinkRouteConstraint() });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SimpleCMS" }
            );

            //routes.MapRoute(
            //    name: "CmsRoute",
            //    url: "{*permalink}",
            //    defaults: new { controller = "Page", action = "Index" },
            //    namespaces: new[] { "SimpleCMS", "BootstrapControllers" },
            //    constraints: new { permalink = new CmsUrlConstraint() }
            //);
        }


        public class PermaLinkRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                RouteDirection routeDirection)
            {
                var permaRoute = values[parameterName] as string;
                if (string.IsNullOrEmpty(permaRoute))
                    return false;

                var db = new DAL.BootstrapContext();
                var routeFromDb = db.Permalinks.Where(x => x.Name == permaRoute).FirstOrDefault();
                if (routeFromDb != null)
                    return true;

                //if (permaRoute == "friendly-content-title")
                //    return true; //this indicates we should handle this route with our action specified
                //else if (permaRoute == "sida/2")
                //    return true; //this indicates we should handle this route with our action specified

                return false; //false means nope, this isn't a route we should handle
            }
        }

        //public class CmsUrlConstraint : IRouteConstraint
        //{
        //    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        //    {
        //        var db = new SimpleCMS.DAL.BootstrapContext();
        //        if (values[parameterName] != null)
        //        {
        //            var permalink = values[parameterName].ToString();
        //            return db.Pages.Any(p => p.Permalink != null && p.Permalink.Name == permalink);
        //        }
        //        return false;
        //    }
        //}
    }
}