using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SimpleCMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // TODO: *** CORS SECURITY ISSUE - CHANGE IF NESSECARY! ***
            // http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            if (General.Helpers.Debug.IsDebugMode)
            {
                var cors = new EnableCorsAttribute("*", "*", "*");
                config.EnableCors(cors);
            }
            else
            {
                config.EnableCors();
            }

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional }
            );

            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            //{
            //    Formatting = Newtonsoft.Json.Formatting.Indented,
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //};

        }
    }
}
