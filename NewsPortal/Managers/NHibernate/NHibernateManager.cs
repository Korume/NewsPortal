using NewsPortal.Models.DataBaseModels;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Managers.NHibernate
{
    public static class NHibernateManager
    {
        public static ISession Session = NHibernateHelper.GetCurrentSession();

        public static User ReturnDB_User(int userID)
        {
            return Session.Get<User>(userID);
        }
        public static NewsItem ReturnDB_News(int newsID)
        {
            return Session.Get<NewsItem>(newsID);
        }
        public static CommentItem ReturnDB_Comment(int commentID)
        {
            return Session.Get<CommentItem>(commentID);
        }
    }
}