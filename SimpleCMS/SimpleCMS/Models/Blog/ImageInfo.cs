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
    public class ImageInfo : IHasChangeEvent, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Width", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Width { get; set; }
        
        [DataMember]
        [Display(Name = "Height", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Height { get; set; }
        
        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        //[DataMember]
        //[ForeignKey("ImageInfoFile")]
        //public int? ImageInfoFileId { get; set; }
        //[DataMember]
        //[Display(Name = "Image", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual ImageInfoFile ImageInfoFile { get; set; }
        [DataMember]
        [ForeignKey("File")]
        public int? FileId { get; set; }
        [DataMember]
        public virtual File File { get; set; }

        public ImageInfo()
        {

        }
    }
}