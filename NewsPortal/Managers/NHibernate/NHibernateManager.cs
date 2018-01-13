using Microsoft.AspNet.Identity;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.Identity;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Web;

namespace NewsPortal.Managers.NHibernate
{
    public class NHibernateManager
    {
        //WebConfig
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory _sessionFactory;

        static NHibernateManager()
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
        }

        public static void CloseSession()
        {
            var context = HttpContext.Current;
            var currentSession = context.Items[CurrentSessionKey] as ISession;
            
            if (currentSession == null)
            {
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

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(GetCurrentSession()); }
        }

        public static User ReturnDB_User(int userID)
        {
            return GetCurrentSession().Get<User>(userID);
        }

        public static NewsItem ReturnDB_News(int newsID)
        {
            return GetCurrentSession().Get<NewsItem>(newsID);
        }

        public static CommentItem ReturnDB_Comment(int commentID)
        {
            return GetCurrentSession().Get<CommentItem>(commentID);
        }
    }
}