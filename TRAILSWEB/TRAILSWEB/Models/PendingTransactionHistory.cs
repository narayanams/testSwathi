using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TRAILSWEB.Models
{
    public class PendingTransactionHistory : TransactionHistory
    {
        [DataMember]
        public string TransactionIDValue { get; set; }

        [DataMember]
        public double TransactionDateElapsed { get; set; }

        [DataMember]
        public int TransactionID { get; set; }

        [DataMember]
        public bool IsSelected { get; set; }
    }
}