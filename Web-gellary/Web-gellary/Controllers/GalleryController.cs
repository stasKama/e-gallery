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
        private static string UserId;
        private static int CountSkip;

        public ActionResult Home(string id)
        {
            UserId = id;
            EGalleryEntities db = new EGalleryEntities();
            var User = db.Users.FirstOrDefault(u => u.UserURL == id);
            return View(User);
        }

        [HttpPost]
        public JsonResult GetImages()
        {
            var id = Int32.Parse(UserId);
            List<string> imagesUrl = new List<string>();
            using (EGalleryEntities db = new EGalleryEntities())
            {
                foreach (var img in db.Images.Where(im => im.UserId == id))
                {
                    imagesUrl.Add(Url.Content("~/Images/gallery/" + img.Name + "." + img.Expansion));
                }
            }
            imagesUrl.Reverse();
            return Json(imagesUrl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            EGalleryEntities db = new EGalleryEntities();
            List<UserViewModel> UsersModel = new List<UserViewModel>();
            foreach (var user in db.Users)
            {
                UsersModel.Add(new UserViewModel()
                {
                    Url = user.UserURL,
                    Avatar = user.Avatar,
                    Nick = user.Nick,
                    Status = user.State,
                    CountUploadImages = user.Images.Count()
                });
            }
            return Json(UsersModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Filter(string Nick, bool Online, bool Avatar, bool Popular)
        {
            EGalleryEntities db = new EGalleryEntities();
            List<UserViewModel> UsersModel = new List<UserViewModel>();
            IQueryable<Users> users = db.Users;
            if (Nick != null || Nick != "")
            {
                users = db.Users.Where(u => u.Nick.Contains(Nick));
            }
            if (Online)
            {
                users = db.Users.Where(u => u.State == "online");
            }
            if (Avatar)
            {
                users = db.Users.Where(u => u.Avatar != "http://www.teniteatr.ru/assets/no_avatar-e557002f44d175333089815809cf49ce.png");
            }
            if (Popular)
            {
                users = users.OrderByDescending(u => u.Images.Sum(im => im.LikesToImages.Count) / u.Images.Count);
            }
            foreach (var user in users)
            {
                UsersModel.Add(new UserViewModel()
                {
                    Url = user.UserURL,
                    Avatar = user.Avatar,
                    Nick = user.Nick,
                    Status = user.State,
                    CountUploadImages = user.Images.Count()
                });
            }
            return Json(UsersModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult History()
        {
            CountSkip = 0;
            return View();
        }

        [HttpPost]
        public JsonResult GetHistory()
        {
            EGalleryEntities db = new EGalleryEntities();
            var image = db.Images.ToList();
            image.Reverse();
            List<string> imagesUrl = new List<string>();
            foreach (var img in image.Skip(CountSkip).Take(12))
            {
                imagesUrl.Add(Url.Content("~/Images/gallery/" + img.Name + "." + img.Expansion));
            }
            CountSkip += 12;
            return Json(imagesUrl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetStatusUser(string Status)
        {
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            user.Status = Status;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCountAnswer()
        {
            var UserId = Int32.Parse(User.Identity.Name);
            EGalleryEntities db = new EGalleryEntities();
            return Json(db.Answers.Where(an => an.UserId == UserId).Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCountQuery()
        {
            EGalleryEntities db = new EGalleryEntities();
            return Json(db.PicturesWaiting.Where(im => im.Status == (int)Status.WAITING).Count(),
                JsonRequestBehavior.AllowGet);
        }
    }
}