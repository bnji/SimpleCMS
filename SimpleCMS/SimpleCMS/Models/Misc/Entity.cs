using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SimpleCMS.Models
{
    public class Entity
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }

        public Entity()
        {
            Phones = new HashSet<Phone>();
            EmailAddresses = new HashSet<EmailAddress>();
            Addresses = new HashSet<Address>();
        }

        [NotMapped]
        [Display(Name = "EmailAddress", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual EmailAddress PrimaryEmailAddress
        {
            get
            {
                if (EmailAddresses.Count > 0)
                {
                    var ae = EmailAddresses.GetEnumerator();
                    while (ae.MoveNext())
                    {
                        var changeEvent = ae.Current as IHasChangeEvent;
                        if (changeEvent != null && changeEvent.ChangeEventDeletedOn == null && ae.Current.IsPrimary)
                        {
                            return ae.Current;
                        }
                    }
                }
                return null;
            }
        }

        [NotMapped]
        [Display(Name = "Address", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual Address PrimaryAddress
        {
            get
            {
                if (Addresses.Count > 0)
                {
                    var ae = Addresses.GetEnumerator();
                    while (ae.MoveNext())
                    {
                        var changeEvent = ae.Current as IHasChangeEvent;
                        if (changeEvent != null && changeEvent.ChangeEventDeletedOn == null && ae.Current.IsPrimary)
                        {
                            return ae.Current;
                        }
                    }
                }
                return null;
            }
        }

        [NotMapped]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public virtual Phone PrimaryPhone
        {
            get
            {
                if (Phones.Count > 0)
                {
                    var ae = Phones.GetEnumerator();
                    while (ae.MoveNext())
                    {
                        var changeEvent = ae.Current as IHasChangeEvent;
                        if (changeEvent != null && changeEvent.ChangeEventDeletedOn == null && ae.Current.IsPrimary)
                        {
                            return ae.Current;
                        }
                    }
                }
                return null;
            }
        }
    }
}