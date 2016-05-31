using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class BlogFeedSettings : IHasId
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

        public BlogFeedSettings()
        {

        }
    }
}