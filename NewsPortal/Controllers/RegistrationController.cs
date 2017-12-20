using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(RegisterInputVM registerModel)
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