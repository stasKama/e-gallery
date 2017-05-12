using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_gellary.Models;

namespace Web_gellary.Controllers
{
    public class EditController : Controller
    {
        [Authorize]
        public ActionResult Edit()
        {
            EGellaryEntities db = new EGellaryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            user.Password = "";
            EditUserModel model = new EditUserModel()
            {
                User = user,
                UserPassword = new UpdatePassword()
            };
            return View(model);
        }
    }
}