using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;
using System.Web;
using NewsPortal.Managers.News;
using NewsPortal.Managers.Storage;
using System;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
        const int newsItemsQuantity = 15;
        HttpCookie cookie = new HttpCookie("Storage");

        public ActionResult Index(int page = 0, bool sortedByDate = true)
        {
            if (cookie.Value == null)
            {
                cookie.Value = "Database";
                cookie.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Add(cookie);
            }
            return View(StorageManager.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(string storage)
        {
            if (storage == "Database" || cookie.Value == "Database")
            {
                MemoryMode.CurrentMemoryMode = MemMode.Database;
                if (cookie.Value != "Database")
                {
                    cookie.Value = "Database";
                }
            }
            else if (storage == "LocalStorage" || cookie.Value == "LocalStorage")
            {
                MemoryMode.CurrentMemoryMode = MemMode.LocalStorage;
                if (cookie.Value != "LocalStorage")
                {
                    cookie.Value = "LocalStorage";
                }
            }
            cookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(cookie);
            return View(StorageManager.GetHomePage(0, true));
        }


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
                throw new HttpException(404, "Not Found");
            }
            var editedNewsItem = StorageManager.GetEditedNewsItem(newsItemId, User.Identity.GetUserId());
            if (editedNewsItem == null)
            {
                return View("NewsOwnerError");
            }
            return View(StorageManager.GetEditedNewsItem(newsItemId, User.Identity.GetUserId()));
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(editModel);
            }

            StorageManager.Edit(editModel, uploadedImage);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId, string title)
        {
            if (!NewsManager.CheckedNewsItem(newsItemId))
            {
                throw new HttpException(404, "Not Found");
            }

            return View(StorageManager.GetMainNews(newsItemId, title));
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            StorageManager.Delete(newsItemId);
            return RedirectToAction("Index", "Home");
        }
    }
}