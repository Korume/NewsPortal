using System.Collections.Generic;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Managers.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using System.Configuration;
using System;
using System.Web;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        const int newsItemsQuantity = 15;

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();

                var lastPage = (int)Math.Ceiling(session.QueryOver<NewsItem>().RowCount() / (double)newsItemsQuantity) - 1;
                if (page < 0 || page > lastPage)
                {
                    throw new HttpException(404, "Not Found");
                }

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
                    var userName = session.Get<User>(item.UserId)?.UserName ?? String.Empty;

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserName = userName
                    });
                }
                var homePageModel = new HomePageModel()
                {
                    Thumbnails = thumbnails,
                    CurrentPage = page,
                    SortedByDate = sortedByDate,
                    LastPage = lastPage
                };
                return View(homePageModel);
            }
        }
    }
}
