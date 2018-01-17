using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class RegisterViewModel
    {
        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Enter E-mail address")]
        [Display(Name = "E-mail address:")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required(ErrorMessage = "Enter username:")]
        [Display(Name = "Username:")]
        [DataType(DataType.Text)]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Enter password")]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required(ErrorMessage = "Confirm the password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm the password:")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string ConfirmPassword { set; get; }
    }
}