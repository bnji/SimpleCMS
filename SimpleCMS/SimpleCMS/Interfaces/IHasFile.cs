using SimpleCMS.Models;
using SimpleCMS.Models.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS
{
    [DataContract]
    public abstract class IHasFile
    {
        [DataMember]
        [ForeignKey("File")]
        public int? FileId { get; set; }
        [DataMember]
        //[Display(Name = "File", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual File File { get; set; }
    }

    [DataContract]
    public abstract class IHasFileWithChangeEvent : IHasChangeEvent
    {
        [DataMember]
        [ForeignKey("File")]
        public int? FileId { get; set; }
        [DataMember]
        //[Display(Name = "File", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual File File { get; set; }
    }

    [DataContract]
    public abstract class IHasFiles
    {
        [DataMember]
        //[Display(Name = "Files", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ICollection<File> Files { get; set; }
    }

    [DataContract]
    public abstract class IHasFilesWithChangeEvent : IHasChangeEvent
    {
        [DataMember]
        //[Display(Name = "Files", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ICollection<File> Files { get; set; }
    }
}
