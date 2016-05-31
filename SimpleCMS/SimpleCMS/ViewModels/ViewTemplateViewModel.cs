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
    public class ViewTemplateViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        public FileUploadInfo[] Files { get; set; }

        [DataMember]
        public PageContentType PageContentType { get; set; }
    }

    public class ViewTemplateCreateOrEditViewModel : ViewModelBase
    {
        [Display(Name = "PageContentType", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? PageContentTypeId { get; set; }

        [Display(Name = "PageContentType", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PageContentTypeName { get; set; }

        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(@SimpleCMS.Resources.Resources))]
        public IEnumerable<FileUploadInfo> Files { get; set; }
    }
}