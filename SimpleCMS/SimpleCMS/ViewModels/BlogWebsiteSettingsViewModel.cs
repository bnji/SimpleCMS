using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class BlogWebsiteSettingsViewModel : ViewModelBase
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "PostsPerPage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int PostsPerPage { get; set; }

        [DataMember]
        [Display(Name = "DefaultFrontPage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string DefaultFrontPage { get; set; }

        [DataMember]
        [Display(Name = "DefaultLanguage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string DefaultLanguageDescription { get; set; }
        [DataMember]
        [Required]
        [Display(Name = "DefaultLanguage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int DefaultLanguageId { get; set; }

        public BlogWebsiteSettingsViewModel()
        {

        }
    }

    [DataContract]
    public class BlogWebsiteSettingsCreateOrEditViewModel : ViewModelBase
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "PostsPerPage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int PostsPerPage { get; set; }

        [DataMember]
        [Display(Name = "DefaultFrontPage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string DefaultFrontPage { get; set; }

        [DataMember]
        [Display(Name = "DefaultLanguage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string DefaultLanguage { get; set; }
        [DataMember]
        [Required]
        [Display(Name = "DefaultLanguage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int DefaultLanguageId { get; set; }

        public BlogWebsiteSettingsCreateOrEditViewModel()
        {

        }
    }
}