using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using System.Configuration;
using System;
using System.Web;
using NewsPortal.Managers.Storage;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        const int newsItemsQuantity = 15;

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            return View(StorageManager.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(string action)
        {
            if (action == "database")
            {
                MemoryMode.MemorySwitch("database");
            }
            else if (action == "memory")
            {
                MemoryMode.MemorySwitch("memory");
            }
            return View(StorageManager.GetHomePage(0, true));
        }
    }
}
