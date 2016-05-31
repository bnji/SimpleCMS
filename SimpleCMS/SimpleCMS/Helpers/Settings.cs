using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public class Settings : ISettings
    {
        #region Singleton
        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        private Settings() {
            AppName = "DocFinder";
            AppRegInfo = "© " + DateTime.Now.Year + " - " + AppName;
            DateTimeFormatReporting = "ddMMyyyy";
        }
        #endregion

        public string AppName { get; set; }

        public string AppRegInfo { get; set; }

        public string DateTimeFormatReporting { get; set; }
    }
}