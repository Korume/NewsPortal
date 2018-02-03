using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System;
using System.Web;
using NewsPortal.Interfaces;
using System.Collections.Generic;
using NHibernate.Criterion;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.Managers.Storage
{
    public class DataBaseManager :  IStorage
    {
        public void Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage, int userId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    NewsItem newsItem = new NewsItem()
                    {
                        UserId = userId,
                        Title = newsModel.Title,
                        Content = newsModel.Content,
                        CreationDate = DateTime.Now
                    };
                    session.Save(newsItem);
                    newsItem.SourceImage = PictureManager.Upload(uploadedImage, newsItem.Id);
                    session.Update(newsItem);
                    transaction.Commit();
                }
            }
        }

        public void Delete(int id)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    var newsItem = session.Get<NewsItem>(id);
                    PictureManager.Delete(newsItem.SourceImage);
                    session.Delete(newsItem);
                    transaction.Commit();
                }
            }
        }

        public void Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();

                using (var transaction = session.BeginTransaction())
                {
                    var newsItemToUpdate = session.Get<NewsItem>(editModel.Id);
                    newsItemToUpdate.Title = editModel.Title;
                    newsItemToUpdate.Content = editModel.Content;

                    if (uploadedImage != null)
                    {
                        PictureManager.Delete(newsItemToUpdate.SourceImage);
                        newsItemToUpdate.SourceImage = PictureManager.Upload(uploadedImage, editModel.Id);
                    }

                    session.Update(newsItemToUpdate);
                    transaction.Commit();
                }
            }
        }

        public NewsItem Get(int id)
        {
           return NHibernateManager.ReturnDB_News(id);
        }

        public List<NewsItem> GetItems(int firstIndex, int itemsCount, bool sortedByDate=true)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();

                var propertyForOrder = "CreationDate";
                var orderType = sortedByDate ? Order.Desc(propertyForOrder) : Order.Asc(propertyForOrder);
                var newsItemList = session.CreateCriteria<NewsItem>().
                AddOrder(orderType).
                SetFirstResult(firstIndex).
                SetMaxResults(itemsCount).
                List<NewsItem>();

                return newsItemList as List<NewsItem>;
            }
        }

        public int Length()
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                return session.QueryOver<NewsItem>().RowCount();
            }
        }

    }
}