using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using System.Runtime.Serialization;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class PageContentFileViewModel
    {
        [DataMember]
        public int PageContentId { get; set; }

        [DataMember]
        public FileUploadInfo[] Files { get; set; }
    }
}