using NewsPortal.Models.ViewModels;
using System.Collections.Generic;
using System.Web;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Interfaces;
using NewsPortal.Managers.LocalMemory;
using NewsPortal.Models.ViewModels.News;

namespace NewsPortal.Managers.Storage
{
   public enum MemoryMode { Database, LocalStorage };

    static public class MemorySwticher
    {
        public static MemoryMode currentMemoryMode;
        public static List<IStorage> memorySelectionList = new List<IStorage>();

        public static void MemorySwitch(MemoryMode value)
        {
            switch (value)
            {
                case MemoryMode.Database:
                {
                   currentMemoryMode = MemoryMode.Database;
                   break;
                }
                case MemoryMode.LocalStorage:
                {
                   currentMemoryMode = MemoryMode.LocalStorage;
                   break;
                }
            }
        }

        static MemorySwticher()
        {
            memorySelectionList.Add(new NhibernateShortenedManager());
            memorySelectionList.Add(new LocalMemoryManager());
        }
    }

    public static class StorageProvider
    {

        public static void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).Edit(editModel, uploadedImage);
        }

        public static void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId)
        {
            (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).Add(newsModel, uploadedImage, UserId);
        }

        public static void Delete(int id)
        {
            (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).Delete(id);
        }

        public static HomePageModel GetHomePage(int page = 0, bool sortedByDate = true)
        {
            return (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).GetHomePage(page, sortedByDate);
        }

        public static HomePageModel GetCheckedToggle(bool toggleCheck = true)
        {
            return new HomePageModel() { CheckedToggle = toggleCheck };
        }

        public static NewsItemEditViewModel GetEditedNewsItem(int? newsItemId, string UserId)
        {
            return (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).GetEditedNewsItem(newsItemId, UserId);
        }

        public static NewsItemMainPageViewModel GetMainNews(int id)
        {
            return (MemoryMode.memorySelectionList[GetMemoryMode()] as IStorage).GetMainNews(id);
        }
    }
}