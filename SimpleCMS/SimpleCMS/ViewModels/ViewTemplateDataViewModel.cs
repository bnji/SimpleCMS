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
    public class ViewTemplateDataViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}