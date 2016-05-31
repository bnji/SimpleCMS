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
    public abstract class IHasPageContentWithChangeEvent : IHasChangeEvent
    {
        [DataMember]
        [NotMapped]
        public Type PageType { get; private set; }

        [DataMember]
        [ForeignKey("ImageContent")]
        public int? ImageContentId { get; set; }
        [DataMember]
        [Display(Name = "ImageContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ImageContent ImageContent { get; set; }

        [DataMember]
        [ForeignKey("BlogItemContent")]
        public int? BlogItemContentId { get; set; }
        [DataMember]
        [Display(Name = "BlogItemContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual BlogItemContent BlogItemContent { get; set; }

        [DataMember]
        [ForeignKey("PageItemContent")]
        public int? PageItemContentId { get; set; }
        [DataMember]
        [Display(Name = "PageItemContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageItemContent PageItemContent { get; set; }

        public IHasPageContentWithChangeEvent()
        {
            PageType = null;
            if (ImageContent != null)
            {
                PageType = typeof(ImageContent);
            }
            if (BlogItemContent != null)
            {
                PageType = typeof(BlogItemContent);
            }
            if (PageItemContent != null)
            {
                PageType = typeof(PageItemContent);
            }
            //if (PageType == null)
            //{
            //    throw new Exception("PageContent Type is missing");
            //}
        }
    }

    [DataContract]
    public abstract class IHasPageContentFile
    {
        [DataMember]
        [ForeignKey("File")]
        public int? PageContentFileId { get; set; }
        [DataMember]
        //[Display(Name = "File", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentFile File { get; set; }
    }

    [DataContract]
    public abstract class IHasPageContentFileWithChangeEvent : IHasChangeEvent
    {
        [DataMember]
        [ForeignKey("File")]
        public int? PageContentFileId { get; set; }
        [DataMember]
        //[Display(Name = "File", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentFile File { get; set; }
    }

    [DataContract]
    public abstract class IHasPageContentFiles
    {
        [DataMember]
        //[Display(Name = "Files", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ICollection<PageContentFile> Files { get; set; }
    }

    [DataContract]
    public abstract class IHasPageContentFilesWithChangeEvent : IHasChangeEvent
    {
        [DataMember]
        //[Display(Name = "Files", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ICollection<PageContentFile> Files { get; set; }
    }
}
