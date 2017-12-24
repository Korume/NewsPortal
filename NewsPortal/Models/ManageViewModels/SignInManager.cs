using System;
using System.Threading.Tasks;
using NewsPortal.Models.DataBaseModels;
using NHibernate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace NewsPortal.Account
{
    public class SignInManager : SignInManager<User, int>
    {
        public SignInManager(UserManager<User, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }
        public void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}