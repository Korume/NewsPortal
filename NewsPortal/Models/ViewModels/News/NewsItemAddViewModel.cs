using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemAddViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Введите заголовок.")]
        [Display(Name = "Заголовок: ")]
        [DataType(DataType.Text)]
        [MaxLength(250, ErrorMessage = "MaxLength = 250")]
        [MinLength(10, ErrorMessage = "MinLength = 10")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Введите содержимое.")]
        [Display(Name = "Содержимое: ")]
        [DataType(DataType.Text)]
        [MinLength(50, ErrorMessage = "MinLength = 50")]
        public string Content { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public DateTime CreationDate { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int? UserId { set; get; }

    }
}
