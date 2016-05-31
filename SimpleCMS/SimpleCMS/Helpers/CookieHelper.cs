using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimpleCMS
{
    public class CookieHelper
    {
        public static string GetCultureName(HttpRequest request)
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = request.UserLanguages != null && request.UserLanguages.Length > 0 ?
                        request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = General.Helpers.CultureHelper.Instance.GetImplementedCulture(cultureName); // This is safe
            return cultureName;
        }

        public static string GetCultureName(HttpRequestBase request)
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = request.UserLanguages != null && request.UserLanguages.Length > 0 ?
                        request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = General.Helpers.CultureHelper.Instance.GetImplementedCulture(cultureName); // This is safe
            return cultureName;
        }
    }
}
