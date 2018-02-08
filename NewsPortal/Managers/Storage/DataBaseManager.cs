using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System;
using System.Web;
using NewsPortal.Interfaces;
using System.Collections.Generic;
using NHibernate.Criterion;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Domain;
using NewsPortal.Repositories;
using System.Threading.Tasks;

namespace NewsPortal.Managers.Storage
{
    public class DataBaseManager :  IStorage
    {
        public async Task Add(NewsItem newsItem)
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            await newsItemRepository.Add(newsItem);
        }

        public async Task Delete(int newsItemId)
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            var newsItem = await newsItemRepository.GetById(newsItemId);
            await newsItemRepository.Delete(newsItem);
        }

        public async Task Edit(NewsItem newsItem)
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            await newsItemRepository.Update(newsItem);
        }

        public async Task<NewsItem> Get(int newsItem)
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            return await newsItemRepository.GetById(newsItem);
        }

        public async Task<IEnumerable<NewsItem>> GetPageItems(int pageIndex, int itemsQuantity, bool sortedByDate)
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            return await newsItemRepository.GetPageItems(pageIndex, itemsQuantity, sortedByDate);
        }

        public async Task<int> NewsItemsQuantity()
        {
            var newsItemRepository = UnityConfig.Resolve<INewsItemRepository>();
            return await newsItemRepository.GetNewsItemsQuantity();
        }
    }
}