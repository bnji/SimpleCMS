using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleCMS.SimpleCMS
{
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
        {
            var viewLocations = new[] {  
                // Standard Views:
                "~/Views/{1}/{0}.aspx",  
                "~/Views/{1}/{0}.ascx",  
                "~/Views/{1}/{0}.cshtml",  
                "~/Views/{1}/{0}.vbhtml",  
                "~/Views/Shared/{0}.aspx",  
                "~/Views/Shared/{0}.ascx",  
                "~/Views/Shared/{0}.cshtml",  
                "~/Views/Shared/{0}.vbhtml",  
                // Custom Views: 
                "~/SimpleCMS/Views/{1}/{0}.aspx",  
                "~/SimpleCMS/Views/{1}/{0}.ascx",  
                "~/SimpleCMS/Views/{1}/{0}.cshtml",  
                "~/SimpleCMS/Views/{1}/{0}.vbhtml",  
                "~/SimpleCMS/Views/Shared/{0}.aspx",  
                "~/SimpleCMS/Views/Shared/{0}.ascx",  
                "~/SimpleCMS/Views/Shared/{0}.cshtml",  
                "~/SimpleCMS/Views/Shared/{0}.vbhtml",  
            };
            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}