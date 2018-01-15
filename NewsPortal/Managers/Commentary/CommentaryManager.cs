using System.Collections.Generic;
using System;
using NewsPortal.Managers.NHibernate;
using NewsPortal.Models.DataBaseModels;
using System.Linq;

namespace NewsPortal.Managers.Commentary
{
    public class CommentaryManager
    {

        public static int ReturnCommentId(int newsId)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().QueryOver<CommentItem>().Where(u => u.NewsId == newsId).SingleOrDefault().Id;
            }
        }

        public static CommentItem ReturnComment(int newsId)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().QueryOver<CommentItem>().Where(u => u.NewsId == newsId).SingleOrDefault();
            }
        }

        public static IList<CommentItem> ReturnCommentaries(int newsId)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().QueryOver<CommentItem>().Where(u => u.NewsId == newsId).List();
            }
        }

        public static void SaveComment(string content, int userId, int newsId, string userName, ref int commentId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                var commentItem = new CommentItem()
                {
                    Content = content,
                    Timestamp = DateTime.Now,
                    UserId = userId,
                    UserName = userName,
                    NewsId = newsId
                };
                session.Save(commentItem);
                commentId = commentItem.Id;
            }
        }

        public static void DeleteComment(int commentId)
        {
            using (var manager = new NHibernateManager())
            {
                var session = manager.GetSession();
                using(var transaction = session.BeginTransaction())
                {
                    var commentItem = session.Get<CommentItem>(commentId);
                    session.Delete(commentItem);
                    transaction.Commit();
                }
            }
        }
    }
}