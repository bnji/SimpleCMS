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
    public class PluginViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public virtual ICollection<PluginFile> Files { get; set; }
    }

    public class PluginCreateOrEditViewModel : ViewModelBase
    {
        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(@SimpleCMS.Resources.Resources))]
        public IEnumerable<FileUploadInfo> Files { get; set; }

        public PluginCreateOrEditViewModel()
        {
            Files = new List<FileUploadInfo>();
        }
    }
}