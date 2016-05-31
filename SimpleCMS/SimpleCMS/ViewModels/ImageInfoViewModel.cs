using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using System.Runtime.Serialization;
using SimpleCMS.Models.Blog;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class ImageInfoViewModel : ViewModelBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public long Length { get; set; }
    }

    [DataContract]
    public class ImageInfoCreateViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Images", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public IEnumerable<FileUploadInfo> Files { get; set; }
    }

    [DataContract]
    public class ImageInfoEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        //[DataMember]
        //[Display(Name = "Width", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public int Width { get; set; }

        //[DataMember]
        //[Display(Name = "Height", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public int Height { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        //[Display(Name = "Image", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public IEnumerable<FileUploadInfo> Images { get; set; }
    }
}