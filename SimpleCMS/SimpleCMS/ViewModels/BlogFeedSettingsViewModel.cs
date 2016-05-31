using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class BlogFeedSettingsViewModel : ViewModelBase
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "Uri", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string BaseUri { get; set; }

        [DataMember]
        [Display(Name = "Uri", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string UriName { get; set; }

        public BlogFeedSettingsViewModel()
        {

        }
    }

    [DataContract]
    public class BlogFeedSettingsCreateOrEditViewModel : ViewModelBase
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "Uri", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string BaseUri { get; set; }

        [DataMember]
        [Display(Name = "Uri", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string UriName { get; set; }

        public BlogFeedSettingsCreateOrEditViewModel()
        {

        }
    }
}