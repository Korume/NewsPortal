using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.ViewModels.Partial
{
    public class DialogViewModel
    {
        public int Id { set; get; }
        public string Title { set; get; }

        public bool Agreement { set; get; }
    }
}