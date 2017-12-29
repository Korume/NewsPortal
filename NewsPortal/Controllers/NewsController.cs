using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [Authorize]
        [HttpPost]
        public ActionResult EditNewsItem(int newsItemId)
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

        [Authorize]
        [HttpPost]
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
    }
}