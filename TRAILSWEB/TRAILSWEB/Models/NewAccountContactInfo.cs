using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace TRAILSWEB.Models
{
    public class NewAccountContactInfo
    {
        [DataMember]
        public string FirstName { set; get; }

        [DataMember]
        public string LastName { set; get; }

        [DataMember]
        public string Address { set; get; }

        [DataMember]
        public string Address2 { set; get; }

        [DataMember]
        public string City { set; get; }

        [DataMember]
        public string State { set; get; }

        [DataMember]
        public string DayPhone { set; get; }

        [DataMember]
        public string ZipCode { set; get; }

        [DataMember]
        public string PinNumber { set; get; }

        [DataMember]
        public string DriversLicense { set; get; }
        [DataMember]
        public string DriversLicenseState { set; get; }

        [DataMember]
        public string CoAppFirstName { set; get; }
        [DataMember]
        public string CoAppLastName { set; get; }

        [DataMember]
        public string CoAppDriversLicense { set; get; }

        [DataMember]
        public string CoAppLicensState { set; get; }

        //business info
        [DataMember]
        public string BusinessName { set; get; }

        [DataMember]
        public string BusinessContactName { set; get; }

        [DataMember]
        public string IsBusiness { set; get; }

        [DataMember]
        public string BusinessAddress { set; get; }

        [DataMember]
        public string BusinessAddress2 { set; get; }

        [DataMember]
        public string BusinessCity { set; get; }

        [DataMember]
        public string BusinessState { set; get; }

        [DataMember]
        public string BusinessZip { set; get; }

        [DataMember]
        public string BusinessPhone { set; get; }

        [DataMember]
        public List<SelectListItem> StateCodes { set; get; }

    }
}