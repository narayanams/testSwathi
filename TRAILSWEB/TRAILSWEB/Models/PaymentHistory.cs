using System;
using System.Runtime.Serialization;

namespace TRAILSWEB.Models
{
    [DataContract]
    public class PaymentHistory
    {
        [DataMember]
        public long TransactionId { get; set; }

        [DataMember]
        public long TrackingNumber { get; set; }

        [DataMember]
        public DateTime PostingDate { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string PaymentCardNumber { get; set; }

        [DataMember]
        public decimal PaymentAmount { get; set; }


    }
}