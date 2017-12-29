using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<NewsItemThumbnailVM> thumbnails = GetThumbnails();
            return View(thumbnails);
        }

        public ActionResult Sort()
        {
            var thumbnails = GetThumbnails();
            thumbnails.Sort((x, y) => y.CreationDate.CompareTo(x.CreationDate));
            return View("Index", thumbnails);
        }

        private List<NewsItemThumbnailVM> GetThumbnails()
        {
            List<NewsItemThumbnailVM> thumbnails;
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemList = session.QueryOver<NewsItem>().List();
                thumbnails = new List<NewsItemThumbnailVM>(newsItemList.Count);
                foreach (var item in newsItemList)
                {
                    var user = session.Get<User>(item.UserId);
                    thumbnails.Add(new NewsItemThumbnailVM()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserLogin = user.Login
                    });
                }
                transaction.Commit();
            }
            return thumbnails;
        }
    }
}