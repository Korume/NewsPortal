using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels
{
    public class HomePageModel
    {
        public IEnumerable<NewsItemThumbnailViewModel> Thumbnails { set; get; }
        public int CurrentPageIndex { set; get; }
        public bool SortedByDate { set; get; }
        public int LastPageIndex { set; get; }
    }
}