using Newtonsoft.Json;
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
    public class PageContent : IHasChangeEvent, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "Index", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Index { get; set; }

        //[DataMember]
        //public string ViewName { get; set; }

        [DataMember]
        [ForeignKey("ViewTemplate")]
        public int? ViewTemplateId { get; set; }
        [DataMember]
        [Display(Name = "ViewTemplate", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ViewTemplate ViewTemplate { get; set; }

        [DataMember]
        [ForeignKey("Page")]
        public int? PageId { get; set; }
        //[DataMember]
        [JsonIgnore]
        [Display(Name = "Page", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual Page Page { get; set; }

        [DataMember]
        [ForeignKey("PageContentType")]
        public int? PageContentTypeId { get; set; }
        [DataMember]
        [Display(Name = "PageContentType", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentType PageContentType { get; set; }


        //public PageContent()
        //    : base()
        //{

        //}
        
        [DataMember]
        [NotMapped]
        public virtual Type PageType { get; private set; }

        //[DataMember]
        //[ForeignKey("ImageContent")]
        //public int? ImageContentId { get; set; }
        //[DataMember]
        //[Display(Name = "ImageContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual ImageContent ImageContent { get; set; }

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

        [DataMember]
        [NotMapped]
        public PageContentInfo PageContentInfo
        {
            get
            {
                if (BlogItemContent != null)
                {
                    return BlogItemContent.PageContentInfo;
                }
                //if (ImageContent != null)
                //{
                //    return ImageContent.PageContentInfo;
                //}
                //else if (BlogItemContent != null)
                //{
                //    return BlogItemContent.PageContentInfo;
                //}
                else if (PageItemContent != null)
                {
                    return PageItemContent.PageContentInfo;
                }
                return null;
            }
        }

        public PageContent()
        {
            PageType = null;
            if (BlogItemContent != null)
            {
                PageType = BlogItemContent.GetType();
            }
            //if (ImageContent != null)
            //{
            //    PageType = ImageContent.GetType();
            //}
            //if (BlogItemContent != null)
            //{
            //    PageType = BlogItemContent.GetType();
            //}
            if (PageItemContent != null)
            {
                PageType = PageItemContent.GetType();
            }
            //if (PageType == null)
            //{
            //    throw new Exception("PageContent Type is missing");
            //}
        }

        //[DataMember]
        //[ForeignKey("ImageContent")]
        //public int? ImageContentId { get; set; }
        //[DataMember]
        //[Display(Name = "ImageContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual ImageContent ImageContent { get; set; }

        //[DataMember]
        //[ForeignKey("BlogItemContent")]
        //public int? BlogItemContentId { get; set; }
        //[DataMember]
        //[Display(Name = "BlogItemContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual BlogItemContent BlogItemContent { get; set; }

        //[DataMember]
        //[ForeignKey("PageItemContent")]
        //public int? PageItemContentId { get; set; }
        //[DataMember]
        //[Display(Name = "PageItemContent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual PageItemContent PageItemContent { get; set; }
    }
}
