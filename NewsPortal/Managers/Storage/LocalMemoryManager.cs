using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Interfaces;
using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System.Threading.Tasks;

namespace NewsPortal.Managers.LocalMemory
{
    public class LocalMemoryManager : IStorage
    {
        static int id = 0;
        List<NewsItem> allNews = new List<NewsItem>();

        public Task Add(NewsItem newsItem)
        {
            return Task.Run(() =>
            {
                newsItem.Id = ++id;
                allNews.Add(newsItem);
            });
        }

        //public void Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage, int userId)
        //{
        //    id++;
        //    allNews.Add(new NewsItem()
        //    {
        //        Id = id,
        //        UserId = userId,
        //        Title = newsModel.Title,
        //        Content = newsModel.Content,
        //        CreationDate = DateTime.Now,
        //        SourceImage = PictureManager.Upload(uploadedImage, id)
        //    });
        //}

        public Task Delete(int id)
        {
            return Task.Run(() =>
            {
                return allNews.RemoveAll(n => n.Id == id);
            });
        }

        public async Task Edit(NewsItem newsItem)
        {
            await Delete(newsItem.Id);
            allNews.Add(newsItem);
        }

        //public void Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        //{
        //    foreach (var news in allNews.ToList())
        //    {
        //        if (news.Id == editModel.Id)
        //        {
        //            news.Title = editModel.Title;
        //            news.Content = editModel.Content;
        //            news.SourceImage = PictureManager.Upload(uploadedImage, id);
        //        }
        //    }
        //}

        public Task<NewsItem> Get(int id)
        {
            return Task.Run(() =>
            {
                return allNews.SingleOrDefault(n => n.Id == id);
            });
        }

        public Task<IEnumerable<NewsItem>> GetPageItems(int pageIndex, int itemsQuantity, bool sortedByDate = true)
        {
            return Task.Run(() =>
            {
                var sortedList = sortedByDate ? allNews.OrderBy(x => x.CreationDate) : allNews.OrderByDescending(x => x.CreationDate);
                var newsItemList = sortedList.Skip(pageIndex * itemsQuantity).Take(itemsQuantity);
                return newsItemList;
            });
        }

        public Task<int> NewsItemsQuantity()
        {
            return Task.Run(() =>
            {
                return allNews.Count;
            });
        }
    }
}