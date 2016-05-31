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
    public class PageContentTypeViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public PageContentTypeViewModel()
        {

        }
    }

    public class PageContentTypeCreateOrEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public PageContentTypeCreateOrEditViewModel()
        {

        }
    }
}