using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using SimpleCMS.Models.Blog;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace SimpleCMS.ViewModels
{
    public class BlogItemContentViewModel : ViewModelBase
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        [Display(Name = "Excerpt", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Excerpt { get; set; }

        //[Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Keywords { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        public int? PageContentInfoId { get; set; }
        //[Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentInfo PageContentInfo { get; set; }

        public BlogItemContentViewModel()
        {

        }
    }

    public class BlogItemContentCreateOrEditViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Display(Name = "Index", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Index { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public ViewTemplate ViewTemplate { get; set; }

        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? ViewTemplateId { get; set; }

        [Display(Name = "ContentTranslation", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public ContentTranslation ContentTranslation { get; set; }

        [Display(Name = "ContentTranslation" , ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int? ContentTranslationId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Title { get; set; }

        //[Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string BodyHtml { get; set; }

        [AllowHtml]
        [Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Body { get; set; }

        [Display(Name = "Excerpt", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Excerpt { get; set; }

        //[Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Keywords { get; set; }

        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [Display(Name = "HasRSS", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool HasRSS { get; set; }

        [Display(Name = "IsDraft", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsDraft { get; set; }

        [Display(Name = "IsPublished", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPublished { get; set; }

        [Display(Name = "PublishedBy", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PublishedBy { get; set; }

        [Display(Name = "PublishedOn", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? PublishedOn { get; set; }

        [Display(Name = "ActiveFrom", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveFrom { get; set; }

        [Display(Name = "ActiveTo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public DateTime? ActiveTo { get; set; }

        [Display(Name = "Images", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(@SimpleCMS.Resources.Resources))]
        public IEnumerable<FileUploadInfo> Files { get; set; }

        [Display(Name = "Images", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ICollection<PageContentFile> PageContentFiles { get; set; }

        public BlogItemContentCreateOrEditViewModel()
        {
            Files = new List<FileUploadInfo>();
            PageContentFiles = new List<PageContentFile>();
        }
    }

    [DataContract]
    public class CustomContentImagesViewModel : ViewModelBase
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int? PageContentId { get; set; }

        //[DataMember]
        //public ImageInfo ImageInfo { get; set; }

        [DataMember]
        public int ImageInfoId { get; set; }

        [DataMember]
        [Display(Name = "Width", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Width { get; set; }

        [DataMember]
        [Display(Name = "Height", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Height { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Description { get; set; }

        [DataMember]
        [Display(Name = "File", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int FileId { get; set; }

        [DataMember]
        [Display(Name = "Name", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }

        [DataMember]
        [Display(Name = "ContentType", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string ContentType { get; set; }

        [DataMember]
        [Display(Name = "Length", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public long Length { get; set; }

        public CustomContentImagesViewModel()
        {
            
        }
    }
}