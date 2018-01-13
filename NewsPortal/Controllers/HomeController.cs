using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 0, int newsItemsQuantity = 20, bool sortedByDate = true)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            {
                var propertyForOrder = "CreationDate";
                var orderType = sortedByDate ? Order.Desc(propertyForOrder) : Order.Asc(propertyForOrder);
                var newsItemList = session.CreateCriteria<NewsItem>().
                    AddOrder(orderType).
                    SetFirstResult(page * newsItemsQuantity).
                    SetMaxResults(newsItemsQuantity).
                    List<NewsItem>();

                var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);
                foreach (var item in newsItemList)
                {
                    var userLogin = session.Get<User>(item.UserId).Login;

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserLogin = userLogin
                    });
                }

                return View(thumbnails);
            }
        }
    }
}
