using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class Postal : IHasChangeEvent
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public String CityName { get; set; }

        [DataMember]
        public string CountryCode { get; set; }
    }
}