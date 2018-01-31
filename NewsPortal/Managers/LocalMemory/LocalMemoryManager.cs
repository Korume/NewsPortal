﻿using System;
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
using NewsPortal.ServiceClasses;

namespace NewsPortal.Managers.LocalMemory
{
    public class LocalMemoryManager : StorageProvider, IStorage
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

        public void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
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

        public NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string UserId)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == newsItemId)
                {
                    bool isUserOwner = news.UserId == Convert.ToInt32(UserId);
                    if (!isUserOwner)
                    {
                        return null;
                    }
                    var editedNewsItem = new NewsItemEditViewModel()
                    {
                        Id = news.Id,
                        Title = news.Title,
                        Content = news.Content,
                        SourceImage = news.SourceImage
                    };
                    return editedNewsItem;
                }
            }
            throw new HttpException(404, "Error 404, bad page");
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

        public HomePageModel GetHomePage(int page, bool sortedByDate)
        {
            int newsItemsQuantity = 15;
            var sortedNews = sortedByDate ? allNews.OrderBy(x => x.CreationDate).ToList() :
                    allNews.OrderByDescending(x => x.CreationDate).ToList();
            var some = sortedNews.Skip(page * newsItemsQuantity).Take(newsItemsQuantity).ToList();
            int lastPage = allNews.Count / (newsItemsQuantity + 1);
            var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);

            foreach (var item in some)
            {
                string userName;
                using (var manager = new NHibernateManager())
                {
                    var session = manager.GetSession();
                    userName = session.Get<User>(item.UserId)?.UserName ?? String.Empty;
                }

                thumbnails.Add(new NewsItemThumbnailViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    UserId = item.UserId,
                    UserName = userName,
                    CreationDate = item.CreationDate
                });
            }
            var homePageModel = new HomePageModel()
            {
                Thumbnails = thumbnails,
                CurrentPageIndex = page,
                SortedByDate = sortedByDate,
                LastPageIndex = lastPage
            };
            return homePageModel;
        }

        public NewsItemMainPageViewModel GetMainNews(int id, string title)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == id)
                {
                    var commentItems = CommentaryManager.ReturnCommentaries(news.Id);
                    var newsUser = NHibernateManager.ReturnDB_User(news.UserId);
                    var showMainNews = new NewsItemMainPageViewModel()
                    {
                        Id = news.Id,
                        Title = news.Title,
                        Content = news.Content,
                        SourceImage = news.SourceImage,
                        CreationDate = news.CreationDate,
                        UserId = news.UserId,
                        UserName = newsUser.UserName,
                        CommentItems = commentItems
                    };
                    return showMainNews;
                }
            }
            throw new HttpException(404, "Error 404, bad page");
        }
    }
}