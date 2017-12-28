﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NewsPortal.Models.DataBaseModels;

namespace NewsPortal.Models.ViewModels
{
    public class NewsItemVM
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public DateTime CreationDate { set; get; }

        public int? UserId { set; get; }
        public User User { set; get; }
    }
}