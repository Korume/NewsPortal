using Microsoft.AspNet.Identity;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.Identity;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Web;
using System.Data.Common;

namespace NewsPortal.Managers.NHibernate
{
    public class NHibernateManager : IDisposable
    {
        private static ISessionFactory _sessionFactory;
        private static ISession _currentSession;

        public ISession GetSession()
        {
            ISessionFactory factory = getSessionFactory();
            ISession session = GetExistingOrNewSession(factory);
            return session;
        }

        private ISessionFactory getSessionFactory()
        {
            if (_sessionFactory == null)
            {
                Configuration configuration = GetConfiguration();
                _sessionFactory = configuration.BuildSessionFactory();
            }

            return _sessionFactory;
        }

        private Configuration GetConfiguration()
        {
            var configuration = new Configuration();
            configuration.Configure();
            return configuration;
        }

        private ISession GetExistingOrNewSession(ISessionFactory factory)
        {
            if (HttpContext.Current != null)
            {
                ISession session = GetExistingWebSession();
                if (session == null)
                {
                    session = OpenSessionAndAddToContext(factory);
                }
                else if (!session.IsOpen)
                {
                    session = OpenSessionAndAddToContext(factory);
                }

                return session;
            }

            if (_currentSession == null)
            {
                _currentSession = factory.OpenSession();
            }
            else if (!_currentSession.IsOpen)
            {
                _currentSession = factory.OpenSession();
            }

            return _currentSession;
        }

        public ISession GetExistingWebSession()
        {
            return HttpContext.Current.Items[GetType().FullName] as ISession;
        }

        private ISession OpenSessionAndAddToContext(ISessionFactory factory)
        {
            ISession session = factory.OpenSession();
            HttpContext.Current.Items.Remove(GetType().FullName);
            HttpContext.Current.Items.Add(GetType().FullName, session);
            return session;
        }

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(GetSession()); }
        }

        #region Вспомогательные приложения
        public static User ReturnDB_User(int userID)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().Get<User>(userID);
            }
        }

        public static NewsItem ReturnDB_News(int newsID)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().Get<NewsItem>(newsID);
            }
        }

        public static CommentItem ReturnDB_Comment(int commentID)
        {
            using (var manager = new NHibernateManager())
            {
                return manager.GetSession().Get<CommentItem>(commentID);
            }
        }
        #endregion

        public void Dispose()
        {
            //реализовать
        }
    }
}