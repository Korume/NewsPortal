using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;
using System;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int newsItemId)
        {
            using (var session = NHibernateManager.GetCurrentSession())
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
            using (var session = NHibernateManager.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemToUpdate = session.Get<NewsItem>(model.Id);

                newsItemToUpdate.Title = model.Title;
                newsItemToUpdate.Content = model.Content;

                session.Update(newsItemToUpdate);
                transaction.Commit();
            }
            //Cделать уведомление "Новость сохранена успешно"
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult MainNews(int newsItemId)
        {
            var newsItem = NHibernateManager.ReturnDB_News(newsItemId);
            var newsUser = NHibernateManager.ReturnDB_User(newsItem.UserId);

            var showMainNews = new NewsItemMainPageViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                CreationDate = newsItem.CreationDate,
                UserId = newsItem.UserId,
                UserName = newsUser.UserName
            };
            //Возвращаем список комментариев находятся на странице новостей
            ViewBag.NewsItemCommentaries = CommentaryManager.ReturnCommentaries(newsItemId);

            return View(showMainNews);
        }

        //Временный метод
        public string MainNews(string newsTitle, int newsItemId)
        {
            return newsTitle + " " + newsItemId.ToString();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(NewsItemAddViewModel NewNewsItem)
        {
            if (!ModelState.IsValid)
            {
                return View(NewNewsItem);
            }

            using (var session = NHibernateManager.GetCurrentSession())
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
            //Создать уведомление "Новость сохранена успешно"
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var MyNewsItem = session.Get<NewsItem>(newsItemId);
                session.Delete(MyNewsItem);
                transaction.Commit();
            }
            //Создать уведомление "Новость удалена успешно"
            return RedirectToAction("Index", "Home");
        }

    }
}