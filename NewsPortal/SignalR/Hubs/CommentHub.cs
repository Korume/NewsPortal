using Microsoft.AspNet.SignalR;
using NewsPortal.Models.DataBaseModels;
using System;
using NewsPortal.Managers.Commentary;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.SignalR.Hubs
{
    public class CommentHub : Hub
    {
        public void Send(int newsId, int userId, string comment, string userName)
        {
            var session = NHibernateManager.GetCurrentSession();
            var commentItem = new CommentItem()
            {
                Content = comment,
                Timestamp = DateTime.Now,
                UserId = userId,
                UserName = userName,
                NewsId = newsId
            };
            session.Save(commentItem);
            Clients.All.addNewCommentToPage(commentItem.Id, userName, comment, DateTime.Now.ToString());
        }
        public void Delete(int commentId)
        {
            var session = NHibernateManager.GetCurrentSession();
            var transaction = session.BeginTransaction();

            var MyNewsItem = session.Get<CommentItem>(commentId);
            Clients.All.deleteCommentToPage(commentId);
            session.Delete(MyNewsItem);
            transaction.Commit();

        }
    }
}