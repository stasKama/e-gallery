using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_gellary.Filters;
using Web_gellary.Models;

namespace Web_gellary.Controllers
{
    [Language]
    public class ImageController : Controller
    {
        [Authorize]
        public ActionResult UploadImage()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult AddImageAjax(string expansion, string fileData)
        {
            EGalleryEntities db = new EGalleryEntities();
            Users user = db.Users.FirstOrDefault(u => u.UserURL == User.Identity.Name);
            var permission = user.Permission > 0;
            if (permission)
            {
                if (User.IsInRole("Moderator"))
                {
                    Images image = new Images
                    {
                        Expansion = expansion.ToLower(),
                        UserId = user.Id
                    };
                    db.Images.Add(image);
                    db.SaveChanges();
                    ImageSave.Save(fileData, GetPathToImg(image.Name + "." + image.Expansion, "gallery"));
                }
                else
                {
                    PicturesWaiting image = new PicturesWaiting
                    {
                        Expansion = expansion.ToLower(),
                        UserId = user.Id,
                        Status = (int) Status.WAITING
                    };
                    db.PicturesWaiting.Add(image);
                    db.SaveChanges();
                    ImageSave.Save(fileData, GetPathToImg(image.Name + "." + image.Expansion, "expectation"));
                }
            }
            return Json(permission, JsonRequestBehavior.AllowGet);
        }

        private string GetPathToImg(string fileName, string directory)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Images", directory, fileName);
        }

        [HttpPost]
        public JsonResult GetCountImages()
        {
            EGalleryEntities db = new EGalleryEntities();
            return Json(db.Images.Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Accect(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Picture = GetPicture(UrlImage, Status.VIEW, "Accect");
            var Image = new Images()
            {
                Expansion = Picture.Expansion,
                DateUpload = Picture.DateUpload,
                UserId = Picture.UserId
            };
            db.Images.Add(Image);
            db.SaveChanges();
            var url = Picture.Name + "." + Picture.Expansion;
            System.IO.File.Copy(GetPathToImg(url, "expectation"), GetPathToImg(url, "view"));
            MoveFile(url, Image.Name + "." + Image.Expansion, "expectation", "gallery");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Block(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Picture = GetPicture(UrlImage, Status.BLOCK, "Block");
            MoveFile(Picture.Name + "." + Picture.Expansion, Picture.Name + "." + Picture.Expansion, "expectation", "block");
            var user = db.Users.Find(Picture.UserId);
            user.Permission--;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private PicturesWaiting GetPicture(string UrlImage, Status status, string Message)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Picture = db.PicturesWaiting.FirstOrDefault(im => im.Name == UrlImage);
            Picture.Status = (int)status;
            db.SaveChanges();
            AnswerUser(Message, Picture.Id, Picture.UserId);
            return Picture;
        }

        private void AnswerUser(string Message, int ImageId, int UserId)
        {
            EGalleryEntities db = new EGalleryEntities();
            Users user = db.Users.Find(UserId);
            Answers answer = new Answers()
            {
                UserId = user.Id,
                PictureId = ImageId, 
                Text = user.Nick + ", your image: " + Message + "."
            };
            db.Answers.Add(answer);
            db.SaveChanges();
        }

        private void MoveFile(string OldNameImage, string NewNameImage, string OldDirectory, string NewDirectory)
        {
            System.IO.File.Move(GetPathToImg(OldNameImage, OldDirectory), GetPathToImg(NewNameImage, NewDirectory));
        }

        [HttpPost]
        public JsonResult GetQueryImages()
        {
            List<string> imagesUrl = new List<string>();
            using (EGalleryEntities db = new EGalleryEntities())
            {
                foreach (var img in db.PicturesWaiting.Where(im => im.Status == (int) Status.WAITING))
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
            return View();
        }

        [HttpPost]
        public JsonResult GetInformationImage(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.Images.FirstOrDefault(img => img.Name == UrlImage);
            var Author = db.Users.Find(Image.UserId);
            Image.CountView++;
            db.SaveChanges();
            var informationImage = new ImageInformation()
            {
                CountLike = db.LikesToImages.Where(li => li.ImageId == Image.Id).Count(),
                CountView = Image.CountView,
                UploadData = Convert.ToString(Image.DateUpload),
                AuthorName = Author.Nick,
                UrlUser = Author.UserURL,
                Avatar = Author.Avatar
            };
            return Json(informationImage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetComments(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.Images.FirstOrDefault(img => img.Name == UrlImage);
            List<Comment> Comments = null;
            if (User.Identity.IsAuthenticated)
            {
                Comments = new List<Comment>();
                foreach (var comment in db.CommentsToImages.Where(ci => ci.ImageId == Image.Id))
                {
                    var user = db.Users.Find(comment.UserId);
                    Comments.Add(new Comment()
                    {
                        TextComment = comment.Comment,
                        DataComment = Convert.ToString(comment.DateComment),
                        UrlUser = user.UserURL,
                        Avatar = user.Avatar,
                        AuthorName = user.Nick
                    });
                }
            }
            return Json(Comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetComment(string UrlImage, string Comment)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.Images.FirstOrDefault(img => img.Name == UrlImage);
            var UserId = Int32.Parse(User.Identity.Name);
            var CommentImage = new CommentsToImages()
            {
                Comment = Comment,
                UserId = UserId,
                ImageId = Image.Id
            };
            db.CommentsToImages.Add(CommentImage);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PutImageLikes(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.Images.FirstOrDefault(img => img.Name == UrlImage);
            var UserId = Int32.Parse(User.Identity.Name);
            var UserLike = db.LikesToImages.FirstOrDefault(li => li.ImageId == Image.Id && li.UserId == UserId);
            if (UserLike == null)
            {
                db.LikesToImages.Add(new LikesToImages() { UserId = UserId, ImageId = Image.Id });
            }
            else
            {
                db.LikesToImages.Remove(UserLike);
            }
            db.SaveChanges();
            return Json(db.Images.FirstOrDefault(img => img.Id == Image.Id).LikesToImages.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteImage(string UrlImage)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.Images.FirstOrDefault(img => img.Name == UrlImage);
            if (User.IsInRole("Moderator") && User.Identity.Name != Image.Users.UserURL)
            {
                var Picture = new PicturesWaiting()
                {
                    Expansion = Image.Expansion,
                    DateUpload = Image.DateUpload,
                    UserId = Image.UserId,
                    Status = (int)Status.BLOCK
                };
                db.PicturesWaiting.Add(Picture);
                db.SaveChanges();
                MoveFile(Image.Name + "." + Image.Expansion, Picture.Name + "." + Picture.Expansion, "gallery", "block");
                AnswerUser("Delete", Picture.Id, Picture.UserId);
            } else
            {
                System.IO.File.Delete(GetPathToImg(Image.Name + "." + Image.Expansion, "gallery"));
            }
            db.Images.Remove(Image);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "User")]
        public ActionResult Answers()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAnswers()
        {
            List<AnswersModel> answers = new List<AnswersModel>();
            EGalleryEntities db = new EGalleryEntities();
            var UserId = Int32.Parse(User.Identity.Name);
            foreach (var answer in db.Answers.Where(ans => ans.UserId == UserId))
            {
                var directory = answer.PicturesWaiting.Status == (int)Status.VIEW ? "view" : "block";
                var url = Url.Content("~/Images/" + directory + "/" + answer.PicturesWaiting.Name + "." + answer.PicturesWaiting.Expansion);
                answers.Add(new AnswersModel() {
                    DateAnswer =  Convert.ToString(answer.Date),
                    Text = answer.Text,
                    UrlImage = url
                });
            }
            answers.Reverse();
            return Json(answers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReadAnswer(string NameImage, string Directory)
        {
            EGalleryEntities db = new EGalleryEntities();
            var Image = db.PicturesWaiting.FirstOrDefault(p => p.Name == NameImage);
            var Answer = db.Answers.FirstOrDefault(a => a.PictureId == Image.Id);
            db.Answers.Remove(Answer);
            db.SaveChanges();
            System.IO.File.Delete(GetPathToImg(Image.Name + "." + Image.Expansion, Directory));
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}