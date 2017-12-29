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
        public ActionResult Login(LoginViewModel model)
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
                    return RedirectToAction("Login", "Account");
                }

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }
        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }

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
                    //SendEmail(id, registerModel.Email);
                    return RedirectToAction("Register", "Account");
                }
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("Login", "Account");
        }


    }
}