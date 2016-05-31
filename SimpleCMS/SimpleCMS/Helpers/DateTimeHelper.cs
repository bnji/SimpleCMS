using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// http://stackoverflow.com/questions/179940/convert-utc-gmt-time-to-local-time
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="dateTimeKind"></param>
        /// <returns></returns>
        public static DateTime? ParseForReport(string dateTimeString, string dateTimeFormat = "dd-MM-yyyy", DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            DateTime result = DateTime.Now;
            if(DateTime.TryParseExact(dateTimeString, dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result))
            {
                return DateTime.SpecifyKind(result, dateTimeKind).ToLocalTime();
            }
            return null;
        }
    }
}
