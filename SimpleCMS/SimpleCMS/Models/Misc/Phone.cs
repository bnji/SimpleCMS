using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class Phone : IHasChangeEvent, IHasEntity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Boolean IsPrimary { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public int CountryCode { get; set; }

        [DataMember]
        public int? AreaCode { get; set; }

        [DataMember]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Number { get; set; }

        /// <summary>
        /// The number will be parsed using the 'libphonenumber' parser
        /// https://code.google.com/p/libphonenumber/
        /// </summary>
        [DataMember]
        [Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string RawNumber { get; set; }

        public override string ToString()
        {
            return CountryCode + " " + Number;
        }

        [DataMember]
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        [Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources), ErrorMessageResourceName = "Required")]
        public virtual Entity Entity { get; set; }
    }
}