using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;
using System.Web;
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

            return View(StorageManager.GetEdit(newsItemId, User.Identity.GetUserId()));

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
            //Cделать уведомление "Новость сохранена успешно"
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId)
        {
            if (!NewsManager.CheckedNewsItem(newsItemId))
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(StorageManager.GetMainNews(newsItemId));
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