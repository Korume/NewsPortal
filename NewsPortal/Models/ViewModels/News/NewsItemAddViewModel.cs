using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemAddViewModel
    {
        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Введите заголовок.")]
        [Display(Name = "Заголовок: ")]
        [DataType(DataType.MultilineText)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина строки должна быть от 5 до 50 символов")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Введите содержимое.")]
        [Display(Name = "Содержимое: ")]
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public DateTime CreationDate { set; get; }

        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int? UserId { set; get; }

    }
}
