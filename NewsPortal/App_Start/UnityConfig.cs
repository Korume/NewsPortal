using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsPortal.Domain;
using NewsPortal.Repositories;
using Unity;
using System.Web.Mvc;
using NewsPortal.Services;

namespace NewsPortal
{
    public static class UnityConfig
    {
        public static void BuildUnityContainer()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<INewsItemRepository, NewsItemRepository>().
                RegisterType<IUserRepository, UserRepository>().
                RegisterType<ICommentRepository, CommentRepository>();
        }
    }
}