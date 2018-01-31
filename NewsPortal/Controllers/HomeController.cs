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
        HttpCookie cookie = new HttpCookie("Storage");

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            if (cookie.Value == null)
            {
                cookie.Value = "Database";
                cookie.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Add(cookie);
            }
            return View(StorageManager.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(bool isDatabase)
        {
            if (isDatabase || cookie.Value == "Database")
            {
                MemoryMode.CurrentMemoryMode = MemMode.Database;
                if (cookie.Value != "Database")
                {
                    cookie.Value = "Database";
                }
            }
            else if (!isDatabase || cookie.Value == "LocalStorage")
            {
                MemoryMode.CurrentMemoryMode = MemMode.LocalStorage;
                if (cookie.Value != "LocalStorage")
                {
                    cookie.Value = "LocalStorage";
                }
            }
            cookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(cookie);
            return View(StorageManager.GetHomePage(0, true));
        }
    }
}
