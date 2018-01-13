using System;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.DataBaseModels
{
    public class CommentItem
    {
        public virtual int Id { set; get; }
        public virtual int UserId { set; get; }
        public virtual string UserName { set; get; }
        public virtual int NewsId { set; get; }
        public virtual string Content { set; get; }
        public virtual DateTime Timestamp { set; get; } 
    }
}