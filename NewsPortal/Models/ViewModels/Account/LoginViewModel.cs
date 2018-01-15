using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter username")]
        [Display(Name = "Username: ")]
        [DataType(DataType.Text)]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Enter password")]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { set; get; }
    }
}