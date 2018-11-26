using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRIPWEB.Models
{
    public class Confirmation
    {
        public int AccountNumber { get; set; }
        public string TripId { get; set; }
        public string TripUUId { get; set; }
        public string EmailAddress { get; set; }
        public TripStatus Status { get; set; }
        public TripStage Stage { get; set; }
    }
}