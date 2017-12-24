using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using NewsPortal.Models;
using Microsoft.Owin.Security;
using System.Net.Mail;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var session = NHibernateHelper.GetCurrentSession();
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    var list = session.QueryOver<User>().Where(u => u.Email == registerModel.Email).List();
                    if (list.Count > 0)
                    {
                        ModelState.AddModelError("Email", "Данный E-mail адрес занят.");
                        return View(registerModel);
                    }

                    User newUser = new User() {
                        Email = registerModel.Email,
                        Login = registerModel.Login,
                        Password = registerModel.Password
                    };
                    session.Save(newUser);
                    transaction.Commit();

                    var id = session.QueryOver<User>().Where(u => u.Email == registerModel.Email).List().First().Id;
                    SendEmail(id, registerModel.Email);
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("Login", "Account");
        }

        private void SendEmail(int id, string email)
        {
            MailAddress from = new MailAddress("inging234@gmail.com", "NewsPortal registration");
            MailAddress to = new MailAddress(email);
            using (MailMessage message = new MailMessage(from, to))
            using (SmtpClient smtp = new SmtpClient())
            {
                message.Subject = "Email confirmation";
                message.Body = string.Format("Для завершения регистрации перейдите по ссылке:" +
                                "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>",
                    Url.Action("ConfirmEmail", "Account", new { token = id, code = email },
                    Request.Url.Scheme));
                message.IsBodyHtml = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(from.Address, "3m0a0r3i7n6a");
                smtp.Send(message);
            }
        }
        public ActionResult ConfirmEmail(int token, string code)
        {
            if (code != null)
            {
                var session = NHibernateHelper.GetCurrentSession();
                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        var userToUpdate = session.Get<User>(token);
                        userToUpdate.EmailConfirmed = true;

                        session.Save(userToUpdate);
                        transaction.Commit();
                    }
                }
                finally
                {
                    NHibernateHelper.CloseSession();
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}