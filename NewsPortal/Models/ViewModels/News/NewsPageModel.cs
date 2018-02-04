using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels
{
    public class NewsPageModel
    {
        public IEnumerable<NewsItemThumbnailViewModel> Thumbnails { set; get; }
        public int CurrentPageIndex { set; get; }
        public int LastPageIndex { set; get; }
        public bool SortedByDate { set; get; }
    }
}