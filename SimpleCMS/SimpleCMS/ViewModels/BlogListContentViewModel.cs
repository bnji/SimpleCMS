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
    public class BlogListContentViewModel : ViewModelBase
    {
        public int Id { get; set; }

        //public int? PageContentInfoId { get; set; }
        ////[Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual PageContentInfo PageContentInfo { get; set; }

        public BlogListContentViewModel()
        {

        }
    }

    public class BlogListContentCreateOrEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Display(Name = "Index", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Index { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public ViewTemplate ViewTemplate { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? ViewTemplateId { get; set; }

        public BlogListContentCreateOrEditViewModel()
        {

        }
    }
}