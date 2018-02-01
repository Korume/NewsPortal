using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System.Collections.Generic;
using System.Web;

namespace NewsPortal.Interfaces
{
    public interface IStorage
    {
        void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage);

        void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId);

        void Delete(int id);

        NewsItem Get(int id);

        List<NewsItem> GetItems(int firstIndex, int itemsCount, bool sortedByDate = true);

        int Length();
    }
}