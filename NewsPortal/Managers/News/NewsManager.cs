using NewsPortal.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NewsPortal.Managers.News
{
    public class NewsManager
    {
        public static string EditNewsTitleForUrl(string title)
        {
            string titleForEdit = Regex.Replace(title, @"(^\W+|\W+$)", "").Trim();
            return Regex.Replace(titleForEdit, @"\W+", "-");
        }
    }
}