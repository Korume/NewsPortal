using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using System.Configuration;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            var newsItemsQuantity = int.Parse(ConfigurationManager.AppSettings["newsItemsQuantityOnHomePage"]);
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();

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
                var homePageModel = new HomePageModel()
                {
                    Thumbnails = thumbnails,
                    Page = page,
                    SortedByDate = sortedByDate
                };
                return View(homePageModel);
            }
        }
    }
}
