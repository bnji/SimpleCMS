using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using SimpleCMS.Models.Blog;

namespace SimpleCMS.ViewModels
{
    public class PageContentViewModel : ViewModelBase
    {
        [Display(Name =  "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        [Display(Name = "Excerpt", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Excerpt { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public virtual PageContentInfo PageContentInfo { get; set; }

        public virtual PageViewModel Page { get; set; }

        public PageContentViewModel()
        {

        }
    }
}