using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemEditViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Введите заголовок.")]
        [Display(Name = "Заголовок: ")]
        [DataType(DataType.Text)]
        public string Title { set; get; }

        [Required(ErrorMessage = "Введите содержание.")]
        [Display(Name = "Содержание: ")]
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        // Позже будет картинка
    }
}