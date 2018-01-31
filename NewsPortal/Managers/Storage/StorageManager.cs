using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System.Collections.Generic;
using System.Web;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Interfaces;
using NewsPortal.Managers.LocalMemory;
using NewsPortal.Models.ViewModels.News;

namespace NewsPortal.Managers.Storage
{
    public enum MemMode { Database, LocalStorage };

    public class MemoryMode
    {
        public static MemMode CurrentMemoryMode;
        public static List<StorageProvider> list = new List<StorageProvider>();
        
        static MemoryMode()
        {
            list.Add(new NhibernateShortenedManager());
            list.Add(new LocalMemoryManager());
            CurrentMemoryMode = MemMode.Database;
        }
    }

    public static class StorageManager
    {
        public static int GetMemoryMode()
        {
            return (int)MemoryMode.CurrentMemoryMode;
        }

        public static void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            (MemoryMode.list[GetMemoryMode()] as IStorage).Edit(editModel, uploadedImage);
        }

        public static void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            (MemoryMode.list[GetMemoryMode()] as IStorage).Add(newsModel, uploadedImage, UserId);
        }

        public static void Delete(int id)
        {
            (MemoryMode.list[GetMemoryMode()] as IStorage).Delete(id);
        }

        public static HomePageModel GetHomePage(int page = 0, bool sortedByDate = true)
        {
            return (MemoryMode.list[GetMemoryMode()] as IStorage).GetHomePage(page, sortedByDate);
        }

        public static NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string UserId)
        {
            return (MemoryMode.list[GetMemoryMode()] as IStorage).GetEditedNewsItem(newsItemId, UserId);
        }

        public static NewsItemMainPageViewModel GetMainNews(int id, string title)
        {
            return (MemoryMode.list[GetMemoryMode()] as IStorage).GetMainNews(id, title);
        }
    }
}