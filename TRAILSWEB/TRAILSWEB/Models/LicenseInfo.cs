using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TRAILSWEB.Models
{
    [DataContract]
    public class LicenseInfo
    {
        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string LicenseState { get; set; }

        [DataMember]
        public decimal TranponderPrice { set; get; }

        [DataMember]
        public string TransponderType { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ValidationError { get; set; }

    }
}