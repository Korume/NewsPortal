using NewsPortal.Managers.NHibernate;
using NewsPortal.Managers.Storage;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Managers.Commentary;
using NewsPortal.Models.ViewModels;
using NewsPortal.Models.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Web;


namespace NewsPortal.ModelService
{
    public static class ModelProvider
    {
        static public HomePageModel GetHomePage(int page, bool sortedByDate)
        {
            int newsItemsQuantity = 15;

            var lastPage = (int)Math.Ceiling(StorageProvider.GetStorage().Length() / (double)newsItemsQuantity) - 1;
            if (lastPage == -1)
            {
                lastPage = 0;
            }
            if (page < 0 || page > lastPage)
            {
                throw new HttpException(404, "Error 404, bad page");
            }
            List<NewsItem> newsItemList = StorageProvider.GetStorage().GetItems(page * newsItemsQuantity, newsItemsQuantity, sortedByDate);
            /*
            if (newsItemList == null)
            {
               throw new HttpException(404, "Error 404, bad page");
            }
            */
            var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);

            try
            {
                using (var manager = new NHibernateManager())
                {
                    var session = manager.GetSession();

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
                }
            }
            catch
            {

            }

            var homePageModel = new HomePageModel()
            {
                Thumbnails = thumbnails,
                CurrentPageIndex = page,
                LastPageIndex = lastPage,
                SortedByDate = sortedByDate,

            };

            return homePageModel;
        }

        static public NewsItemMainPageViewModel GetMainNews(int id)
        {
            var newsItem = StorageProvider.GetStorage().Get(id);

            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }

            var newsUser = NHibernateManager.ReturnDB_User(newsItem.UserId);

            var commentItems = CommentaryManager.ReturnCommentaries(id);

            var showMainNews = new NewsItemMainPageViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate,
                UserId = newsItem.UserId,
                UserName = newsUser.UserName,
                CommentItems = commentItems
            };

            return showMainNews;
        }

        static public NewsItemViewModel GetEditedNewsItem(int newsItemId, int userId)
        {
            var newsItem = StorageProvider.GetStorage().Get(newsItemId);
            var newsItemUser = NHibernateManager.ReturnDB_User(newsItem.UserId);

            if (newsItem == null)
            {
                throw new HttpException(404, "Error 404, bad page");
            }
            bool isUserNewsItemOwner = newsItem.UserId == userId;

            if (!isUserNewsItemOwner)
            {
                return null;
            }

            var editedNewsItem = new NewsItemViewModel()
            {
                Id = newsItem.Id,
                Title = newsItem.Title,
                Content = newsItem.Content,
                SourceImage = newsItem.SourceImage,
                CreationDate = newsItem.CreationDate,
                UserName = newsItemUser.UserName
            };

            return editedNewsItem;

        }
    }
}