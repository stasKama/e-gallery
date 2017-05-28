using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Web_gellary.Filters;
using Web_gellary.Models;

namespace Web_gellary.Controllers
{
    [Language]
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                            new { controller = "Gallery", action = "Home", id = User.Identity.Name }));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            using (EGalleryEntities db = new EGalleryEntities())
            {
                Users user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    user.State = "online";
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.UserURL, false);
                    Response.Cookies.Add(EditLanguageUserPage.EditLanguage(user.CodeLanguage, Request.Cookies["lang"]));
                    return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                        new { controller = "Gallery", action = "Home", id = user.UserURL }));
                }
                else
                {
                    ModelState.Clear(); 
                    ModelState.AddModelError("", Resources.Resource.NoSuchUser);
                }
            }
            return View();
        }

        public ActionResult Registration()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                            new { controller = "Gallery", action = "Home", id = User.Identity.Name }));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(AccountModel account)
        {
            if (ModelState.IsValid)
            {
                Users user = null;
                using (EGalleryEntities db = new EGalleryEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == account.Email);
                    if (user == null)
                    {
                        user = new Users() { Email = account.Email, Password = account.Password, Nick = account.Username };
                        db.Users.Add(user);
                        db.SaveChanges();
                        FormsAuthentication.SetAuthCookie(user.UserURL, false);
                        ModelState.Clear();
                        return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                            new { controller = "Gallery", action = "Home", id = user.UserURL }));
                    }
                    else
                    {
                        ModelState.AddModelError("Email", Resources.Resource.EmailExist);
                    }
                }

            }
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            EGalleryEntities db = new EGalleryEntities();
            var user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            user.State = "offline";
            db.SaveChanges();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}