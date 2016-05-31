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
    public class PageItemContent : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public virtual ICollection<PageItemContentData> ContentData { get; set; }

        //[DataMember]
        //[Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Title { get; set; }

        //[DataMember]
        //[Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Body { get; set; }

        //[DataMember]
        //[Display(Name = "Keywords", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Keywords { get; set; }

        //[DataMember]
        //[Display(Name = "Description", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Description { get; set; }

        [DataMember]
        [ForeignKey("PageContentInfo")]
        public int? PageContentInfoId { get; set; }
        [DataMember]
        [Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentInfo PageContentInfo { get; set; }

        public PageItemContent()
        {
            ContentData = new List<PageItemContentData>();
            if (ContentData == null)
            {
                ContentData = new List<PageItemContentData>();
            }
        }
    }
}
