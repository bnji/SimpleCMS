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
    public class CustomContent : IHasPageContentFiles, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public virtual ICollection<CustomContentData> ContentData { get; set; }

        [DataMember]
        [ForeignKey("PageContentInfo")]
        public int? PageContentInfoId { get; set; }
        [DataMember]
        [Display(Name = "PageContentInfo", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual PageContentInfo PageContentInfo { get; set; }

        public CustomContent()
        {
            Files = new List<PageContentFile>();
            if (ContentData == null)
            {
                ContentData = new List<CustomContentData>();
            }
        }
    }
}
