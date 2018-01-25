using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NewsPortal.ModelBinders;

namespace NewsPortal.Models.ViewModels
{
    [ModelBinder(typeof(RegisterModelBinder))]
    public class RegisterViewModel
    {
        [ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Enter E-mail address")]
        [Display(Name = "E-mail address:")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^\p{IsCyrillic}]+$", ErrorMessage = "Don't use Cyrillic characters")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Enter username")]
        [Display(Name = "Username:")]
        [DataType(DataType.Text)]
        [RegularExpression("^[A-Za-z0-9@_]+$", ErrorMessage = "You can use these symbols: a-z A-Z 0-9 @ _")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Enter password")]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { set; get; }

        [Required(ErrorMessage = "Confirm the password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm the password:")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { set; get; }
    }
}