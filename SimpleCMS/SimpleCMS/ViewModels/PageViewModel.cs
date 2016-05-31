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
    public class PageViewModel : ViewModelBaseWithChangeEvent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Tags { get; set; }

        //public IEnumerable<PageContent> Content { get; set; }

        public int? ParentId { get; set; }

        //public IEnumerable<Page> Pages { get; set; }

        public int? PermalinkId { get; set; }
        public Permalink Permalink { get; set; }

        public string TemplateName { get; set; } // Default or custom partial view

        public bool IncludeInMenu { get; set; }

        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }

        [Display(Name = "IsRssFeed", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsRssFeed { get; set; }

        //public IEnumerable<PageContentType> PageContentTypes { get; set; }

        public PageViewModel()
        {

        }

        public PageViewModel(Page m)
        {
            Id = m.Id;
            Name = m.Name;
            Title = m.Title;
            Description = m.Description;
            Keywords = m.Keywords;
            Tags = m.Tags;
            //Content = m.Content;
            ParentId = m.Parent != null ? new Nullable<int>(m.Parent.Id) : null;
            //Pages = m.Pages;
            PermalinkId = m.PermalinkId;
            Permalink = m.Permalink;
            TemplateName = m.TemplateName;
            IncludeInMenu = m.IncludeInMenu;
            IsDraft = m.IsDraft;
            IsPublished = m.IsPublished;
            ActiveFrom = m.ActiveFrom;
            ActiveTo = m.ActiveTo;
            IsRssFeed = m.IsRssFeed;
            ChangeEventId = m.ChangeEventId;
            ChangeEvent = m.ChangeEvent;
        }
    }

    public class PageCreateOrEditViewModel : ViewModelBaseWithChangeEvent
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [MinLength(1)]
        public string Name { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Keywords { get; set; }

        [Display(Name = "Tags", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Tags { get; set; }

        //public IEnumerable<PageContent> Content { get; set; }

        public string ParentName { get; set; }
        public int? ParentId { get; set; }
        [Display(Name = "ParentPage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Page Parent { get; set; }

        public int? PermalinkId { get; set; }
        public Permalink Permalink { get; set; }
        [Display(Name = "PermalinkName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(@SimpleCMS.Resources.Blog.Resources))]
        [MinLength(1)]
        public string PermalinkName { get; set; }

        [Display(Name = "TemplateName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string TemplateName { get; set; } // Default or custom partial view

        [Display(Name = "IncludeInMenu", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IncludeInMenu { get; set; }

        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }

        [Display(Name = "IsRssFeed", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsRssFeed { get; set; }
    }
}