using NewsPortal.Models.DataBaseModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using NewsPortal.App_Start;

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
            var dataProtectionProvider = Startup.DataProtectionProvider;
            UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
        }

        //public bool CheckedEmailUser(stri)
    }
}