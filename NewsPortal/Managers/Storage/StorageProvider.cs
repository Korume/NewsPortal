using NewsPortal.Models.ViewModels;
using System.Collections.Generic;
using System.Web;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Interfaces;
using NewsPortal.Models.ViewModels.News;
using NewsPortal.Managers.LocalMemory;

namespace NewsPortal.Managers.Storage
{
    public enum MemoryMode { Database, LocalStorage };

    public class StorageProvider
    {
        static MemoryMode currentStorageMode;

        static Dictionary<MemoryMode,IStorage> storageManagers=new Dictionary<MemoryMode,IStorage>(2);

        static StorageProvider()
        {
            storageManagers.Add(MemoryMode.Database,new DataBaseManager());
            storageManagers.Add(MemoryMode.LocalStorage,new LocalMemoryManager());
        }

        public static IStorage GetStorage()
        {
            return storageManagers[currentStorageMode];
        }

        public static void SwitchStorage(MemoryMode value)
        {
            currentStorageMode = value;
        }
    }

    public static class Storage
    {
        public static void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            StorageProvider.GetStorage().Edit(editModel, uploadedImage);
        }

        public static void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            StorageProvider.GetStorage().Add(newsModel, uploadedImage, UserId);
        }

        public static void Delete(int id)
        {
            StorageProvider.GetStorage().Delete(id);
        }

        public static HomePageModel GetHomePage(int page = 0, bool sortedByDate = true)
        {
            return StorageProvider.GetStorage().GetHomePage(page, sortedByDate);
        }

        public static NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string UserId)
        {
            return StorageProvider.GetStorage().GetEditedNewsItem(newsItemId, UserId);
        }

        public static NewsItemMainPageViewModel GetMainNews(int id)
        {
            return StorageProvider.GetStorage().GetMainNews(id);
        }
    }
}