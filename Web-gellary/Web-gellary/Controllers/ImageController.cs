using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_gellary.Models;

namespace Web_gellary.Controllers
{
    public class ImageController : Controller
    {
        [Authorize(Roles = "User")]
        public ActionResult UploadImage()
        {
            EGellaryEntities db = new EGellaryEntities();
            Users user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            return View(user);
        }

        public ActionResult AddImage(HttpPostedFileBase file)
        {
            return RedirectToAction("UploadImage");
        }
        
        public JsonResult AddImageAjax(string fileName, string fileData)
        {
            var serverPath = Server.MapPath("~");
            var point = fileName.LastIndexOf(".");
            var userId = Int32.Parse(User.Identity.Name);
            Images image = new Images
            {
                Expansion = fileName.Substring(point + 1).ToLower(),
                UserId = userId
            };
            using (EGellaryEntities db = new EGellaryEntities())
            {
                db.Images.Add(image);
                db.SaveChanges();
            }
            var path = GetPathToImg(image.Name + "." + image.Expansion, "expectation");
            var dataIndex = fileData.IndexOf("base64", StringComparison.Ordinal) + 7;
            var cleareData = fileData.Substring(dataIndex);
            var fileInformation = Convert.FromBase64String(cleareData);
            var bytes = fileInformation.ToArray();
            var fileStream = System.IO.File.Create(path);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private string GetPathToImg(string fileName, string directory)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Images", directory, fileName);
        }
    }
}