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
    public class BlogItemContent : IHasPageContentFiles, IHasContentData<BlogItemContentData>, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public virtual ICollection<BlogItemContentData> ContentData { get; set; }

        //[DataMember]
        //[Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Title { get; set; }

        //[DataMember]
        //[Display(Name = "Body", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Body { get; set; }

        //[DataMember]
        //[Display(Name = "Excerpt", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public string Excerpt { get; set; }

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

        //[DataMember]
        //public virtual ICollection<PageContentFile> Files { get; set; }

        //[DataMember]
        //[ForeignKey("Permalink")]
        //public int? PermalinkId { get; set; }
        //[DataMember]
        //[Display(Name = "Permalink", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //public virtual Permalink Permalink { get; set; }

        //[DataMember]
        //public string Controller { get; set; }

        public BlogItemContent()
        {
            Files = new List<PageContentFile>();
            if (ContentData == null)
            {
                ContentData = new List<BlogItemContentData>();
            }
        }
    }
}
