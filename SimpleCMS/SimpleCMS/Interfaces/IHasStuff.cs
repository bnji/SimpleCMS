using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS
{
    public interface IHasStuff : IHasId
    {
        Phone PrimaryPhone { get; }
        Address PrimaryAddress { get; }
        EmailAddress PrimaryEmailAddress { get; }
        ICollection<Phone> Phones { get; }
        ICollection<Address> Addresses { get; }
        ICollection<EmailAddress> EmailAddresses { get; }

        string CountryCode { get; set; }
        string CountryName { get; set; }
        string AddressLine { get; set; }
        int? PostalCode { get; set; }
        string CityName { get; set; }
        int? PhoneCountryCode { get; set; }
        string PhoneNumber { get; set; }
    }
}