using NewsPortal.Managers.NHibernate;
using NewsPortal.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NewsPortal.Managers.News
{
    public class NewsManager
    {
        public static bool CheckedNewsItem(int newsItemId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                var newsItem = session.Get<NewsItem>(newsItemId);
                return newsItem != null ? true : false;
            }
        }

        public static string EditNewsTitleForUrl(string title)
        {
            StringBuilder builder = new StringBuilder(title);
            builder.Replace(' ', '-');
            return builder.ToString();
        }
    }
}