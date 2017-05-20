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
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            user.Password = "";
            EditUserModel model = new EditUserModel()
            {
                User = user,
                UserPassword = new UpdatePassword()
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult EditAvatar(string fileData)
        {
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            var serverPath = Server.MapPath("~");
            var path = Path.Combine(serverPath, "Images", "avatar", user.UserURL + ".jpg");
            var dataIndex = fileData.IndexOf("base64", StringComparison.Ordinal) + 7;
            var cleareData = fileData.Substring(dataIndex);
            var fileInformation = Convert.FromBase64String(cleareData);
            var bytes = fileInformation.ToArray();
            var fileStream = System.IO.File.Create(path);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            user.Avatar = Url.Content("~/Images/avatar/" + user.UserURL + ".jpg");
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditPassword()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditNick()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}