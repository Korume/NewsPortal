using NewsPortal.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain
{
    public interface ICommentRepository
    {
        Task<int> Add(CommentItem comment);
        Task Update(CommentItem comment);
        Task Delete(CommentItem comment);
        Task<CommentItem> GetById(int commentId);
        Task<IList<CommentItem>> GetCommentsOnNewsPage(int newsItemId);
    }
}
