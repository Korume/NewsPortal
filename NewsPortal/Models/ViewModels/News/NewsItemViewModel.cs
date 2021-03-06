﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int UserId { set; get; }

        [StringLength(400, ErrorMessage = "Max length = 400")]
        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Title")]
        [DataType(DataType.MultilineText)]
        public string Title { set; get; }

        [StringLength(8000, ErrorMessage = "Max length = 8000")]
        [Required(ErrorMessage = "The field must be set!")]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [DataType(DataType.ImageUrl)]
        public string SourceImage { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public DateTime CreationDate { set; get; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string UserName { get; internal set; }
    }
}