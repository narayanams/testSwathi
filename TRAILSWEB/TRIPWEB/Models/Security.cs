using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TRIPWEB.Models
{
    public class Security
    {

    }
    [DataContract]
    public class LoginModel
    {
        [DataMember]
        public string ClientIPAddress { get; set; }
        [DataMember]
        public string ClientBrowserType { get; set; }
        [DataMember]
        public string ClientOperatingSystem { get; set; }
        [DataMember]
        public string ReservationID { get; set; }
        [DataMember]
        public string TripID { get; set; }
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class SessionInformation
    {
        [DataMember]
        public string TripId { get; set; }

        [DataMember]
        public string TripUUId { get; set; }

        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public DateTime SessionStartedTime { get; set; }

        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public string ApplicationName { get; set; }

        [DataMember]
        public string[] Error { get; set; }

    }
}