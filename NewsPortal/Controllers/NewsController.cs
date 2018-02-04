using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using NewsPortal.Managers.News;
using System;
using NewsPortal.Managers.Storage;
using System.Collections.Generic;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Managers.NHibernate;
using System.Configuration;
using NewsPortal.Managers.Commentary;
using NewsPortal.Models.ViewModels.News;
using System.Web.WebPages;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        HttpCookie cookie = new HttpCookie("Storage");

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            int newsItemsQuantity = int.Parse(ConfigurationManager.AppSettings["newsItemsQuantity"]);

            if (cookie.Value == null)
            {
                cookie.Value = "Database";
                cookie.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Add(cookie);
            }

            int lastPage = (int)Math.Ceiling(StorageManager.GetStorage().Length() / (double)newsItemsQuantity) - 1;
            if (lastPage == -1)
            {
                lastPage = 0;
            }
            else if (page < 0 || page > lastPage)
            {
                throw new HttpException(404, "Not found");
            }

            var newsItemsList = StorageManager.GetStorage().GetItems(page, newsItemsQuantity, sortedByDate);
            var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();

                foreach (var item in newsItemsList)
                {
                    var userName = session.Get<User>(item.UserId)?.UserName ?? string.Empty;
                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserName = userName
                    });
                }
            }

            var newsPageModel = new NewsPageModel()
            {
                Thumbnails = thumbnails,
                CurrentPageIndex = page,
                LastPageIndex = lastPage,
                SortedByDate = sortedByDate,

            };

            return View(newsPageModel);
        }

        [HttpPost]
        public ActionResult Index(string storage)
        {
            if (storage == "Database" || cookie.Value == "Database")
            {
                StorageManager.SwitchStorage(MemoryMode.Database);
                if (cookie.Value != "Database")
                {
                    cookie.Value = "Database";
                }
            }
            if (storage == "LocalStorage" || cookie.Value == "LocalStorage")
            {
                StorageManager.SwitchStorage(MemoryMode.LocalStorage);
                if (cookie.Value != "LocalStorage")
                {
                    cookie.Value = "LocalStorage";
                }
            }
            cookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(cookie);

            return Index(); // Переделать это говно! (c) def1x
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (string.IsNullOrEmpty(newsModel.Title) && string.IsNullOrEmpty(newsModel.Content))
            {
                return View(newsModel);
            }

            StorageManager.GetStorage().Add(newsModel, uploadedImage, Convert.ToInt32(User.Identity.GetUserId()));
            return RedirectToAction("Index", "News");
        }

        [Authorize]
        public ActionResult Edit(int? newsItemId)
        {
            if (newsItemId == null)
            {
                throw new HttpException(404, "Not Found");
            }

            var newsItem = StorageManager.GetStorage().Get(newsItemId.Value); // Переделать (c) def1x
            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }

            bool isUserNewsItemOwner = newsItem.UserId == User.Identity.GetUserId().AsInt();
            if (!isUserNewsItemOwner)
            {
                return View("NewsOwnerError");
            }

            var userName = NHibernateManager.ReturnDB_User(newsItem.UserId).UserName; // Переделать (c) def1x
            var editedNewsItem = new NewsItemViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate,
                UserName = userName
            };

            return View(editedNewsItem);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }
            StorageManager.GetStorage().Edit(editModel, uploadedImage);
            return RedirectToAction("Index", "News");
        }

        public ActionResult MainNews(int newsItemId, string title)
        {
            if (!NewsManager.CheckedNewsItem(newsItemId))
            {
                throw new HttpException(404, "Not Found");
            }

            var newsItem = StorageManager.GetStorage().Get(newsItemId);
            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }

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
            StorageManager.GetStorage().Delete(newsItemId);
            return RedirectToAction("Index", "News");
        }
    }
}