using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using FluentNHibernate.Mapping;

namespace NewsPortal.Models.DataBaseModels
{
    public class User : IUser<int>
    {
        //Создаем поля, с помощью которых работаем с базом
        public virtual int Id { get; protected set; }

        public virtual string UserName { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { set; get; }
        
        public virtual string Password { get; set; }
        public virtual string PasswordHash { get; set; }

<<<<<<< HEAD
        //public class Map : ClassMap<User>
        //{
        //    public Map()
        //    {
        //        Id(x => x.Id).GeneratedBy.Identity();
        //        Map(x => x.Login).Not.Nullable();
        //        Map(x => x.Password).Not.Nullable();
        //    }
        //}
=======
        private IList<NewsItem> NewsItems { set; get; }
>>>>>>> 45fc478... Обновление дизайна
    }
}