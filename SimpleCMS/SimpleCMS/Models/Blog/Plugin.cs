using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class Plugin : IHasId
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
        //[InverseProperty("Art")]
        public virtual ICollection<PluginFile> Files { get; set; }

        public Plugin()
        {
            Files = new List<PluginFile>();
        }

    }
}