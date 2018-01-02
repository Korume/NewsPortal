using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Host.SystemWeb;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsPortal.Account;
using System.Text;
using System.Net.Mail;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Authorization
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginInputVM model)
        {
            if (ModelState.IsValid)
            {
                var result = SignInManager.PasswordSignIn(model.Login, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    return RedirectToAction("Index", "Home");             
                }
                else
                {
                    //ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return RedirectToAction("Login", "Account");
                }
            }
            //NHibernateHelper.CloseSession();
            return View(model);
        }

        //[HttpPost] //Выйти с сервера
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //COOKIES
        public SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }
        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }


        //GET: Registation
        [HttpGet]
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
                    var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager>();

                    User newUser = new User()
                    {
                        Email = registerModel.Email,
                        Login = registerModel.Login,
                        Password = registerModel.Password,
                        UserName = registerModel.UserName,
                        PasswordHash = registerModel.Password,
                        EmailConfirmed = false
                    };

                    var result = UserManager.Create(newUser, registerModel.Password);
                    if (result.Succeeded)
                    {
                        SignInManager.SignIn(newUser, false, false);
                        session.Save(newUser);
                        transaction.Commit();

                        var id = session.QueryOver<User>().Where(u => u.Email == registerModel.Email).List().First().Id;
                        SendEmail(id, registerModel.Email);
                        return RedirectToAction("Register", "Account");
                    }
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