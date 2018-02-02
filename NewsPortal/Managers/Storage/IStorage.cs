using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System.Collections.Generic;
using System.Web;

namespace NewsPortal.Interfaces
{
    public interface IStorage
    {
        void Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage);
        void Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage, int UserId);
        void Delete(int id);

        #region Вспомогательные функции
        NewsItem Get(int id);
        List<NewsItem> GetItems(int firstIndex, int itemsCount, bool sortedByDate = true);
        int Length();
        #endregion
    }
}