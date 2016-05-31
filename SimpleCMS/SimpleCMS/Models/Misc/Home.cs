using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models
{
    [DataContract]
    public class Home : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
    }
}