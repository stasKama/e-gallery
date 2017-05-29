using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Web_gellary.Models
{
    public class SendEmail
    {
        private static readonly int port = 587;
        private static readonly string smtp = "smtp.mail.ru";
        private static readonly bool SSL = true;
        private static NetworkCredential login = new NetworkCredential("e.gallery.sys@mail.ru", "egallery2905Sys");
        private static SmtpClient client = new SmtpClient(smtp);
        private static MailMessage msg;
        private static Random random = new Random();


        public static void SendVerificationCode(string EmailAddressUser, string NickUser, int UserId)
        {
            int verificationCode = random.Next(1000000, 9999999);
            client.Port = port;
            client.EnableSsl = SSL;
            client.Credentials = login;
            msg = new MailMessage()
            {
                From = new MailAddress("e.gallery.sys@mail.ru", "E-Gallery", Encoding.UTF8)
            };
            msg.To.Add(new MailAddress(EmailAddressUser));
            msg.Subject = "Verification Code Registration.";
            msg.Body = "Hi, " + NickUser + ", this your verification code to registration: " + "<b>" + verificationCode + "</b>";
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(msg);
            EGalleryEntities db = new EGalleryEntities();
            db.Verification.Add(new Verification()
            {
                UserId = UserId,
                VerificationCode = Convert.ToString(verificationCode)
            });
            db.SaveChanges();
        }
    }
}