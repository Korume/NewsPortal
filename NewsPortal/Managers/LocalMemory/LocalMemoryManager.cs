using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Interfaces;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Managers.Picture;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;

namespace NewsPortal.Managers.LocalMemory
{
    public class LocalMemoryManager : StorageProvider, IStorage
    {
        static int id = 0;
        List<NewsItemAddViewModel> allNews = new List<NewsItemAddViewModel>();
        void IStorage.Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            id++;
            allNews.Add(new NewsItemAddViewModel()
            {
                Id = id,
                UserId = Convert.ToInt32(UserId),
                UserName = newsModel.UserName,
                Title = newsModel.Title,
                Content = newsModel.Content,
                CreationDate = DateTime.Now,
                SourceImage = PictureManager.Upload(uploadedImage, id)
            });
        }

        void IStorage.Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == editModel.Id)
                {
                    int index = allNews.IndexOf(news);
                    allNews[index].Title = editModel.Title;
                    allNews[index].Content = editModel.Content;
                    if (uploadedImage != null)
                    {
                        allNews[index].SourceImage = PictureManager.Upload(uploadedImage, id);
                    }
                }
            }
        }

        NewsItemEditViewModel IStorage.GetEdit(int? newsItemId, string UserId)
        {
            NewsItemEditViewModel editedNewsItem = null;
            foreach (var news in allNews.ToList())
            {
                if (news.Id == newsItemId)
                {
                    int index = allNews.IndexOf(news);
                    bool isUserNewsItemOwner = allNews[index].UserId == Convert.ToInt32(UserId);
                    if (!isUserNewsItemOwner)
                    {
                        return null;
                    }
                    editedNewsItem = new NewsItemEditViewModel()
                    {
                        Id = allNews[index].Id,
                        Title = allNews[index].Title,
                        Content = allNews[index].Content,
                        SourceImage = allNews[index].SourceImage
                    };
                }
                else
                {
                    throw new HttpException(404, "Error 404, bad page");
                }
            }
            return editedNewsItem;
        }

        void IStorage.Delete(int id)
        {
            foreach (var news in allNews.ToList())
            {
                if (news.Id == id)
                {
                    allNews.RemoveAt(allNews.IndexOf(news));
                }
            }
        }

        HomePageModel IStorage.GetHomePage(int page, bool sortedByDate)
        {
            int newsItemsQuantity = 15;
            var sortedNews = sortedByDate ? allNews.OrderBy(x => x.CreationDate).ToList() :
                    allNews.OrderByDescending(x => x.CreationDate).ToList();
            //var some = allNews.GetRange(page * newsItemsQuantity, newsItemsQuantity);
            var lastPage = (int)allNews.Count / newsItemsQuantity;
            if (page < 0 || page > lastPage)
            {
                throw new HttpException(404, "Error 404, bad page");
            }
            var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemsQuantity);

            foreach (var item in sortedNews)
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
                CurrentPage = page,
                SortedByDate = sortedByDate,
                LastPage = lastPage
            };
            return homePageModel;
        }
    }
}