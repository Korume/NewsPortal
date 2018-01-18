using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NewsPortal.Managers.Identity;
using NewsPortal.Managers.NHibernate;
using Microsoft.Owin.Security.DataProtection;

[assembly: OwinStartup(typeof(NewsPortal.App_Start.Startup))]

namespace NewsPortal.App_Start
{
    public class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
            app.CreatePerOwinContext(() => new UserManager(new NHibernateManager().Users));
            app.CreatePerOwinContext<SignInManager>((options, context) => 
            new SignInManager(context.GetUserManager<UserManager>(), context.Authentication));
            DataProtectionProvider = app.GetDataProtectionProvider();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"), 
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}
