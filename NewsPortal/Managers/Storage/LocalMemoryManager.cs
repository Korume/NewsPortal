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
    public class LocalMemoryManager :  IStorage
    {
        static int id = 0;
        List<NewsItem> allNews = new List<NewsItem>();

        public void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            id++;
            allNews.Add(new NewsItem()
            {
                Id = id,
                UserId = Convert.ToInt32(UserId),
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

        public void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == editModel.Id)
                {
                    news.Title = editModel.Title;
                    news.Content = editModel.Content;
                    // перенести
                    if (uploadedImage != null)
                    {
                        news.SourceImage = PictureManager.Upload(uploadedImage, id);
                    }
                }
            }
        }

        public NewsItem Get(int id)
        {
            NewsItem article=null;
            foreach (var news in allNews.ToList())
            {
                if (news.Id == id)
                {
                   article=news;
                }
                break;   
            }
            return article;
        }

        public List<NewsItem> GetItems(int firstIndex, int itemsCount, bool sortedByDate = true)
        {
            var sortedNews = sortedByDate ? allNews.OrderBy(x => x.CreationDate).ToList() :
                    allNews.OrderByDescending(x => x.CreationDate).ToList();
 
            List<NewsItem> articleRange=null;
            if (sortedNews.Count > 0)
            {
                try
                {
                    articleRange = sortedNews.GetRange(firstIndex, itemsCount);
                }
                catch
                {
                    articleRange = sortedNews.GetRange(firstIndex, sortedNews.Count-firstIndex);
                }
            }
            return articleRange;

        }

        public int Length()
        {
            return allNews.Capacity;
        }
    }
}