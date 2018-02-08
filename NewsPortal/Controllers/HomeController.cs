using System.Web;
using System.Web.Mvc;
using NewsPortal.Managers.Storage;

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
