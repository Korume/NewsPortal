using NewsPortal.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain
{
    public interface INewsItemRepository
    {
        Task Add(NewsItem newsItem);
        Task Update(NewsItem newsItem);
        Task Delete(NewsItem newsItem);
        Task<NewsItem> GetById(int newsItemId);
        Task<IList<NewsItem>> GetPageItems(int pageIndex, int itemsQuantity, bool sortedByDate);
        Task<int> GetNewsItemsQuantity();
    }
}
