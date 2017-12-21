using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.Models
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Login { set; get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }


    }
}