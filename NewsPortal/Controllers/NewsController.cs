using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;
using System;
using NewsPortal.Models.ViewModels.News;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int newsItemId)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                var newsItem = session.Get<NewsItem>(newsItemId);

                bool isUserNewsItemOwner = newsItem.UserId == User.Identity.GetUserId().AsInt();
                if (!isUserNewsItemOwner)
                {
                    return RedirectToAction("Index", "Home");
                }
                var editedNewsItem = new NewsItemEditViewModel()
                {
                    Id = newsItem.Id,
                    Title = newsItem.Title,
                    Content = newsItem.Content
                };
                return View(editedNewsItem);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveEditedNewsItem(NewsItemEditViewModel model)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                var newsItemToUpdate = session.Get<NewsItem>(model.Id);

                newsItemToUpdate.Title = model.Title;
                newsItemToUpdate.Content = model.Content;

                session.Update(newsItemToUpdate);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                var newsItem = session.Get<NewsItem>(newsItemId);

                if (newsItem == null)
                {
                    return View("NotFound");
                }

                var showMainNews = new NewsItemMainPageViewModel()
                {
                    Id = newsItem.Id,
                    Title = newsItem.Title,
                    Content = newsItem.Content
                };
                return View(showMainNews);
            }
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(NewsItemAddViewModel NewNewsItem)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                NewsItem newItem = new NewsItem()
                {
                    Id = NewNewsItem.Id,
                    Title = NewNewsItem.Title,
                    Content = NewNewsItem.Content,
                    CreationDate = DateTime.Now,
                    UserId = Convert.ToInt32(User.Identity.GetUserId())
                };
                session.Save(newItem);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                var MyNewsItem = session.Get<NewsItem>(newsItemId);
                session.Delete(MyNewsItem);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}