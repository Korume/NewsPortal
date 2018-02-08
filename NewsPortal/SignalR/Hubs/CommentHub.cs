using System;
using NewsPortal.Managers.Commentary;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace NewsPortal.SignalR.Hubs
{
    public class CommentHub : Hub
    {
        public async Task Send(int newsId, int userId, string comment, string userName)
        {
            var commentId = await CommentaryManager.SaveComment(comment, userId, newsId, userName);
            Clients.All.addNewCommentToPage(commentId, userName, comment, DateTime.Now.ToString());
        }

        public async Task Delete(int commentId)
        {
            await CommentaryManager.DeleteComment(commentId);
            Clients.All.deleteCommentToPage(commentId);
        }
    }
}