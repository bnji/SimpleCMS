using SimpleCMS.Models;
using SimpleCMS.Models.Blog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS
{

    public interface IChangeEvent
    {
        int? ChangeEventId { get; set; }
        ChangeEvent ChangeEvent { get; set; }
    }

    [DataContract]
    public abstract class IHasChangeEvent : IChangeEvent
    {
        [ForeignKey("ChangeEvent")]
        [DataMember]
        public int? ChangeEventId { get; set; }
        [DataMember]
        [Display(Name = "ChangeEvent", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual ChangeEvent ChangeEvent { get; set; }

        [NotMapped]
        [DataMember]
        public DateTime? ChangeEventCreatedOn { get { return ChangeEvent != null ? new Nullable<DateTime>(ChangeEvent.CreatedOn) : null; } }

        [NotMapped]
        [DataMember]
        public string ChangeEventCreatedByUserIdCode { get { return ChangeEvent != null ? ChangeEvent.CreatedByUserIdCode : ""; } }

        [NotMapped]
        [DataMember]
        public DateTime? ChangeEventDeletedOn { get { return ChangeEvent != null ? ChangeEvent.DeletedOn : null; } }

        [NotMapped]
        [DataMember]
        public string ChangeEventDeletedByUserIdCode { get { return ChangeEvent != null ? ChangeEvent.DeletedByUserIdCode : ""; } }

        [NotMapped]
        [DataMember]
        public DateTime? ChangeEventUpdatedOn { get { return ChangeEvent != null ? new Nullable<DateTime>(ChangeEvent.UpdatedOn) : null; } }

        [NotMapped]
        [DataMember]
        public string ChangeEventUpdatedByUserIdCode { get { return ChangeEvent != null ? ChangeEvent.UpdatedByUserIdCode : ""; } }

    }

    [DataContract]
    public abstract class IHasStuffWithPersonAndChangeEvent : IHasChangeEvent, IHasStuff, IHasPerson
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [ForeignKey("Person")]
        public int? PersonId { get; set; }
        [Display(Name = "Person", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        //[Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "Required")]
        [DataMember]
        public virtual Person Person { get; set; }

        [NotMapped]
        [DataMember]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Phone PrimaryPhone
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.PrimaryPhone;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "Address", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Address PrimaryAddress
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.PrimaryAddress;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "EmailAddress", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public EmailAddress PrimaryEmailAddress
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.PrimaryEmailAddress;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<Address> Addresses
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.Addresses;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<Phone> Phones
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.Phones;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<EmailAddress> EmailAddresses
        {
            get
            {
                if (Person != null && Person.Entity != null)
                {
                    return Person.Entity.EmailAddresses;
                }
                return null;
            }
        }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string AddressLine { get; set; }

        [DataMember]
        public int? PostalCode { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public int? PhoneCountryCode { get; set; }
        
        [DataMember]
        public string PhoneNumber { get; set; }
    }

    [DataContract]
    public abstract class IHasStuffWithEntityAndChangeEvent : IHasChangeEvent, IHasStuff, IHasEntity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [ForeignKey("Entity")]
        [DataMember]
        public int? EntityId { get; set; }
        [DataMember]
        public virtual Entity Entity { get; set; }

        [NotMapped]
        [DataMember]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Phone PrimaryPhone
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryPhone;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "Address", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Address PrimaryAddress
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryAddress;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "EmailAddress", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public EmailAddress PrimaryEmailAddress
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryEmailAddress;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<Address> Addresses
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.Addresses;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<Phone> Phones
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.Phones;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        public virtual ICollection<EmailAddress> EmailAddresses
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.EmailAddresses;
                }
                return null;
            }
        }

        [DataMember]
        public string CountryCode { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string AddressLine { get; set; }

        [DataMember]
        public int? PostalCode { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public int? PhoneCountryCode { get; set; }
        
        [DataMember]
        public string PhoneNumber { get; set; }
    }

    [DataContract]
    public abstract class IHasEntity<T> : IHasChangeEvent where T : Entity
    {
        //[DataMember]
        //[ForeignKey("Entity")]
        //public int? EntityId { get; set; }
        //[Required(ErrorMessageResourceType = typeof(SimpleCMS.Resources.Resources), ErrorMessageResourceName = "Required")]
        //[DataMember]
        //public virtual T entity { get; set; }

        public abstract int? EntityId { get; set; }
        public abstract Entity Entity { get; set; }
        
        [NotMapped]
        [DataMember]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Phone PrimaryPhone
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryPhone;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "Address", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public Address PrimaryAddress
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryAddress;
                }
                return null;
            }
        }

        [NotMapped]
        [DataMember]
        [Display(Name = "EmailAddress", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public EmailAddress PrimaryEmailAddress
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.PrimaryEmailAddress;
                }
                return null;
            }
        }
    }
}
