<<<<<<< HEAD
﻿using System.Collections.Generic;
using System.Web.Mvc;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
>>>>>>> Nata
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;

namespace NewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
<<<<<<< HEAD
            List<NewsItemThumbnailViewModel> thumbnails = GetThumbnails();
            return View(thumbnails);
        }

        public ActionResult Sort()
        {
            var thumbnails = GetThumbnails();
            thumbnails.Sort((x, y) => y.CreationDate.CompareTo(x.CreationDate));
            return View("Index", thumbnails);
        }

        private List<NewsItemThumbnailViewModel> GetThumbnails()
        {
=======
            var thumbnails = GetThumbnails();
            return View(thumbnails);
        }
        private IList<NewsItemViewModel> GetThumbnails()
        {
            List<NewsItemViewModel> thumbnails;
>>>>>>> Nata
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var newsItemList = session.QueryOver<NewsItem>().List();
<<<<<<< HEAD
                var thumbnails = new List<NewsItemThumbnailViewModel>(newsItemList.Count);

                foreach (var item in newsItemList)
                {
                    var user = session.Get<User>(item.UserId);

                    thumbnails.Add(new NewsItemThumbnailViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        UserId = item.UserId,
                        CreationDate = item.CreationDate,
                        UserLogin = user.Login
                    });
                }

                transaction.Commit();

                return thumbnails;
            }
        }
    }
}
=======
                thumbnails = new List<NewsItemViewModel>(newsItemList.Count);
                foreach (var item in newsItemList)
                {
                    var user = session.Get<User>(item.UserId);
                    thumbnails.Add(new NewsItemViewModel()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        CreationDate = item.CreationDate,
                        UserName = session.Get<User>(item.UserId).UserName
                    });
                }
                transaction.Commit();
            }
            return thumbnails;
        }
    }
}
>>>>>>> Nata
