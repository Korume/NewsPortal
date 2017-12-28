using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult MainNews()
        {
            return View();
        }
    }
}