using System.Collections.Generic;
using System.Runtime.Serialization;
using TRAILSWEB.Models;

namespace TRAILSWEB.ViewModels
{
    public class TransponderActivationViewModel
    {
        public Customer CustomerData { get; set; }

        public SecurityInfoModel SecurityModel { get; set; }

        public NewAccountContactInfo ContactInfoModel { get; set; }

        public PaymentInfoModel PaymentModal { get; set; }

        public PreviewTransaction OrderSummary { get; set; }

        public List<Transponder> TranspondersForPurchase { set; get; }
        public List<Cart> AccountTransponders { get; set; }
        public Vehicle VehicleData { get; set; }
        public RegisterModel SecurityPreferences { get; set; }
        public bool CreateUserAccount { get; set; }
        public bool MakePayment { get; set; }
        public bool NewAccount { get; set; }
    }
}