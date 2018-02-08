using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using NewsPortal.Managers.News;
using System;
using NewsPortal.Managers.Storage;
using System.Collections.Generic;
using NewsPortal.Models.DataBaseModels;
using System.Configuration;
using NewsPortal.Managers.Commentary;
using NewsPortal.Models.ViewModels.News;
using System.Web.WebPages;
using NewsPortal.Repositories;
using NewsPortal.Domain;
using System.Threading.Tasks;
using NewsPortal.Managers.Picture;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        HttpCookie cookie = new HttpCookie("Storage");

        public async Task<ActionResult> Index(int page = 0, bool sortedByDate = true)
        {
            int newsItemsQuantityPerPage = int.Parse(ConfigurationManager.AppSettings["newsItemsQuantityPerPage"]);

            if (cookie.Value == null)
            {
                cookie.Value = "Database";
                cookie.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Add(cookie);
            }

            int lastPage = (int)Math.Ceiling(await StorageManager.GetStorage().NewsItemsQuantity() / (double)newsItemsQuantityPerPage) - 1;
            if (lastPage == -1)
            {
                lastPage = 0;
            }
            else if (page < 0 || page > lastPage)
            {
                throw new HttpException(404, "Not found");
            }

            var newsItemsList = await StorageManager.GetStorage().GetPageItems(page, newsItemsQuantityPerPage, sortedByDate);
            var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantityPerPage);

            var userRepository = UnityConfig.Resolve<IUserRepository>();
            foreach (var item in newsItemsList)
            {
                var user = await userRepository.GetById(item.UserId);
                var userName = user?.UserName ?? string.Empty;
                thumbnails.Add(new NewsItemThumbnailViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    UserId = item.UserId,
                    CreationDate = item.CreationDate,
                    UserName = userName
                });
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

            return RedirectToAction("Index");
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
        public async Task<ActionResult> Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if(!ModelState.IsValid)
            {
                return View(newsModel);
            }

            var sourceImage = PictureManager.Upload(uploadedImage);

            var newsItem = new NewsItem()
            {
                UserId = Convert.ToInt32(User.Identity.GetUserId()),
                Title = newsModel.Title,
                Content = newsModel.Content,
                SourceImage = sourceImage,
                CreationDate = DateTime.Now
            };

            await StorageManager.GetStorage().Add(newsItem);
            return RedirectToAction("Index", "News");
        }

        [Authorize]
        public async Task<ActionResult> Edit(int? newsItemId)
        {
            if (newsItemId == null)
            {
                throw new HttpException(404, "Not Found");
            }

            var newsItem = await StorageManager.GetStorage().Get(newsItemId.Value);
            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }

            bool isUserNewsItemOwner = newsItem.UserId == User.Identity.GetUserId().AsInt();
            if (!isUserNewsItemOwner)
            {
                return View("NewsOwnerError");
            }

            var newsItemForEditing = new NewsItemViewModel()
            {
                Id = newsItem.Id,
                UserId = newsItem.UserId,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate
            };

            return View(newsItemForEditing);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            // Надо сделать обновление картинки (c) def1x

            var editedNewsItem = new NewsItem()
            {
                Id = editModel.Id,
                UserId = editModel.UserId,
                Title = editModel.Title,
                Content = editModel.Content,
                SourceImage = editModel.SourceImage, // Это тоже поменяется (c) def1x
                CreationDate = editModel.CreationDate
            };

            await StorageManager.GetStorage().Edit(editedNewsItem);

            return RedirectToAction("Index", "News");
        }

        public async Task<ActionResult> MainNews(int? newsItemId, string title)
        {
            if (newsItemId == null)
            {
                throw new HttpException(404, "Not Found");
            }

            var newsItem = await StorageManager.GetStorage().Get(newsItemId.Value);
            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }

            string canonicalUrl = NewsManager.EditNewsTitleForUrl(newsItem.Title);
            bool isCanonical = title.ToLower() == canonicalUrl.ToLower();

            var userRepository = UnityConfig.Resolve<IUserRepository>();
            var newsUser = await userRepository.GetById(newsItem.UserId);

            var commentRepository = UnityConfig.Resolve<ICommentRepository>();
            var commentItems = await commentRepository.GetCommentsOnNewsPage(newsItemId.Value);

            var showMainNews = new NewsItemMainPageViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate,
                UserId = newsItem.UserId,
                UserName = newsUser.UserName,
                CommentItems = commentItems,
                IsCanonical = isCanonical,
                CanonicalUrl = canonicalUrl
            };

            return View(showMainNews);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> DeleteNewsItem(int newsItemId)
        {
            var newsItem = await StorageManager.GetStorage().Get(newsItemId);
            // Надо удалить картинку (c) def1x
            await StorageManager.GetStorage().Delete(newsItemId);

            return RedirectToAction("Index", "News");
        }
    }
}