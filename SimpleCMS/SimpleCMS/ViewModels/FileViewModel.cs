using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using System.Runtime.Serialization;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class FileViewModel : ViewModelBaseWithChangeEvent
    {
        public int Id { get; set; }

        [Display(Name =  "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [Display(Name = "ContentType", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string ContentType { get; set; }

        [Display(Name = "Length", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public long Length { get; set; }

        //[Display(Name = "Data", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public byte[] Data { get; set; }
    }

    public class FileCreateOrEditViewModel : ViewModelBaseWithChangeEvent
    {
        [DataMember]
        public FileUploadInfo[] Files { get; set; }
    }
}