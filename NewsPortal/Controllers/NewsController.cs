using NewsPortal.Models.ViewModels;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;
using System.Web;
using NewsPortal.Managers.News;
using NewsPortal.Managers.Storage;
using NewsPortal.ModelService;

namespace NewsPortal.Controllers
{
    public class NewsController : Controller
    {
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

            Storage.Add(newsModel, uploadedImage, User.Identity.GetUserId());

            return RedirectToAction("Index", "Home");
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
            return View(ModelReturner.GetEditedNewsItem(newsItemId.Value, User.Identity.GetUserId()));
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

            Storage.Edit(editModel, uploadedImage);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MainNews(int newsItemId)
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
            return RedirectToAction("Index", "Home");
        }
    }
}