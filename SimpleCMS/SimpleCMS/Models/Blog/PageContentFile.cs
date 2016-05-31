using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models.Blog
{
    [DataContract]
    public class PageContentFile : IHasChangeEvent, IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        [ForeignKey("PageContent")]
        public int? PageContentId { get; set; }
        [DataMember]
        public virtual PageContent PageContent { get; set; }
        
        [DataMember]
        [ForeignKey("ImageInfo")]
        public int? ImageInfoId { get; set; }
        [DataMember]
        public virtual ImageInfo ImageInfo { get; set; }

    }
}