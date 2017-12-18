using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models
{
    public class User
    {
        public virtual int Id { set; get; }
        public virtual string Email { set; get; }
        public virtual string Login { set; get; }
        public virtual string Password { set; get; }

        //public string FirstName { set; get; }
        //public string LastName { set; get; }
        //public string PathToAvatar { set; get; }
        //public DateTime DateOfBirth { set; get; }
        //public DateTime RegistrationDate { set; get; }

        // Виды юзеров будут добавлены позже
        // public Status Status { set; get; }
    }
}