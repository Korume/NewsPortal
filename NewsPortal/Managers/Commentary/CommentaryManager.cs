using NewsPortal.Models.Commentaries;
using System.Collections.Generic;
using System;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Models.DataBaseModels;
using System.Linq;

namespace NewsPortal.Managers.Commentary
{
    public class CommentaryManager
    {
        public static CommentItem ReturnComment(int newsID)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            {
                return session.QueryOver<CommentItem>().Where(u => u.NewsId == newsID).SingleOrDefault();
            }
        }
        public static IList<CommentItem> ReturnCommentaries(int newsID)
        {
            using (var session = NHibernateManager.GetCurrentSession())
            {
                return session.QueryOver<CommentItem>().Where(u => u.NewsId == newsID).List();
            }
        }
    }
}