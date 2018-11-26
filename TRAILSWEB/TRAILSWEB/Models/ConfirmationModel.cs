using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace TRAILSWEB.Models
{
    public class ConfirmationModel
    {
        //[DataMember]
        //public TransponderToPurchase[] TranspondersPurchased { get; set; }

        [DataMember]
        public int TransQtyPurchased { get; set; }
        
        [DataMember]
        public StringBuilder TranponderTypesPurchased { get; set; }        

        [DataMember]
        public bool IsTransponderPurchase { get; set; }

        [DataMember]
        public string TransponderAction { get; set;  }

        [DataMember]
        public bool IsTransponderActivation { get; set; }

        [DataMember]
        public bool IsActivationOnly { get; set; }

        [DataMember]
        public bool IsPaymentRequired { get; set; }

        [DataMember]
        public bool IsNewAccount { get; set; }

        [DataMember]
        public decimal TransponderPrice { get; set; }

        [DataMember]
        public double SalesTax { get; set; }

        //Total Payment amount of transaction
        [DataMember]
        public double TotalPaymentAmount { get; set; }

        [DataMember]
        public string PaymentTrackingNumber { get; set; }

        [DataMember]
        public decimal PrepaidTollsAmount { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string CreditCardType { get; set; }

        [DataMember]
        public string CreditCardNumber { get; set; }


        [DataMember]
        public bool IsCCPayment { get; set; }
        [DataMember]
        public string BankAccountNumber { get; set; }

        [DataMember]
        public string JournalTransactionID { get; set; }

        [DataMember]
        public string ThanksMessage { get; set; }

        [DataMember]
        public OrderDetails OrderDesc { get; set; }

        [DataMember]
        public ContactInfoModel ContactInformation { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        public ConfirmationModel()
        {
            this.OrderDesc = new OrderDetails();
        }

        public bool IsPopUp { get; set; }

    }

    public class OrderDetails
    {
        public string Item { get; set; }
        public string MinimumBalance { get; set; }
        public string ArrivalMessage { get; set; }
    }
}