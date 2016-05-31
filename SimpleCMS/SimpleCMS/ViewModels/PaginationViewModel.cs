using SimpleCMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleCMS.Models;
using System.Drawing;
using SimpleCMS.Models.Blog;
using System.Runtime.Serialization;

namespace SimpleCMS.ViewModels
{
    [DataContract]
    public class PaginationViewModel<T> : ViewModelBase 
        where T : class
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public IEnumerable<T> List;

        [DataMember]
        public int PerPage { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int BackIndex { get; set; }

        [DataMember]
        public int ForwardIndex { get; set; }

        [DataMember]
        public int From { get; set; }

        [DataMember]
        public int To { get; set; }
    }
}