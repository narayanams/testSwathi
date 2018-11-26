using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRAILSWEB.Models;

namespace TRAILSWEB.ViewModels
{
    public class TransponderActivationViewModel
    {
        public Customer CustomerData { get; set; }
        public List<Cart> AccountTransponders { get; set; }
        public Vehicle VehicleData { get; set; }
        public RegisterModel SecurityPreferences { get; set; }
        public bool CreateUserAccount { get; set; }
        public bool MakePayment { get; set; }
        public bool NewAccount { get; set; }
    }
}