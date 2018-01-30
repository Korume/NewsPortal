using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System.Web;

namespace NewsPortal.Interfaces
{
    public interface IStorage
    {
        void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage);

        void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId);

        void Delete(int id);

        NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string userId);

        HomePageModel GetHomePage(int page = 0, bool sortedByDate = true);

        NewsItemMainPageViewModel GetMainNews(int id);
    }
}