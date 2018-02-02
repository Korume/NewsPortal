﻿using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using NewsPortal.Managers.News;
using System;
using NewsPortal.ModelService;
using NewsPortal.Managers.Storage;

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
            return View(ModelReturner.GetHomePage(page, sortedByDate));
        }

        [HttpPost]
        public ActionResult Index(string storage)
        {
            if (storage == "Database" || cookie.Value == "Database")
            {
                StorageProvider.SwitchStorage(MemoryMode.Database);
                if (cookie.Value != "Database")
                {
                    cookie.Value = "Database";
                }
            }
            if (storage == "LocalStorage" || cookie.Value == "LocalStorage")
            {
                StorageProvider.SwitchStorage(MemoryMode.LocalStorage);
                if (cookie.Value != "LocalStorage")
                {
                    cookie.Value = "LocalStorage";
                }
            }
            cookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(cookie);
            return View(ModelReturner.GetHomePage(0, true));
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
            Storage.Add(newsModel, uploadedImage, Convert.ToInt32(User.Identity.GetUserId()));
            return RedirectToAction("Index", "News");
        }

        [Authorize]
        public ActionResult Edit(int? newsItemId)
        {
            if (newsItemId == null)
            {
                throw new HttpException(404, "Not Found");
            }

            var editedNewsItem = ModelReturner.GetEditedNewsItem(newsItemId.Value, User.Identity.GetUserId());
            if (editedNewsItem == null)
            {
                return View("NewsOwnerError");
            }
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
            Storage.Edit(editModel, uploadedImage);
            return RedirectToAction("Index", "News");
        }

        public ActionResult MainNews(int newsItemId, string title)
        {
            if (!NewsManager.CheckedNewsItem(newsItemId))
            {
                throw new HttpException(404, "Not Found");
            }
            return View(ModelReturner.GetMainNews(newsItemId));
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteNewsItem(int newsItemId)
        {
            Storage.Delete(newsItemId);
            return RedirectToAction("Index", "News");
        }
    }
}