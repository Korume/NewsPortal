using NewsPortal.Models.DataBaseModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace NewsPortal.Managers.Identity
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator() { RequiredLength = 6 };
            EmailService = new EmailService();
            var provider = new DpapiDataProtectionProvider("NewsPortal");
            UserTokenProvider = new DataProtectorTokenProvider<User, int>(provider.Create("ASP.NET Identity"));
        }

        //public bool CheckedEmailUser(stri)
    }
}