using NewsPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Interfaces
{
    interface IStorage
    {
        void Edit(NewsItemEditViewModel editModel, HttpPostedFileBase uploadedImage);
        void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage);
        void Delete(object obj);
    }
}