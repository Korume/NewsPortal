using System;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Managers.Storage;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
