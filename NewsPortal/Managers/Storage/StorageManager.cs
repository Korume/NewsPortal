﻿using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Interfaces;

namespace NewsPortal.Managers.Storage
{

   public enum MemMode { Database=0, LocalStorage=1 };

    static class MemoryMode
    {
        public static MemMode CurrentMemoryMode;
        public static List<StorageProvider> list = new List<StorageProvider>();
        static MemoryMode() { 
         CurrentMemoryMode = MemMode.Database;
            list.Add(new NhibernateShortenedManager());
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
    }
}