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
        NewsItemEditViewModel GetEdit(int? newsItemId, string UserId);
        void Add(NewsItemAddViewModel newsModel, HttpPostedFileBase uploadedImage, string UserId);
        void Delete(int id);
        HomePageModel GetHomePage(int page = 0, bool sortedByDate = true);
    }
}