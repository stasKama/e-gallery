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
            EGellaryEntities db = new EGellaryEntities();
            ViewModel model = new ViewModel()
            {
                User = db.Users.FirstOrDefault(u => u.UserURL == id)
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetImages()
        {
            var id = Int32.Parse(UserId);
            List<string> imagesUrl = new List<string>();
            using (EGellaryEntities db = new EGellaryEntities())
            {
                foreach (var img in db.Images.Where(im => im.UserId == id && im.Status == (int)Status.VIEW))
                {
                    imagesUrl.Add(Url.Content("~/Images/gallery/" + img.Name + "." + img.Expansion));
                }
            }
            imagesUrl.Reverse();
            return Json(imagesUrl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Users()
        {
            EGellaryEntities db = new EGellaryEntities();
            ViewModel model = new ViewModel()
            {
                User = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name),
                UsersList = db.Users.ToList()
            };
            return View(model);
        }

        public ActionResult History()
        {
            CountSkip = 0;
            EGellaryEntities db = new EGellaryEntities();
            ViewModel model = new ViewModel()
            {
                User = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name),
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetHistory()
        {
            EGellaryEntities db = new EGellaryEntities();
            var image = db.Images.Where(im => im.Status == (int)Status.VIEW).ToList();
            image.Reverse();
            List<string> imagesUrl = new List<string>();
            foreach (var img in image.Skip(CountSkip).Take(12))
            {
                imagesUrl.Add(Url.Content("~/Images/gallery/" + img.Name + "." + img.Expansion));
            }
            CountSkip += 12;
            return Json(imagesUrl, JsonRequestBehavior.AllowGet);
        }
    }
}