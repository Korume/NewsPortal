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
using NewsPortal.Managers.Picture;
using NewsPortal.Managers.News;
using NewsPortal.Managers.Storage;

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
        public ActionResult Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }
            StorageManager.Add(newsModel, uploadedImage, User.Identity.GetUserId());
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Edit(int? newsItemId)
        {
            if (newsItemId == null)
            {
                return Redirect("/Error/NotFound");
            }

            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
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
                    Content = newsItem.Content,
                    SourceImage = newsItem.SourceImage
                };
                return View(editedNewsItem);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            if(!ModelState.IsValid)
            {
                return View(editModel);
            }

            StorageManager.Edit(editModel,uploadedImage);
            //Cделать уведомление "Новость сохранена успешно"
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId)
        {
            if (!NewsManager.CheckedNewsItem(newsItemId))
            {
                return RedirectToAction("NotFound","Error");
            }

            var newsItem = NHibernateManager.ReturnDB_News(newsItemId);
            var newsUser = NHibernateManager.ReturnDB_User(newsItem.UserId);
            var commentItems = CommentaryManager.ReturnCommentaries(newsItemId);

            var showMainNews = new NewsItemMainPageViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate,
                UserId = newsItem.UserId,
                UserName = newsUser.UserName,
                CommentItems = commentItems
            };
            return View(showMainNews);
        }
 
        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            StorageManager.Delete(newsItemId);
            //Создать уведомление "Новость удалена успешно"
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Partial()
        {            
            return PartialView("DialogWindow");
        }
    }
}