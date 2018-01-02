﻿using Microsoft.AspNet.Identity;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class CreationItemController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Index(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }
            var session = NHibernateHelper.GetCurrentSession();
            try
            {
                using (var transaction = session.BeginTransaction())
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
                    transaction.Commit();
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
