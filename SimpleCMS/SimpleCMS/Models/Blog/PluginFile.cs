using Newtonsoft.Json;
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
    public class PluginFile : IHasId
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [ForeignKey("Plugin")]
        public int? PluginId { get; set; }
        //[DataMember]
        [JsonIgnore]
        public virtual Plugin Plugin { get; set; }
        

        [DataMember]
        [ForeignKey("File")]
        public int? FileId { get; set; }
        [DataMember]
        public virtual File File { get; set; }

    }
}