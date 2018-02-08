using NewsPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using System.Threading.Tasks;
using NewsPortal.NHibernate;

namespace NewsPortal.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        public async Task<int> Add(CommentItem comment)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var id = (int)await session.SaveAsync(comment);
                await transaction.CommitAsync();
                return id;
            }
        }

        public async Task Delete(CommentItem comment)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.DeleteAsync(comment);
                await transaction.CommitAsync();
            }
        }

        public async Task<CommentItem> GetById(int commentId)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.GetAsync<CommentItem>(commentId);
            }
        }

        public async Task<IList<CommentItem>> GetCommentsOnNewsPage(int newsItemId)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.QueryOver<CommentItem>().Where(u => u.NewsId == newsItemId).ListAsync();
            }
        }

        public async Task Update(CommentItem comment)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.UpdateAsync(comment);
                await transaction.CommitAsync();
            }
        }
    }
}