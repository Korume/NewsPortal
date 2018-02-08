using NewsPortal.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Domain
{
    public interface IUserRepository
    {
        Task<int> Add(User user);
        Task Update(User user);
        Task Delete(User user);
        Task<User> GetById(int userId);
        Task<User> GetByName(string userName);
        Task<User> GetByEmail(string userEmail);
    }
}
