using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
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
        public ActionResult Index(bool isDatabase)
        {
            if (isDatabase == true)
            {
                MemoryMode.MemorySwitch(MemMode.Database);
            }
            else if (isDatabase == false)
            {
                MemoryMode.MemorySwitch(MemMode.LocalStorage);
            }

            return View(StorageManager.GetHomePage(0, true));
        }
    }
}
