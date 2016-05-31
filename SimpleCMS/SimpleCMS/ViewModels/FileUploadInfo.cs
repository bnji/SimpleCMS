using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models
{
    [DataContract]
    public class FileUploadInfo
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string UID { get; set; }
    }
}