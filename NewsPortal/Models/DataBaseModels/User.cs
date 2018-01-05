using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace NewsPortal.Models.DataBaseModels
{
    public class User : IUser<int>
    {
        public virtual int Id { get; protected set; }
        public virtual string UserName { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { set; get; }
        public virtual bool EmailConfirmed { set; get; }
        public virtual string Password { get; set; }
        private IList<NewsItem> NewsItems { set; get; }
    }
}