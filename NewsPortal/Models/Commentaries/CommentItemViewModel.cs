using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.Commentaries
{
    public class CommentItemViewModel
    {
        [ScaffoldColumn(false)]
        public virtual int Id { set; get; }

        [ScaffoldColumn(false)]       
        public virtual int UserId { set; get; }

        public virtual string UserName { set; get; }

        [ScaffoldColumn(false)]
        public virtual int NewsId { set; get; }

        public virtual string Content { set; get; }

        public virtual DateTime Timestamp { set; get; }
    }
}