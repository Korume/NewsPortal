using System;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Managers.Storage;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        const int newsItemsQuantity = 15;
        HttpCookie cookie = new HttpCookie("cookieValue");

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            return View(StorageManager.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(bool isDatabase)
        {
            if (isDatabase || cookie.Value == "Database")
            {
                MemoryMode.CurrentMemoryMode = MemMode.Database;
                cookie["Storage"] = "Database";
            }
            else if (!isDatabase || cookie.Value == "LocalStorage")
            {
                MemoryMode.CurrentMemoryMode = MemMode.LocalStorage;
                cookie["Storage"] = "LocalStorage";
            }
            Response.Cookies.Add(cookie);
            return View(StorageManager.GetHomePage(0, true));
        }
    }
}
