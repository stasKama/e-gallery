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
        [Authorize]
        public ActionResult UploadImage()
        {
            EGellaryEntities db = new EGellaryEntities();
            ViewModel model = new ViewModel()
            {
                User = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name)
            };
            return View(model);
        }

        public ActionResult AddImage(HttpPostedFileBase file)
        {
            return RedirectToAction("UploadImage");
        }
        
        public JsonResult AddImageAjax(string fileName, string fileData)
        {
            EGellaryEntities db = new EGellaryEntities();
            var serverPath = Server.MapPath("~");
            var point = fileName.LastIndexOf(".");
            var userId = Int32.Parse(User.Identity.Name);
            Users user = db.Users.FirstOrDefault(u => u.Id == userId);
            var permission = user.Permission > 0;
            if (permission)
            {
                Images image = new Images
                {
                    Status = (int)(user.Role == "User" ? Status.WAITING : Status.VIEW),
                    Expansion = fileName.Substring(point + 1).ToLower(),
                    UserId = userId
                };
                db.Images.Add(image);
                db.SaveChanges();
                var path = GetPathToImg(image.Name + "." + image.Expansion, user.Role == "User" ? "expectation" : "gallery");
                var dataIndex = fileData.IndexOf("base64", StringComparison.Ordinal) + 7;
                var cleareData = fileData.Substring(dataIndex);
                var fileInformation = Convert.FromBase64String(cleareData);
                var bytes = fileInformation.ToArray();
                var fileStream = System.IO.File.Create(path);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            return Json(permission, JsonRequestBehavior.AllowGet);
        }

        private string GetPathToImg(string fileName, string directory)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Images", directory, fileName);
        }

        [HttpPost]
        public JsonResult GetCountAnswer()
        {
            var UserId = Int32.Parse(User.Identity.Name);
            EGellaryEntities db = new EGellaryEntities();
            var countQuery = db.Answers.Where(an => an.UserId == UserId).Count();
            return Json(countQuery, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCountQuery()
        {
            EGellaryEntities db = new EGellaryEntities();
            var countQuery = db.Images.Where(im => im.Status == (int) Status.WAITING).Count();
            return Json(countQuery, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Accect(string UrlImage)
        {
            var Image = ActImage(UrlImage, Status.VIEW);
            AnswerUser("Accect", Image);
            MoveFile(Image.Name + "." + Image.Expansion, "gallery");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Block(string UrlImage)
        {
            var Image = ActImage(UrlImage, Status.BLOCK);
            AnswerUser("Block", Image);
            MoveFile(Image.Name + "." + Image.Expansion, "block");
            EGellaryEntities db = new EGellaryEntities();
            var user = db.Users.Find(Image.UserId);
            user.Permission--;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private void AnswerUser(string Message, Images Image)
        {
            EGellaryEntities db = new EGellaryEntities();
            Answers answer = new Answers()
            {
                UserId = Image.UserId,
                ImageId = Image.Id,
                Text = Image.Users.Nick + ", your image: " + Message + "."
            };
            db.Answers.Add(answer);
            db.SaveChanges();
        }

        private void MoveFile(string NameImage, string directory)
        {
            System.IO.File.Move(GetPathToImg(NameImage, "expectation"), GetPathToImg(NameImage, directory));
        }

        private Images ActImage(string NameImage, Status status)
        {
            EGellaryEntities db = new EGellaryEntities();
            var Image = db.Images.FirstOrDefault(im => im.Name == NameImage);
            Image.Status = (int) status;
            db.SaveChanges();
            return Image;
        }

        private string GetNameImage(string UrlImage)
        {
            var PositionPoint = UrlImage.LastIndexOf(".");
            var PositionSlash = UrlImage.LastIndexOf("/");
            return UrlImage.Substring(PositionSlash + 1, PositionPoint);
        }

        [HttpPost]
        public JsonResult GetQueryImages()
        {
            List<string> imagesUrl = new List<string>();
            using (EGellaryEntities db = new EGellaryEntities())
            {
                foreach (var img in db.Images.Where(im => im.Status == (int) Status.WAITING))
                {
                    imagesUrl.Add(Url.Content("~/Images/expectation/" + img.Name + "." + img.Expansion));
                }
            }
            imagesUrl.Reverse();
            return Json(imagesUrl, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Moderator")]
        public ActionResult Query()
        {
            EGellaryEntities db = new EGellaryEntities();
            ViewModel model = new ViewModel()
            {
                User = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name)
            };
            return View(model);
        }
    }
}