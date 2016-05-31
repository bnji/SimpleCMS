using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SimpleCMS
{
    public class CultureConfig
    {
        public static void LoadCultures(string[] cultures, string path)
        {
            General.Helpers.CultureHelper.Instance.ClearCultures();
            JsonSerializer serializer = new JsonSerializer();
            var cultureList = serializer.Deserialize<Dictionary<string, General.Helpers.CultureInfo>>(new JsonTextReader(new StreamReader(HttpContext.Current.Server.MapPath(path))));
            foreach (var item in cultures)
            {
                General.Helpers.CultureHelper.Instance.AddCulture(cultureList[item]);
            }
        }
    }
}