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
            return View(StorageProvider.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(bool isDatabase)
        {
            if (isDatabase == true)
            {
                MemoryMode.MemorySwitch(MemMode.Database);
                StorageProvider.GetCheckedToggle(false);
            }
            else if (isDatabase == false)
            {
                MemoryMode.MemorySwitch(MemMode.LocalStorage);
                StorageProvider.GetCheckedToggle();
            }

            return View(StorageProvider.GetHomePage(0, true));
        }
    }
}
