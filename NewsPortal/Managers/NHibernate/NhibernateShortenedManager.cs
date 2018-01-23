using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System;
using System.Web;
using NewsPortal.Interfaces;
using System.Web.Mvc;

namespace NewsPortal.Managers.NHibernate
{
    public class NhibernateShortenedManager:StorageProvider,IStorage
    {
         void IStorage.Edit(NewsItemEditViewModel editModel , HttpPostedFileBase uploadedImage)
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

        void IStorage.Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    NewsItem newsItem = new NewsItem()
                    {
                        UserId = Convert.ToInt32(UserId),
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

        void IStorage.Delete(int id)
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

    }
}