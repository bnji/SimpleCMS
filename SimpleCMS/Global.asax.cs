using System.Web.Optimization;
using SimpleCMS.BLL;
using SimpleCMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AddGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Handle i18n decimal separators
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            CultureConfig.LoadCultures(new string[] { "en", "fo", "da" }, "~/SimpleCMS/Content/files/cultures.json");
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
            //CountryHandler.TryCreate(HttpContext.Current.Server.MapPath("~/Content/files/countries.json"));
            Settings.Instance.AppName = "Simple CMS for .NET Demo";
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;
            var xmlFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            xmlFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
            //var dcs = new DataContractSerializer(typeof(Website), null, int.MaxValue, false, /* preserveObjectReferences: */ true, null);
            //xmlFormatter.CreateDefaultSerializerSettings(<Website>(dcs);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SimpleCMS.CustomViewEngine());
            
        }

        private static readonly string ERROR_PAGE_LOCATION = "~/SimpleCMS/Errors/404.cshtml";
        private static readonly string NOT_FOUND_PAGE_LOCATION = "~/SimpleCMS/Errors/404.cshtml";

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception is HttpUnhandledException)
            {
                if (exception.InnerException == null)
                {
                    Server.Transfer(ERROR_PAGE_LOCATION, false);
                    return;
                }
                exception = exception.InnerException;
            }

            if (exception is HttpException)
            {
                if (((HttpException)exception).GetHttpCode() == 404)
                {

                    // Log if wished.
                    Server.ClearError();
                    HttpContext.Current.RewritePath(NOT_FOUND_PAGE_LOCATION);
                    //Server.Transfer(NOT_FOUND_PAGE_LOCATION, false);
                    return;
                }
            }

            if (Context != null && Context.IsCustomErrorEnabled)
                Server.Transfer(ERROR_PAGE_LOCATION, false);
            //else
            //    Log.Error("Unhandled Exception trapped in Global.asax", exception);
        }

        public static void AddGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Attributes.LoggingFilterAttribute());
        }
    }
}
