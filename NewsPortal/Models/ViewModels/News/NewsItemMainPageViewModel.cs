using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels.News
{
    public class NewsItemMainPageViewModel
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
    }
}