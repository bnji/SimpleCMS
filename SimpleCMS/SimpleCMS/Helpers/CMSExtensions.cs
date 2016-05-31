using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleCMS
{
    public static class CMSExtensions
    {
        public static bool IsPreview
        {
            get
            {
                return HttpContext.Current.Request.Params["isPreview"] == "true";
            }
        }

        public static string PageTitle
        {
            get
            {
                var pageTitle = string.Empty;
                using (var db = new DAL.BootstrapContext())
                {
                    var pageSettings = db.BlogWebsiteSettings.FirstOrDefault();
                    if (pageSettings != null)
                    {
                        pageTitle = pageSettings.Name;
                    }
                }
                //if (!string.IsNullOrEmpty(@ViewBag.PageTitle))
                //{
                //    pageTitle += " - " + @ViewBag.PageTitle;
                //}
                return pageTitle;
            }
        }

        public static string Culture
        {
            get
            {
                return General.Helpers.CultureHelper.GetCurrentCulture();
            }
        }

        public static string CultureUILowerInvariant
        {
            get
            {
                return General.Helpers.CultureHelper.GetCurrentUICultureLowerInvariant();
            }
        }

        public static string ControllerName
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static MvcHtmlString DisplayDebugInfo()
        {
            var r = HttpContext.Current.Request;
            var text = "Mobile device? " + r.Browser.IsMobileDevice +
                       " Manufacturer: " + r.Browser.MobileDeviceManufacturer +
                       " Model: " + r.Browser.MobileDeviceModel +
                       " Culture: " + CMSExtensions.Culture;
            return MvcHtmlString.Create(text);
        }
    }
}