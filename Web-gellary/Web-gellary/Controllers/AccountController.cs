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
                    Response.Cookies.Add(EditLanguageUserPage.EditLanguage(user.CodeLanguage, Request.Cookies["lang"]));
                    if (user.Verification.Count == 0)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserURL, false);
                        return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                            new { controller = "Gallery", action = "Home", id = user.UserURL }));
                    }
                    else
                    {
                        Session["UserUrl"] = user.UserURL;
                        return View("Verification");
                    }
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
                        Session["UserUrl"] = user.UserURL;
                        ModelState.Clear();
                        SendEmail.SendVerificationCode(user.Email, user.Nick, user.Id);
                        return View("Verification");
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
            return View("Login");
        }

        public ActionResult Verification()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Gallery", new RouteValueDictionary(
                    new { controller = "Gallery", action = "Home", id = User.Identity.Name }));
            }
            if (Session["UserUrl"] == null)
            {
                return View("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Verification(string VerificationCode)
        {
            EGalleryEntities db = new EGalleryEntities();
            int id = Int32.Parse(Session["UserUrl"].ToString());
            Verification verification = db.Verification.FirstOrDefault(v => v.UserId == id);
            if (verification.VerificationCode == VerificationCode)
            {
                db.Verification.Remove(verification);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(Session["UserUrl"].ToString(), false);
                Session.Remove("UserUrl");
                return View("Greeting");
            }
            else
            {
                if ((--verification.NumberAttempts) == 0)
                {
                    Users User = db.Users.FirstOrDefault(u => u.Id == id);
                    db.Users.Remove(User);
                    db.SaveChanges();
                    return View("Login");
                }
                db.SaveChanges();
                ModelState.AddModelError("VerificationCode", Resources.Resource.VerificationCode);
                return View();
            }
        }

        [Authorize]
        public ActionResult Greeting()
        {
            return View();
        }
    }
}