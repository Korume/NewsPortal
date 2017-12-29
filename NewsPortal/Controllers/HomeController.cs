using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var thumbnails = GetThumbnails();
            return View(thumbnails);
        }
        public ActionResult Sort()
        {
            var thumbnails = GetThumbnails().Reverse();
            return View("Index", thumbnails);
        }
        private IList<NewsItemViewModel> GetThumbnails()
        {
            var session = NHibernateHelper.GetCurrentSession();
            IList<NewsItemViewModel> thumbnails = new List<NewsItemViewModel>(20);
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    var newsItemList = session.QueryOver<NewsItem>().List();
                    for (int i = 0; i < newsItemList.Count; i++)
                    {
                        thumbnails.Add(new NewsItemViewModel()
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
        }
    }
}