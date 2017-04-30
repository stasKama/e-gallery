using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_gellary.Models;

namespace Web_gellary.Controllers
{
    public class GalleryController : Controller
    {
        [Authorize]
        public ActionResult Home(string id)
        {
            EGellaryEntities db = new EGellaryEntities();
            Users user = db.Users.FirstOrDefault(u => u.UserURL == id);
            user.Password = "";
            return View(user);
        }
    }
}