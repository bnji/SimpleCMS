using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class Address : IHasChangeEvent, IHasEntity
    {
        [Key]
        [DataMember]
        [Display(Name = "Address",  ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "CareOf", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string CareOf { get; set; }

        [DataMember]
        [Display(Name = "AddressLine1", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string AddressLine1 { get; set; }

        [DataMember]
        [Display(Name = "AddressLine2", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string AddressLine2 { get; set; }

        [DataMember]
        [Display(Name = "IsActive", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsActive { get; set; }

        [DataMember]
        [Display(Name = "IsPrimary", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsPrimary { get; set; }

        [DataMember]
        [Display(Name = "Postal", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [ForeignKey("Postal")]
        public int? PostalId { get; set; }
        [DataMember]
        [Display(Name = "Postal", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual Postal Postal { get; set; }
        
        [DataMember]
        [Display(Name = "StateProvinceRegion", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string StateProvinceRegion { get; set; }
        
        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "Country", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [DataMember]
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        [Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources), ErrorMessageResourceName = "Required")]
        public virtual Entity Entity { get; set; }
    }
}