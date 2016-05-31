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
    public class ContentTranslationViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Language", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Language { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public ContentTranslationViewModel()
        {

        }
    }

    public class ContentTranslationCreateOrEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Language", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Language { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public ContentTranslationCreateOrEditViewModel()
        {

        }
    }
}