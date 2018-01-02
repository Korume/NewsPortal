using System;

namespace NewsPortal.Models.DataBaseModels
{
    public class NewsItem
    {
        public virtual int Id { set; get; }
        public virtual int UserId { set; get; }
        public virtual string Title { set; get; }
        public virtual string Content { set; get; }
        public virtual DateTime CreationDate { set; get; }
    }
}