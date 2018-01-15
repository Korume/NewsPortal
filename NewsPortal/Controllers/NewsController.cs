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
        [Authorize]
        public ActionResult Edit(int? newsItemId)
        {
            if (newsItemId == null)
            {
                return View("NotFound");
            }
            using (var session = NHibernateManager.GetCurrentSession())
            {
                var newsItem = session.Get<NewsItem>(newsItemId);
                if (newsItem == null)
                {
                    return View("NotFound");
                }

                bool isUserNewsItemOwner = newsItem.UserId == User.Identity.GetUserId().AsInt();
                if (!isUserNewsItemOwner)
                {
                    return View("NewsOwnerError");
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
        [ValidateInput(false)]
        public ActionResult Edit(NewsItemEditViewModel editModel)
        {
            if(!ModelState.IsValid)
            {
                return View(editModel);
            }

            using (var session = NHibernateManager.GetCurrentSession())
            {
                var newsItemToUpdate = session.Get<NewsItem>(editModel.Id);

                newsItemToUpdate.Title = editModel.Title;
                newsItemToUpdate.Content = editModel.Content;

                session.Update(newsItemToUpdate);
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

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }

            using (var session = NHibernateManager.GetCurrentSession())
            {
                NewsItem newsItem = new NewsItem()
                {
                    Id = newsModel.Id,
                    UserId = Convert.ToInt32(User.Identity.GetUserId()),
                    Title = newsModel.Title,
                    Content = newsModel.Content,
                    CreationDate = DateTime.Now
                };
                if (uploadedImage != null)
                {
                    string fileName = System.IO.Path.GetFileName(uploadedImage.FileName);
                    uploadedImage.SaveAs(Server.MapPath("~/Content/UploadedImages/" + fileName));
                    newsItem.SourceImage = "/Content/UploadedImages/" + fileName;
                }
                session.Save(newsItem);
            }
            return RedirectToAction("Index", "Home");
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
            //Создать уведомление "Новость удалена успешно"
            return RedirectToAction("Index", "Home");
        }
    }
}