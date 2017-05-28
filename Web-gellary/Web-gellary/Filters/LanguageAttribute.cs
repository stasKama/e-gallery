using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web_gellary.Filters
{
    public class LanguageAttribute : FilterAttribute, IActionFilter
    {
        public static readonly List<string> cultures = new List<string>() { "en", "be", "fr" };

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string cultureName = null;

            HttpCookie cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];

            cultureName = cultureCookie != null ? cultureCookie.Value : "en";

            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
        }
    }
}