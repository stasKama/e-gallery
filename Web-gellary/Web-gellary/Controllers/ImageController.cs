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
        //#block, #accept
        [HttpPost]
        public JsonResult Accect(string UrlImage)
        {
            var NameImage = GetNameImage(UrlImage);
            var Image = ActImage(NameImage, "view");
            AnswerUser("Accect", Image);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Block(string UrlImage)
        {
            var NameImage = GetNameImage(UrlImage);
            var Image = ActImage(NameImage, "block");
            AnswerUser("Block", Image);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void AnswerUser(string Message, Images Image)
        {
            EGellaryEntities db = new EGellaryEntities();
            Answers answer = new Answers()
            {
                UserId = Image.UserId,
                ImageId = Image.Id
            };
            answer.Text = Image.Users.Nick + ", your image: " + Message + ".";
            db.SaveChanges();
        }

        private Images ActImage(string NameImage, string Action)
        {
            EGellaryEntities db = new EGellaryEntities();
            var Image = db.Images.FirstOrDefault(im => im.Name == NameImage);
            Image.Code = Action;
            db.SaveChanges();
            return Image;
        }

        private string GetNameImage(string UrlImage)
        {
            var PositionPoint = UrlImage.LastIndexOf(".");
            var PositionSlash = UrlImage.LastIndexOf("/");
            return UrlImage.Substring(PositionSlash + 1, PositionPoint);
        }
    }
}