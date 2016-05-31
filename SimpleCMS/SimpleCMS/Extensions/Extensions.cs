using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace System
{
    public static class Extensions
    {
        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        //http://stackoverflow.com/questions/3275242/how-do-you-remove-invalid-characters-when-creating-a-friendly-url-ie-how-do-you
        public static string Slugify(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Replace("æ", "ae").Replace("ø", "oe");
        }

        public static string JsonSerialize(this HtmlHelper htmlHelper, object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        public static string NameWithoutExtension(this FileInfo fi)
        {
            return Path.GetFileNameWithoutExtension(fi.FullName);
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public static Expression<Func<T, S>> CreatePropSelectorExpression<T, S>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.PropertyOrField(parameter, propertyName);
            return Expression.Lambda<Func<T, S>>(body, parameter);
        }

        public static bool Contains2(this object obj, object compareTo)
        {
            if (compareTo == null)
                return false;
            return obj.ToString().ToUpper().Contains(compareTo.ToString().ToUpper());
        }

        public static void TryCatch(Action executeAction, Action<Exception> handleException, Action handleFinally = null)
        {
            try
            {
                executeAction();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                if (handleFinally != null)
                    handleFinally();
            }
        }

        public static string ToUpperFirst(this string s)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            result += char.ToUpper(s[0]);
            result += s.Length > 1 ? s.Substring(1).ToLower().Trim() : string.Empty;
            return result;
        }

        public static string ToUpperEachFirst(this string s)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            var strs = Regex.Replace(s, @"\s+", " ").Split(' ');
            foreach (var item in strs)
            {
                result += char.ToUpper(item[0]);
                result += item.Length > 1 ? item.Substring(1).ToLower().Trim() + " " : " ";
            }
            //return char.ToUpper(s[0]) + s.Substring(1).ToLower();
            return result.Trim();
        }

        public static MvcHtmlString ExportToExcelButton(this HtmlHelper helper, string text)
        {
            return MvcHtmlString.Create(string.Format("<button class='k-button k-button-icontext k-grid-excel'><span class='k-icon k-i-excel'></span> {0}</button>", text));
        }

        public static MvcHtmlString ActionLinkExternal(this HtmlHelper helper, Uri URI, string target = "_self")
        {
            return helper.ActionLinkExternal(URI != null ? URI.ToString() : "", URI != null ? URI.ToString() : "", target, null);
        }

        public static MvcHtmlString ActionLinkExternal(this HtmlHelper helper, Uri URI, string label, string target = "_self")
        {
            return helper.ActionLinkExternal(URI != null ? URI.ToString() : "", label, target, null);
        }

        public static MvcHtmlString ActionLinkExternal(this HtmlHelper helper, Uri URI, Uri label, string target = "_self", string prefix = null)
        {
            return helper.ActionLinkExternal(URI != null ? URI.ToString() : "", label != null ? label.ToString() : "", target, prefix);
        }

        public static MvcHtmlString ActionLinkExternal(this HtmlHelper helper, string URI, string label, string target = "_self", string prefix = null)
        {
            if (!string.IsNullOrEmpty(URI))
            {
                URI = (!string.IsNullOrEmpty(prefix) && !URI.StartsWith(prefix)) ? URI : prefix + URI;
                return MvcHtmlString.Create(string.Format("<a target={2} href={0}>{1}</a>", URI, label, target));
            }
            return MvcHtmlString.Create("-");
        }

        public static String ToShortDateTimeString(this DateTime dateTime, string separator = " ", bool isDateShowBeforeTime = true)
        {
            if (isDateShowBeforeTime)
                return dateTime.ToShortDateString() + separator + dateTime.ToShortTimeString();

            return dateTime.ToShortTimeString() + separator + dateTime.ToShortDateString();
        }

        public static string AppendRandomNumberString(this string str)
        {
            return AppendRandomNumber(str).ToString();
        }

        public static int AppendRandomNumber(this string str)
        {
            return new Random((int)DateTime.Now.Ticks).Next(0, int.MaxValue);
        }

        public static string RequestQueryString(this HttpRequestBase request)
        {
            var _requestQueryString = request.QueryString.ToString();
            return _requestQueryString.Trim().Length > 0 ? "?" + _requestQueryString : _requestQueryString;
        }

        public static string GetParamAsQuery(this HttpRequestBase request, string param)
        {
            return param + request.QueryString[param];
        }

        public static string GetControllerName(this ViewContext viewContext)
        {
            return viewContext.RouteData.Values["controller"].ToString();
        }

        public static string GetHtmlAttributesAsString(this object htmlAttributes)
        {
            var htmlAttributesString = string.Empty;
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
            {
                htmlAttributesString += string.Format("{0}=\"{1}\" ", property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
            }
            return htmlAttributesString;
        }

        public static string TrimIncludingMiddle(this string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value.Trim(), @"\s+", " ");
        }

        public static decimal AsPercentage(this int value, int maxValue, int decimalCount)
        {
            var divideBy = (decimal)(maxValue - 1);
            if (divideBy > 0)
            {
                return Math.Round(((value * 100.0m) / divideBy), 2);
            }
            return Math.Round(((value * 100.0m) / 1), 2);
        }
    }
}
