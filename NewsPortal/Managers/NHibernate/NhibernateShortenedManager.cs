using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System;
using System.Web;
using NewsPortal.Interfaces;
using System.Web.Mvc;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace NewsPortal.Managers.NHibernate
{
    public class NhibernateShortenedManager : StorageProvider, IStorage
    {
        public void Edit(NewsItemEditViewModel editModel , HttpPostedFileBase uploadedImage)
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

        public NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string UserId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                var newsItem = session.Get<NewsItem>(newsItemId);
                if (newsItem == null)
                {
                    throw new HttpException(404, "Error 404, bad page");
                }

                bool isUserNewsItemOwner = newsItem.UserId == Convert.ToInt32(UserId);
                if (!isUserNewsItemOwner)
                {
                    return null;
                }
            
                var editedNewsItem = new NewsItemEditViewModel()
                {
                    Id = newsItem.Id,
                    Title = newsItem.Title,
                    Content = newsItem.Content,
                    SourceImage = newsItem.SourceImage
                };
                return editedNewsItem;
            }
        }

        public void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
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
            
        public HomePageModel GetHomePage(int page, bool sortedByDate)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                int newsItemsQuantity = 15;
                var lastPage = (int)Math.Ceiling(session.QueryOver<NewsItem>().RowCount() / (double)newsItemsQuantity) - 1;
                if (page < 0 || page > lastPage)
                {
                    throw new HttpException(404, "Error 404, bad page");
                }

                var propertyForOrder = "CreationDate";
                var orderType = sortedByDate ? Order.Desc(propertyForOrder) : Order.Asc(propertyForOrder);
                var newsItemList = session.CreateCriteria<NewsItem>().
                    AddOrder(orderType).
                    SetFirstResult(page * newsItemsQuantity).
                    SetMaxResults(newsItemsQuantity).
                    List<NewsItem>();

                var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);
                foreach (var item in newsItemList)
                {
                    var userName = session.Get<User>(item.UserId)?.UserName ?? String.Empty;

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserName = userName
                    });
                }
                var homePageModel = new HomePageModel()
                {
                    Thumbnails = thumbnails,
                    CurrentPage = page,
                    SortedByDate = sortedByDate,
                    LastPage = lastPage
                };
                return homePageModel;
            }
        }
    }
}