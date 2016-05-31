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
    public class PageContentInfo : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "HasRSS", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool HasRSS { get; set; }

        [DataMember]
        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [DataMember]
        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [DataMember]
        [Display(Name = "PublishedBy", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PublishedBy { get; set; }

        [DataMember]
        [Display(Name = "PublishedOn", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? PublishedOn { get; set; }

        [DataMember]
        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [DataMember]
        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }
    }
}
