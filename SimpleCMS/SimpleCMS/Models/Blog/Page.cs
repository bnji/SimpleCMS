using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class Page : IHasChangeEvent, IHasId, IPublishableContent
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Keywords { get; set; }

        [DataMember]
        public string Tags { get; set; }

        [DataMember]
        public virtual ICollection<PageContent> Content { get; set; }

        [DataMember]
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [DataMember]
        public virtual Page Parent { get; set; }

        [DataMember]
        public virtual ICollection<Page> Pages { get; set; }

        [DataMember]
        [ForeignKey("Permalink")]
        //[Required]
        public int? PermalinkId { get; set; }
        [DataMember]
        public virtual Permalink Permalink { get; set; }

        [DataMember]
        public string TemplateName { get; set; } // Default or custom partial view

        [DataMember]
        public bool IncludeInMenu { get; set; }

        [DataMember]
        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [DataMember]
        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [DataMember]
        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [DataMember]
        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }

        [DataMember]
        [Display(Name = "IsRssFeed", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsRssFeed { get; set; }

        public Page()
        {
            Content = new List<PageContent>();
            Pages = new List<Page>();
        }
    }
}
