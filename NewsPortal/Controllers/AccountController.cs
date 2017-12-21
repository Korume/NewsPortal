using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Authorization
        [HttpGet]
        public ActionResult IndexAuthorization()
        {
            return View();
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
                    User newUser = new User()
                    {
                        Email = registerModel.Email,
                        Login = registerModel.Login,
                        Password = registerModel.Password
                    };
                    session.Save(newUser);
                    transaction.Commit();
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("Index", "Authorization");
        }
    }
}