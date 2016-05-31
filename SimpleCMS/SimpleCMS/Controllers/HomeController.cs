using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleCMS.DAL;
using SimpleCMS.ViewModels;
using SimpleCMS.Models;
using Newtonsoft.Json;

namespace BootstrapControllers
{
    //[RequireHttps]
    [AllowAnonymous]
    public class HomeController : Controller<Home>
    {
        public ActionResult Apitester()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult BrowserInfo()
        {
            return View();
        }

        public ActionResult Mobile()
        {
            return View();
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = General.Helpers.CultureHelper.Instance.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies.Get("_culture");
            if (cookie != null)
            {
                //cookie.Expires = DateTime.UtcNow.AddYears(-1);
                cookie.Value = culture;   // update cookie value
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            var returnTo = Request.UrlReferrer;
            if (returnTo == null)
                return Redirect("/");
            return Redirect(returnTo.ToString());
        }

        protected override void AddViewBag(Home obj)
        {
        }

        public override ActionResult Index()
        {
            //var settings = db.BlogWebsiteSettings.FirstOrDefault();
            //if (settings != null && !string.IsNullOrEmpty(settings.DefaultFrontPage))
            //{
            //    return Redirect(settings.DefaultFrontPage);
            //}
            return View();
        }
    }
}