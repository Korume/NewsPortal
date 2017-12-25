using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class LoginInputVM
    {
        [Required(ErrorMessage = "Введите логин.")]
        [Display(Name = "Логин: ")]
        [DataType(DataType.Text)]
        public string Login { set; get; }

        [Required(ErrorMessage = "Введите пароль.")]
        [Display(Name = "Пароль: ")]
        [DataType(DataType.Password)]
        public string Password { set; get; }        
    }
}