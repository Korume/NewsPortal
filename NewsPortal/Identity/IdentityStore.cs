﻿using System;
using System.Threading.Tasks;
using NewsPortal.Models.DataBaseModels;
using NHibernate;
using Microsoft.AspNet.Identity;
using NewsPortal;

namespace NewsPortal.Models.Identity
{
    public class IdentityStore : IUserStore<User, int>, IUserPasswordStore<User, int>, 
        IUserLockoutStore<User, int>, IUserTwoFactorStore<User, int>, IUserEmailStore<User, int>
    {
        private readonly ISession _session;

        public IdentityStore(ISession session)
        {
            _session = session;
        }
        
        #region IUserStore<User, int>
        public Task CreateAsync(User user)
        {
            return Task.Run(() => _session.SaveOrUpdate(user));
        }
        public Task DeleteAsync(User user)
        {
            return Task.Run(() => _session.Delete(user));
        }
        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Run(() => _session.Get<User>(userId));
        }
        public Task<User> FindByNameAsync(string UserName)
        {
            return Task.Run(() =>
            {     
                return _session.QueryOver<User>().Where(u => u.UserName == UserName).SingleOrDefault();
            });
        }
        public Task UpdateAsync(User user)
        {
            return Task.Run(() => _session.SaveOrUpdate(user));
        }
        #endregion

        #region IUserPasswordStore<User, int>
        public Task SetPasswordHashAsync(User user, string password)
        {            
            return Task.Run(() => user.Password = password);
        }
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password); ;
        }
        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore<User, int>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }
        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }
        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }
        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.CompletedTask;
        }
        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }
        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region IUserTwoFactorStore<User, int>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.CompletedTask;
        }
        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        #endregion

        #region IUserEmailStore<User, int>
        public Task SetEmailAsync(User user, string email)
        {
            return Task.Run(() => user.Email = email);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.Email); ;
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            return Task.Run(() => user.EmailConfirmed = confirmed);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task.Run(() =>
            {
                return _session.QueryOver<User>().Where(u => u.Email == email).SingleOrDefault();
            });
        }
        #endregion

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}