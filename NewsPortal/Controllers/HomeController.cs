using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NewsPortal.Managers.Storage;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(CacheProfile = "cacheProfile")]
        public ActionResult Index()
        {   
            return View();
        }
    }
}
