using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System;
using System.Web;
using NewsPortal.Interfaces;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Web.WebPages;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;

using NewsPortal.Managers.News;

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


        void IStorage.Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    NewsItem newsItem = new NewsItem()
                    {
                        UserId = Convert.ToInt32(User),
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

    }
}