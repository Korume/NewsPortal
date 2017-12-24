using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.DataBaseModels
{
    public class User
    {
        public virtual int Id { set; get; }
        public virtual string Email { set; get; }
        public virtual string Login { set; get; }
        public virtual string Password { set; get; }
        public virtual bool EmailConfirmed { set; get; }

        private IList<NewsItem> NewsItems { set; get; }
    }
}