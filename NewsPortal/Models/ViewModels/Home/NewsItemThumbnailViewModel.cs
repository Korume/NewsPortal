using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemThumbnailViewModel
    {
        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [ScaffoldColumn(false)]
        public int UserId { set; get; }

        [StringLength(400, ErrorMessage = "Max length = 400")]
        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Title")]
        [DataType(DataType.Text)]
        public string Title { set; get; }

        [StringLength(4000, ErrorMessage = "Max length = 4000")]
        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        [ScaffoldColumn(false)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [DataType(DataType.ImageUrl)]
        public string SourceImage { set; get; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { set; get; }
        public object UserName { get; internal set; }
    }
}