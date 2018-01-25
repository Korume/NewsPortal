using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using System.Configuration;
using System;
using NewsPortal.Managers.Storage;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        const int newsItemsQuantity = 15;

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
                return View(StorageManager.GetHomePage(page,sortedByDate));
        }
    }
}
