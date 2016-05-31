using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class Country : IHasChangeEvent
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "CountryCode", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Code { get; set; }

        [DataMember]
        [Display(Name = "Country", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Name { get; set; }
    }
}