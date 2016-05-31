using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS.Helpers
{
    public class ActionLinkData
    {
        public dynamic Model { get; set; }
        public string ControllerName { get; set; }
        public object RouteValues { get; set; }
        public object HtmlAttributes { get; set; }
    }
}