using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SimpleCMS.Models
{
    [DataContract]
    public partial class Person : IHasChangeEvent, IHasEntity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [ForeignKey("Entity")]
        [DataMember]
        public int? EntityId { get; set; }
        [DataMember]
        public virtual Entity Entity { get; set; }

        [DataMember]
        [Display(Name = "SSN", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string SSN { get; set; }

        [DataMember]
        [Display(Name = "FirstName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //[Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "FirstNameRequired")]
        //[MaxLength(50, ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "FirstNameValidLength")]
        //[MinLength(3, ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "FirstNameValidLength")]
        public string FirstName { get; set; }

        [DataMember]
        [Display(Name = "MiddleName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //[MaxLength(50, ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "MiddleNameValidLength")]
        public string MiddleName { get; set; }

        [DataMember]
        [Display(Name = "LastName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //[Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "LastNameRequired")]
        //[MaxLength(50, ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "LastNameValidLength")]
        //[MinLength(3, ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "LastNameValidLength")]
        public string LastName { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        [Display(Name = "Birthday", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [DataMember]
        [Display(Name = "IsAlive", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool IsAlive { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [DataMember]
        public string Gender { get; set; }

        [Display(Name = "Title", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [DataMember]
        public string Title { get; set; }

        public Person()
        {
        
        }
    }
}