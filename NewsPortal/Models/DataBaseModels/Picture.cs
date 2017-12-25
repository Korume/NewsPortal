using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Models.DataBaseModels
{
    public class Picture
    {
        public virtual int Id { set; get; }
        public virtual string Name { set; get; }
        public virtual byte[] Image { set; get; }
    }
}