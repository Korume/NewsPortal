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
            var commentId = 0;
            CommentaryManager.SaveComment(comment, newsId, userId, userName, ref commentId);
            Clients.All.addNewCommentToPage(commentId, userName, comment, DateTime.Now.ToString());
        }
        public void Delete(int commentId)
        {
            CommentaryManager.DeleteComment(commentId);
            Clients.All.deleteCommentToPage(commentId);
        }
    }
}