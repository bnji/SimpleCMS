using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class EmailAddress : IHasChangeEvent, IHasEntity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "EmailAddress", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public String Address { get; set; }

        [DataMember]
        [Display(Name = "IsVerified", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Boolean IsVerified { get; set; }

        [DataMember]
        [Display(Name = "IsActive", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Boolean IsActive { get; set; }

        [DataMember]
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        [Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources), ErrorMessageResourceName = "Required")]
        public virtual Entity Entity { get; set; }

        [DataMember]
        [Display(Name = "IsPrimary", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPrimary { get; set; }
    }
}