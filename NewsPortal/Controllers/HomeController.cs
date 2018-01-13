using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 0, int quantity = 20, bool sorted = true)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemList = session.QueryOver<NewsItem>().List();

                if (sorted)
                {
                    (newsItemList as List<NewsItem>).Sort((x, y) => y.CreationDate.CompareTo(x.CreationDate));
                }
                else
                {
                    (newsItemList as List<NewsItem>).Sort((x, y) => x.CreationDate.CompareTo(y.CreationDate));
                }

                var thumbnails = new List<NewsItemThumbnailViewModel>(quantity);

                for (int i = page * quantity; i < (page + 1) * quantity; i++)
                {
                    if (i >= newsItemList.Count)
                    {
                        break;
                    }

                    var userName = session.Get<User>(newsItemList[i].UserId).UserName;

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = newsItemList[i].Id,
                        Title = newsItemList[i].Title,
                        UserId = newsItemList[i].UserId,
                        CreationDate = newsItemList[i].CreationDate,
                        UserLogin = userName
                    });
                }
                return View(thumbnails);
            }
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
