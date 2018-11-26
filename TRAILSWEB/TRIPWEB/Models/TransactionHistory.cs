using System;
using System.Runtime.Serialization;

namespace TRIPWEB.Models
{
    [DataContract]
    public class TransactionHistory
    {
        [DataMember]
        public DateTime TransactionDate { get; set; }

        [DataMember]
        public DateTime PostingDate { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Lane { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string TransponderNumber { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public string Year { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string LicensePlateState { get; set; }


    }
}