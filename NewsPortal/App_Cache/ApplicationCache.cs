using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using NewsPortal.Models.DataBaseModels;

namespace NewsPortal.App_Cache
{
    public class ApplicationCache
    {
        private MemoryCache memoryCache;

        public ApplicationCache()
        {
            memoryCache = MemoryCache.Default;
        }

        public NewsItem GetValue(int id)
        {
            return memoryCache.Get(id.ToString()) as NewsItem;
        }

        public bool Add(NewsItem value)
        {
            return memoryCache.Add(value.Id.ToString(), value, DateTime.Now.AddMinutes(10));
        }

        public void Update(NewsItem value)
        {
            memoryCache.Set(value.Id.ToString(), value, DateTime.Now.AddMinutes(10));
        }

        public void Delete(int id)
        {
            if (memoryCache.Contains(id.ToString()))
            {
                memoryCache.Remove(id.ToString());
            }
        }
    }
}