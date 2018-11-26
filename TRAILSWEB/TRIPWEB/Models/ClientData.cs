using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TRIPWEB.Models
{
    public class ClientData
    {
        public string IPAddress { get; set; }
        public string BrowserType { get; set; }
        public string Platform { get; set; }
    }

    public class RequiredParameters
    {
        public Guid SessionId { get; set; }
        public string RequestNumber { get; set; }
        public ClientData Client { get; set; }
    }


    public class RequesterInformation
    {
        [DataMember]
        public string ClientIP { get; set; }

        [DataMember]
        public string BrowserType { get; set; }

        [DataMember]
        public string OperatingSystem { get; set; }

        [DataMember]
        public string Version { get; set; }

    }

    [DataContract]
    public class States
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class Country
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}