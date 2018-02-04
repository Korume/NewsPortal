using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Interfaces;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;

namespace NewsPortal.Managers.LocalMemory
{
    public class LocalMemoryManager : IStorage
    {
        static int id = 0;
        List<NewsItem> allNews = new List<NewsItem>();

        public void Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage, int userId)
        {
            id++;
            allNews.Add(new NewsItem()
            {
                Id = id,
                UserId = userId,
                Title = newsModel.Title,
                Content = newsModel.Content,
                CreationDate = DateTime.Now,
                SourceImage = PictureManager.Upload(uploadedImage, id)
            });
        }

        public void Delete(int id)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == id)
                {
                    allNews.Remove(news);
                }
            }
        }

        public void Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == editModel.Id)
                {
                    news.Title = editModel.Title;
                    news.Content = editModel.Content;
                    news.SourceImage = PictureManager.Upload(uploadedImage, id);
                }
            }
        }

        public NewsItem Get(int id)
        {
            return allNews.SingleOrDefault(n => n.Id == id);
        }

        public IList<NewsItem> GetItems(int pageIndex, int itemsQuantity, bool sortedByDate = true)
        {
            var sortedList = sortedByDate ? allNews.OrderBy(x => x.CreationDate) : allNews.OrderByDescending(x => x.CreationDate);
            var newsItemList = sortedList.Skip(pageIndex * itemsQuantity).Take(itemsQuantity).ToList();
            return newsItemList;
        }

        public int Length()
        {
            return allNews.Capacity;
        }
    }
}