using System.Collections.Generic;
using System;
using NewsPortal.Models.DataBaseModels;
using System.Linq;
using NewsPortal.Domain;
using System.Threading.Tasks;

namespace NewsPortal.Managers.Commentary
{
    public class CommentaryManager
    {
        public static async Task<IList<CommentItem>> GetCommentsOnNewsPage(int newsId)
        {
            var commentRepository = UnityConfig.Resolve<ICommentRepository>();
            return await commentRepository.GetCommentsOnNewsPage(newsId);
        }

        public static async Task<int> SaveComment(string content, int userId, int newsId, string userName)
        {
            var commentItem = new CommentItem()
            {
                Content = content,
                Timestamp = DateTime.Now,
                UserId = userId,
                UserName = userName,
                NewsId = newsId
            };

            var commentRepository = UnityConfig.Resolve<ICommentRepository>();
            var commentId = await commentRepository.Add(commentItem);
            return commentId;
        }

        public static async Task DeleteComment(int commentId)
        {
            var commentRepository = UnityConfig.Resolve<ICommentRepository>();
            var newsItem = await commentRepository.GetById(commentId);
            await commentRepository.Delete(newsItem);
        }
    }
}