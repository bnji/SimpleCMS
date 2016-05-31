using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class City
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
