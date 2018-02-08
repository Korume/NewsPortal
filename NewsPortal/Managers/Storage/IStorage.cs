using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace NewsPortal.Interfaces
{
    public interface IStorage
    {
        Task Add(NewsItem newsItem);
        Task Delete(int newsItemId);
        Task Edit(NewsItem newsItem);
        Task<NewsItem> Get(int newsItem);
        Task<IEnumerable<NewsItem>> GetPageItems(int pageIndex, int itemsQuantity, bool sortedByDate);
        Task<int> NewsItemsQuantity();
    }
}