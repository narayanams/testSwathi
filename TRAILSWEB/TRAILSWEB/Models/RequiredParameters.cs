using System;

namespace TRAILSWEB.Models
{
    public class Client
    {
        public string IPAddress { get; set; }
        public string BrowserType { get; set; }
        public string Platform { get; set; }
    }
    public class RequiredParameters
    {
        public Guid SessionId { get; set; }
        public string RequestNumber { get; set; }
        public Client Client { get; set; }
    }
}