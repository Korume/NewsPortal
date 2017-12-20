using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.DataBaseModels
{
    public class NewsItem
    {
        public int Id { set; get; }
        public int UserId { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public string SourceImage { set; get; }
        public DateTime CreationDate { set; get; }
    }
}