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
    public class ViewTemplateFile : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [ForeignKey("ViewTemplate")]
        public int? ViewTemplateId { get; set; }
        [DataMember]
        public virtual ViewTemplate ViewTemplate { get; set; }
        

        [DataMember]
        [ForeignKey("File")]
        public int? FileId { get; set; }
        [DataMember]
        public virtual File File { get; set; }

    }
}