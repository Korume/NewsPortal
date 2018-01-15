using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;
using System;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;
using System.Web;
namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(NewsItemThumbnailViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }
            using (var session = NHibernateManager.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                NewsItem newsItem = new NewsItem()
                {
                    UserId = Convert.ToInt32(User.Identity.GetUserId()),
                    Title = newsModel.Title,
                    Content = newsModel.Content,
                    CreationDate = DateTime.Now
                };
                session.Save(newsItem);

                if (uploadedImage != null)
                {
                    string fileName = System.IO.Path.GetFileName(uploadedImage.FileName);
                    uploadedImage.SaveAs(Server.MapPath("~/Content/UploadedImages/" + newsItem.Id + fileName));
                    newsItem.SourceImage = "/Content/UploadedImages/" + newsItem.Id + fileName;
                }
                session.Update(newsItem);
                transaction.Commit();
            }
            return RedirectToAction("Index", "Home");
        }

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

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            {
                var MyNewsItem = session.Get<NewsItem>(newsItemId);
                session.Delete(MyNewsItem);
            }
            //using (var session = NHibernateManager.GetCurrentSession())
            //using (var transaction = session.BeginTransaction())
            //{
            //    var MyNewsItem = session.Get<NewsItem>(newsItemId);
            //    session.Delete(MyNewsItem);
            //    transaction.Commit();
            //}
            //Создать уведомление "Новость удалена успешно"
            return RedirectToAction("Index", "Home");
        }
    }
}