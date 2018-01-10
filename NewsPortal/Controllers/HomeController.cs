using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<NewsItemThumbnailViewModel> thumbnails = GetThumbnails();
            return View(thumbnails);
        }

        public ActionResult Sort()
        {
            var thumbnails = GetThumbnails();
            thumbnails.Sort((x, y) => y.CreationDate.CompareTo(x.CreationDate));
            return View("Index", thumbnails);
        }

        private List<NewsItemThumbnailViewModel> GetThumbnails()
        {
            using (var session = NHibernateManager.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemList = session.QueryOver<NewsItem>().List();
                var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemList.Count);

                foreach (var item in newsItemList)
                {
                    var user = session.Get<User>(item.UserId);

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserLogin = user.Login
                    });
                }

                transaction.Commit();

                return thumbnails;
            }
        }
    }
}
