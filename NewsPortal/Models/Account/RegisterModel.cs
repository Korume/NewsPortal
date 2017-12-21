using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models
{
    public class RegisterModel
    {
        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required]
        [Display(Name = "E-mail адрес: ")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Логин: ")]
        [DataType(DataType.Text)]
        public string Login { set; get; }

        [Required]
        [Display(Name = "Пароль: ")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Пароль (подтверждение): ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { set; get; }
    }
}