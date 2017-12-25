using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using Microsoft.AspNet.Identity;

namespace NewsPortal.Account
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator() { RequiredLength = 6 };
        }
    }
}