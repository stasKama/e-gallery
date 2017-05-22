using System;
using System.Collections.Generic;
using System.IO;
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
            return View();
        }

        [HttpPost]
        public JsonResult EditAvatar(string fileData, string expansion)
        {
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            System.IO.File.Delete(GetPathToImg(user.Avatar, "avatar"));
            ImageSave.Save(fileData, GetPathToImg(user.UserURL + "." + expansion, "avatar"));
            user.Avatar = Url.Content("~/Images/avatar/" + user.UserURL + "." + expansion);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditNick(string Nick)
        {
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            user.Nick = Nick;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private string GetPathToImg(string fileName, string directory)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Images", directory, fileName);
        }

        public ActionResult EditPassword()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(UpdatePassword model)
        {
            if (ModelState.IsValid)
            {
                EGalleryEntities db = new EGalleryEntities();
                var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
                if (user.Password == model.OldPassword)
                {
                    user.Password = model.NewPassword;
                    db.SaveChanges();
                    ViewBag.Message = "Edit Password";
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "You have entered the wrong password");
                }
            }
            return PartialView(); 
        }
    }
}