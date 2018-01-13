using System;

namespace NewsPortal.Models.ViewModels.News
{
    public class NewsItemMainPageViewModel
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Content { set; get; }
        public DateTime CreationDate { set; get; }
        public int UserId { set; get; }
        public string UserName { set; get; }
    }
}