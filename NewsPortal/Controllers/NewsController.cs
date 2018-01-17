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
        public ActionResult Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(newsModel);
            }
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
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
            }
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

            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    var newsItemToUpdate = session.Get<NewsItem>(editModel.Id);

                    newsItemToUpdate.Title = editModel.Title;
                    newsItemToUpdate.Content = editModel.Content;

                    session.Update(newsItemToUpdate);
                    transaction.Commit();
                }
            }
            //Cделать уведомление "Новость сохранена успешно"
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId)
        {
            var newsItem = NHibernateManager.ReturnDB_News(newsItemId);
            var newsUser = NHibernateManager.ReturnDB_User(newsItem.UserId);
            var commentItems = CommentaryManager.ReturnCommentaries(newsItemId);

            var showMainNews = new NewsItemMainPageViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
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
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    var newsItem = session.Get<NewsItem>(newsItemId);
                    session.Delete(newsItem);
                    transaction.Commit();
                }
            }
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