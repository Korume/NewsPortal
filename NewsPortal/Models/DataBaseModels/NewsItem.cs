using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
using System.Linq;
using System.Web;
>>>>>>> Nata

namespace NewsPortal.Models.DataBaseModels
{
    public class NewsItem
    {
        public virtual int Id { set; get; }
        public virtual int UserId { set; get; }
<<<<<<< HEAD
        public virtual string Title { set; get; }
        public virtual string Content { set; get; }
=======

        public virtual string Title { set; get; }

        public virtual string Content { set; get; }

        public virtual string SourceImage { set; get; }

>>>>>>> Nata
        public virtual DateTime CreationDate { set; get; }
    }
}