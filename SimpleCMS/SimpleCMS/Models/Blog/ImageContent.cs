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
    public class ImageContent : IHasChangeEvent, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public virtual ICollection<ImageContentData> ContentData { get; set; }

        [DataMember]
        [Display(Name = "Image", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Image { get; set; }

        [DataMember]
        [Display(Name = "Width", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Width { get; set; }

        [DataMember]
        [Display(Name = "Height", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Height { get; set; }

        [DataMember]
        [ForeignKey("PageContentInfo")]
        public int? PageContentInfoId { get; set; }
        [DataMember]
        [Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentInfo PageContentInfo { get; set; }

        public ImageContent()
        {
            ContentData = new List<ImageContentData>();
            if (ContentData == null)
            {
                ContentData = new List<ImageContentData>();
            }
        }
    }
}