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

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Authorization
        public ActionResult IndexAuthorization()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexAuthorization(LoginInputVM model)
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
                    return RedirectToAction("IndexAuthorization", "Account");
                }
            }
            //NHibernateHelper.CloseSession();
            return View(model);
        }
        //public ActionResult IndexRegistration()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult> IndexRegistrationAsync(RegisterInputVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new User() { Login = model.Login };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, false, false);
        //            return RedirectToAction("IndexAuthorization", "Account");
        //        }
        //        else
        //        {
        //            return RedirectToAction("IndexRegistration", "Account");
        //        }
        //    }
        //    return View(model);
        //}


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
        public ActionResult IndexRegistration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexRegistration(RegisterInputVM registerModel)
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
                        Password = registerModel.Password, //               |
                        UserName = registerModel.UserName,      
                        PasswordHash = registerModel.Password //            |
                    };

                    //var userManager = HttpContext.GetOwinContext().GetUserManager<UserManager>();
                    //var user = new User() { UserName = model.UserName };
                    //var result = userManager.Create(user, model.Password);

                    var result = UserManager.Create(newUser, registerModel.Password);
                    if (result.Succeeded)
                    {
                        SignInManager.SignIn(newUser, false, false);
                        session.Save(newUser);
                        transaction.Commit();
                        return RedirectToAction("IndexAuthorization", "Account");
                    }
                    //else
                    //{
                    //    session.Close();
                    //    return RedirectToAction("IndexRegistration", "Account");
                    //}
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("IndexAuthorization", "Account");
        }
    }
}