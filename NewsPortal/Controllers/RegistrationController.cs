using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models;

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
        public ActionResult Index(RegisterModel model)
        {
            User user = new User() { Email = model.Email, Login = model.Login, Password = model.Password };

            var session = NHibernateHelper.GetCurrentSession();
            try
            {
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                using (var tx = session.BeginTransaction())
                {
                    var list = session.QueryOver<User>().List();

                    //var criteria = session.CreateCriteria<User>();
                    //var list = criteria.List<User>();

                    foreach (var item in list)
                    {
                        if (user.Email == item.Email)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        session.Save(user);
                        tx.Commit();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
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