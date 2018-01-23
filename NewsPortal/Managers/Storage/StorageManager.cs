using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using NewsPortal.ServiceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Managers.Storage
{

   public enum MemMode { Database, LocalStorage };

    static class MemoryMode
    {
        public static MemMode CurrentMemoryMode;
        public static List<StorageProvider> list = new List<StorageProvider>();
        static MemoryMode() { 
         CurrentMemoryMode = MemMode.Database;
        }
    }

   
    public static class StorageManager
    {
       public static MemMode GetMemoryMode()
        {
            return MemoryMode.CurrentMemoryMode;
        }
    }
}