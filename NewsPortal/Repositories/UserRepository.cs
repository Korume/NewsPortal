using NewsPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.NHibernate;
using System.Threading.Tasks;

namespace NewsPortal.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<int> Add(User user)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var userId = (int)await session.SaveAsync(user);
                await transaction.CommitAsync();
                return userId;
            }
        }

        public async Task Delete(User user)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.DeleteAsync(user);
                await transaction.CommitAsync();
            }
        }

        public async Task<User> GetByEmail(string userEmail)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.QueryOver<User>().Where(u => u.Email == userEmail).SingleOrDefaultAsync();
            }
        }

        public async Task<User> GetById(int userId)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.GetAsync<User>(userId);
            }
        }

        public async Task<User> GetByName(string userName)
        {
            using (var session = SessionManager.OpenSession())
            {
                return await session.QueryOver<User>().Where(u => u.UserName == userName).SingleOrDefaultAsync();
            }
        }

        public async Task Update(User user)
        {
            using (var session = SessionManager.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                await session.UpdateAsync(user);
                await transaction.CommitAsync();
            }
        }
    }
}