using System.Web;
using System.Web.Mvc;
using NewsPortal.Managers.Storage;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        const int newsItemsQuantity = 15;
        HttpCookie cookie = new HttpCookie("cookieValue");

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            return View(Storage.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(bool isDatabase)
        {
            if (isDatabase || cookie.Value == "Database")
            {
                StorageProvider.SwitchStorage(MemoryMode.Database);
            }
            else if (!isDatabase || cookie.Value == "LocalStorage")
            {
                StorageProvider.SwitchStorage(MemoryMode.LocalStorage);
            }

            //лучше был бы свитч

            Response.Cookies.Add(cookie);
            return View(Storage.GetHomePage(0, true));
        }
    }
}
