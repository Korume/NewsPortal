using NewsPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NewsPortal.Repositories
{
    public class NewsItemRepository : INewsItemRepository
    {
        public async Task Add(NewsItem newsItem)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.SaveAsync(newsItem);
                await transaction.CommitAsync();
            }
        }

        public async Task Delete(NewsItem newsItem)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.DeleteAsync(newsItem);
                await transaction.CommitAsync();
            }
        }

        public async Task<NewsItem> GetById(int newsItemId)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.GetAsync<NewsItem>(newsItemId);
            }
        }

        public async Task Update(NewsItem newsItem)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.UpdateAsync(newsItem);
                await transaction.CommitAsync();
            }
        }

        public async Task<IList<NewsItem>> GetPageItems(int pageIndex, int itemsQuantity, bool sortedByDate)
        {
            using (var session = SessionManager.OpenSession())
            { 
                var propertyForOrder = "CreationDate";
                var orderType = sortedByDate ? Order.Desc(propertyForOrder) : Order.Asc(propertyForOrder);
                var newsItemsList = await session.CreateCriteria<NewsItem>().
                AddOrder(orderType).
                SetFirstResult(pageIndex * itemsQuantity).
                SetMaxResults(itemsQuantity).
                ListAsync<NewsItem>();

                return newsItemsList;
            }
        }

        public async Task<int> GetNewsItemsQuantity()
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.QueryOver<NewsItem>().RowCountAsync();
            }
        }
    }
}