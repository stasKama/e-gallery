using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_gellary.Models
{
    public class EditLanguageUserPage
    {
        private static readonly List<string> cultures = new List<string>() { "en", "be", "fr" };

        public static HttpCookie EditLanguage(string lang, HttpCookie cookie)
        {
           
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }
            if (cookie != null)
            {
                cookie.Value = lang;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            return cookie;
        }
    }
}