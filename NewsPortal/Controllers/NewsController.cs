using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;
using System;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        public ActionResult MainNews()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int newsItemId)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
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
                transaction.Commit();
                return View(editedNewsItem);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveEditedNewsItem(NewsItemEditViewModel model)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemToUpdate = session.Get<NewsItem>(model.Id);
                newsItemToUpdate.Title = model.Title;
                newsItemToUpdate.Content = model.Content;

                session.Update(newsItemToUpdate);
                transaction.Commit();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            //return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveNewsItem(NewsItemAddViewModel NewNewsItem)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
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
                transaction.Commit();
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var MyNewsItem = session.Get<NewsItem>(newsItemId);
                session.Delete(MyNewsItem);
                transaction.Commit();
                NHibernateHelper.CloseSession();
            }        
            return RedirectToAction("Index", "Home");
        }

    }
}