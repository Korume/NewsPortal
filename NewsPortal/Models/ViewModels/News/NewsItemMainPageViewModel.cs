﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels.News
{
    public class NewsItemMainPageViewModel
    {   
        public int Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        //public string Image { set; get; }
    }
}