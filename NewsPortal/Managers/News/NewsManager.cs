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
        public static string EditNewsTitleForUrl(string title)
        {
            StringBuilder builder = new StringBuilder(title);
            builder.Replace(' ', '-');
            return builder.ToString();
        }
    }
}