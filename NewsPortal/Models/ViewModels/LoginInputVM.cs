using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class LoginInputVM
    {
        [Display(Name = "Login: ")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The field must be set!")]
        public string Login { set; get; }

        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The field must be set!")]
        public string Password { set; get; }        
    }
}