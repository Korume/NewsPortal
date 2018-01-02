using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var thumbnails = GetThumbnails();
            return View(thumbnails);
        }
        private IList<NewsItemViewModel> GetThumbnails()
        {
            List<NewsItemViewModel> thumbnails;
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemList = session.QueryOver<NewsItem>().List();
                thumbnails = new List<NewsItemViewModel>(newsItemList.Count);
                var nm = session.Get<User>(Convert.ToInt32(User.Identity.GetUserId()));
                foreach (var item in newsItemList)
                {
                    var user = session.Get<User>(item.UserId);
                    thumbnails.Add(new NewsItemViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        CreationDate = item.CreationDate,
                        UserName = session.Get<User>(item.UserId).UserName
                    });
                }
                transaction.Commit();
            }
            return thumbnails;
        }
    }
}