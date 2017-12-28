using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
<<<<<<< HEAD
            return View();
=======
            var thumbnails = GetThumbnails();
            return View(thumbnails);
        }
        public ActionResult Sort()
        {
            var thumbnails = GetThumbnails().Reverse();
            return View("Index", thumbnails);
        }
        private IList<NewsItemVM> GetThumbnails()
        {
            var session = NHibernateHelper.GetCurrentSession();
            IList<NewsItemVM> thumbnails = new List<NewsItemVM>(20);
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    var newsItemList = session.QueryOver<NewsItem>().List();
                    for (int i = 0; i < newsItemList.Count; i++)
                    {
                        thumbnails.Add(new NewsItemVM()
                        {
                            Id = newsItemList[i].Id,
                            Title = newsItemList[i].Title,
                            UserId = newsItemList[i].UserId,
                            CreationDate = newsItemList[i].CreationDate,
                            User = new User()
                            {
                                Email = newsItemList[i].User.Email,
                                Login = newsItemList[i].User.Login,
                                Password = newsItemList[i].User.Password
                            }
                        });
                    }
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
            return thumbnails;
>>>>>>> 45fc478... Обновление дизайна
        }
    }
}