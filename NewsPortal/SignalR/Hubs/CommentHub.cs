using System;
using NewsPortal.Managers.Commentary;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace NewsPortal.SignalR.Hubs
{
    public class CommentHub : Hub
    {
        public void Send(int newsId, int userId, string comment, string userName)
        {
            var commentId = CommentaryManager.SaveComment(comment, userId, newsId, userName);
            Clients.All.addNewCommentToPage(commentId, userName, comment, DateTime.Now.ToString());
        }

        public void Delete(int commentId)
        {
            CommentaryManager.DeleteComment(commentId);
            Clients.All.deleteCommentToPage(commentId);
        }
    }
}