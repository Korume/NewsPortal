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
        public int Id { set; get; }
        public string Title { set; get; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { set; get; }

        public int? UserId { set; get; }
        public string UserLogin { set; get; }
    }
}