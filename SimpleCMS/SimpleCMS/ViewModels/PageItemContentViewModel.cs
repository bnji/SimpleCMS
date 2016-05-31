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
    public class PageItemContentViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        [Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Keywords { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public int? PageContentInfoId { get; set; }
        //[Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentInfo PageContentInfo { get; set; }

        public PageItemContentViewModel()
        {

        }
    }

    public class PageItemContentCreateOrEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        //[Display(Name = "Index", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public int Index { get; set; }

        [Display(Name = "Index", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Index { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public ViewTemplate ViewTemplate { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? ViewTemplateId { get; set; }

        [Display(Name = "ContentTranslation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public ContentTranslation ContentTranslation { get; set; }

        [Display(Name = "ContentTranslation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? ContentTranslationId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string BodyHtml { get; set; }

        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        //[Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Keywords { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [Display(Name = "HasRSS", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool HasRSS { get; set; }

        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [Display(Name = "PublishedBy", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PublishedBy { get; set; }

        [Display(Name = "PublishedOn", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? PublishedOn { get; set; }

        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }

        public PageItemContentCreateOrEditViewModel()
        {

        }
    }
}