using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class BlogWebsiteSettings : IHasId
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
        [Required]
        [ForeignKey("DefaultLanguage")]
        public int? DefaultLanguageId { get; set; }
        [DataMember]
        [Display(Name = "DefaultLanguage", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ContentTranslation DefaultLanguage { get; set; }
        
        public BlogWebsiteSettings()
        {

        }
    }
}