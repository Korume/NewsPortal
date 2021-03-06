﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NewsPortal.Managers.Identity;
using Microsoft.Owin.Security.DataProtection;
using NewsPortal.Models.Identity;
using NewsPortal.NHibernate;

[assembly: OwinStartup(typeof(NewsPortal.App_Start.Startup))]

namespace NewsPortal.App_Start
{
    public class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.CreatePerOwinContext(() => new UserManager(new IdentityStore(SessionManager.OpenSession())));
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
