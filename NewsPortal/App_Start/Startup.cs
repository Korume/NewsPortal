using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NewsPortal.Account;

[assembly: OwinStartup(typeof(NewsPortal.App_Start.Startup))]

namespace NewsPortal.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //NHibernateHelper
            app.CreatePerOwinContext(() => new UserManager(new NHibernateHelper().Users));
            app.CreatePerOwinContext<SignInManager>((options, context) => 
            new SignInManager(context.GetUserManager<UserManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/IndexAuthorization"), 
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}
