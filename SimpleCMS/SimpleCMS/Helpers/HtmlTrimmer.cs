using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCMS.Html
{
    public static class HtmlTrimmer
    {
        public static string Trim(string input)
        {
            var cellHtml = input.Replace("\r\n", "").Replace("\0","").Trim();
            string noHTML = Regex.Replace(cellHtml, @"<[^>]+>|&nbsp;", "").Trim();
            string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");
            return noHTMLNormalised;
        }
    }
}
