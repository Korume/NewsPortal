using System;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using Microsoft.AspNet.Identity;
using NewsPortal.Models.DataBaseModels;
<<<<<<< HEAD
=======
using NHibernate.AspNet.Identity.Helpers;
using NHibernate.AspNet.Identity;
using NewsPortal.Models;
>>>>>>> Nata
using NewsPortal.Models.Identity;

namespace NewsPortal
{
<<<<<<< HEAD
    public sealed class NHibernateHelper : IDisposable 
=======
    public sealed class NHibernateHelper
>>>>>>> Nata
    {
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory _sessionFactory;

        static NHibernateHelper()
        {
            _sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            var context = HttpContext.Current;
            var currentSession = context.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
                currentSession = _sessionFactory.OpenSession();
                context.Items[CurrentSessionKey] = currentSession;
            }

            return currentSession;
<<<<<<< HEAD
        }   
=======
        }
>>>>>>> Nata

        public static void CloseSession()
        {
            var context = HttpContext.Current;
            var currentSession = context.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
<<<<<<< HEAD
=======
                // No current session
>>>>>>> Nata
                return;
            }

            currentSession.Close();
            context.Items.Remove(CurrentSessionKey);
        }

        public static void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
            }
        }

<<<<<<< HEAD
=======
        //GetCurrentSession
>>>>>>> Nata
        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(GetCurrentSession()); }
        }
<<<<<<< HEAD

        public void Dispose()
        {
            //нужно реализовать
        }


=======
>>>>>>> Nata
    }
}