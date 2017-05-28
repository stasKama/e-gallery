using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_gellary.Filters;

namespace Web_gellary.Controllers
{
    [Language]
    public class ErrorController : Controller
    {
       public ActionResult NotFound()
        {
            return View();
        }
    }
}