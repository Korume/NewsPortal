using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Managers.Storage
{
    public static class Storage
    {
        public static void Edit(NewsItemViewModel editModel, HttpPostedFileBase uploadedImage)
        {
            StorageProvider.GetStorage().Edit(editModel, uploadedImage);
        }

        public static void Add(NewsItemViewModel newsModel, HttpPostedFileBase uploadedImage, int userId)
        {
            StorageProvider.GetStorage().Add(newsModel, uploadedImage, userId);
        }

        public static void Delete(int id)
        {
            StorageProvider.GetStorage().Delete(id);
        }

        public static NewsItem Get(int id)
        {
            return StorageProvider.GetStorage().Get(id);
        }

        public static List<NewsItem> GetItems(int firstIndex, int itemsCount, bool sortedByDate = true)
        {
            return StorageProvider.GetStorage().GetItems(firstIndex, itemsCount, sortedByDate);
        }

        public static int Length()
        {
            return StorageProvider.GetStorage().Length();
        }
    }
}