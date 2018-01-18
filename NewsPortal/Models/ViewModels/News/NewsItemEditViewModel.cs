using System.ComponentModel.DataAnnotations;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemEditViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Title")]
        [DataType(DataType.MultilineText)]
        public string Title { set; get; }

        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [DataType(DataType.ImageUrl)]
        public string SourceImage { set; get; }
    }
}