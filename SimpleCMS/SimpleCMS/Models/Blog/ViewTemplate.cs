using Newtonsoft.Json;
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
    public class ViewTemplate : IHasId
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
        [ForeignKey("PageContentType")]
        public int? PageContentTypeId { get; set; }
        [DataMember]
        public virtual PageContentType PageContentType { get; set; }

        //[DataMember]
        //[InverseProperty("Art")]
        [JsonIgnore]
        public virtual ICollection<ViewTemplateFile> Files { get; set; }

        public ViewTemplate()
        {
            Files = new List<ViewTemplateFile>();
        }
    }
}