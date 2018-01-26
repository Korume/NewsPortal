using System.Web.Mvc;
using NewsPortal.Managers.Storage;
using NewsPortal.Models.ViewModels;

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
                StorageManager.GetCheckedToggle(false);
            }
            else if (isDatabase == false)
            {
                MemoryMode.MemorySwitch(MemMode.LocalStorage);
                StorageManager.GetCheckedToggle();
            }

            return View(StorageManager.GetHomePage(0, true));
        }
    }
}
