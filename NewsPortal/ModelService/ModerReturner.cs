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
    public static class ModelReturner
    {
        static public HomePageModel GetHomePage(int page, bool sortedByDate)
        {   
            int newsItemsQuantity = 15;

            var lastPage = (int)Math.Ceiling(Storage.Length() / (double)newsItemsQuantity) - 1;
            /*
            if (page < 0 || page > lastPage)
                {
                    throw new HttpException(404, "Error 404, bad page");
                }
             */
            List<NewsItem> newsItemList = Storage.GetItems(page * newsItemsQuantity, newsItemsQuantity, sortedByDate);
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
                CurrentPage = page,
                SortedByDate = sortedByDate,
                LastPage = lastPage
            };

                return homePageModel;
        }

        static public NewsItemMainPageViewModel GetMainNews(int id)
        {
            var newsItem = Storage.Get(id);

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

        static public NewsItemEditViewModel GetEditedNewsItem(int newsItemId, string UserId)
        {
           var newsItem = Storage.Get(newsItemId);

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
}