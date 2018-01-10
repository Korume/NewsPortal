using Microsoft.AspNet.Identity;
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
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }
           
            using (var session = NHibernateHelper.GetCurrentSession())
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
        
    }
}
