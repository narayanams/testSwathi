using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using TRAILSWEB.TRAILSWEBServiceReference;
using TransactionHistory = TRIPWEB.Models.TransactionHistory;

namespace TRIPWEB.Models
{
    public class Reservation
    {
        public Reservation(){
            Action ="Update";
            ContactInfo = new ContactInformation() { AddressCountryList = new List<SelectListItem>(), AddressStateList = new List<SelectListItem>() };
            PaymentInfo = new PaymentInformation();
            VehicleInfo = new VehicleInformation();
            DepartureTimeList = new List<SelectListItem>();
            ArrivalTimeList = new List<SelectListItem>();
            RentalAgencyList = new List<SelectListItem>();
            TollTransactions = new List<TransactionHistory>();
            FinancialTransactions = new List<TransactionHistory>();
        }
        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string TripId { get; set; }
        [DataMember]
        public string TripUUId { get; set; }

        [DataMember]
        public bool IsBusinessAccountType { get; set; }

        [DataMember]
        public bool EmailUpdateFlag { get; set; }

        [DataMember]
        public TripStatus TripCurrentStatus { get; set; } //reservation(reserved,fullfilled,cancelled)-Trip(pending,started,ended,closed,voided,cancelled)

        [DataMember]
        public TripStage TripStage { get; set; } //either Reservation or Trip

        public ContactInformation ContactInfo { get; set; }

        [DataMember]
        public string ArrivalDate { get; set; }

        [DataMember]
        public List<SelectListItem> ArrivalTimeList { get; set; }

        [DataMember]
        public string ArrivalTimeSelected { get; set; }

        [DataMember]
        public string DepartureDate { get; set; }

        [DataMember]
        public List<SelectListItem> DepartureTimeList { get; set; }

        [DataMember]
        public string DepartureTimeSelected { get; set; }
        
        [DataMember]
        public List<SelectListItem> RentalAgencyList { get; set; }

        [DataMember]
        public string RentalAgencySelected { get; set; }

        [DataMember]
        public string RentalAgencySelectedText { get; set; }

        public PaymentInformation PaymentInfo { get; set; }

        public VehicleInformation VehicleInfo { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public bool ValidationError { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public bool TermsAndConditions { get; set; }


        [DataMember]
        public bool IsCCDeclined { get; set; }

        [DataMember]
        public string ReservationSource { get; set; }


        [DataMember]
        public List<TransactionHistory> TollTransactions { get; set; }

        [DataMember]
        public List<TransactionHistory> FinancialTransactions { get; set; }


    }


    [DataContract]
    public class ContactInformation
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string OldEmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public List<SelectListItem> AddressCountryList { get; set; }

        [DataMember]
        public string AddressCountrySelected { get; set; }

        [DataMember]
        public List<SelectListItem> AddressStateList { get; set; }

        [DataMember]
        public string AddressStateSelected { get; set; }

        [DataMember]
        public string ZipCode { get; set; }
        
    }



    public class PaymentInformation
    {
        [DataMember]
        public bool UseCardOnFile { get; set; }

        [DataMember]
        public bool AutoReload { get; set; }

        [DataMember]
        public string NameOnCard { get; set; }

        [DataMember]
        public string CreditCardNumber { get; set; }

        [DataMember]
        public string CreditCardType { get; set; }

        public List<SelectListItem> CreditCardTypeList { get; set; }

        [DataMember]
        public DateTime CreditCardExpirationDate { get; set; }

        [DataMember]
        public string ExpirationMonth { get; set; }

        public List<SelectListItem> ExpirationMonthList { get; set; }

        [DataMember]
        public string ExpirationYear { get; set; }

        public List<SelectListItem> ExpirationYearList { get; set; }

        [DataMember]
        public string OldNameOnCard { get; set; }

        [DataMember]
        public string OldCreditCardNumber { get; set; }

        [DataMember]
        public string OldCreditCardType { get; set; }

        [DataMember]
        public string OldExpirationMonth { get; set; }

        [DataMember]
        public string OldExpirationYear { get; set; }

    }

    [DataContract]
    public class VehicleInformation
    {
        [DataMember]
        public string AppSessionId { get; set; }

        //[DataMember]
        //public string RequestNumber { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string LicenseState { get; set; }

        [DataMember]
        public List<SelectListItem> LicenseStateList { get; set; }

        [DataMember]
        public string LicenseStateSelected { get; set; }

        //[DataMember]
        //public string Make { get; set; }

        //[DataMember]
        //public string Model { get; set; }

        //[DataMember]
        //public string Year { get; set; }

        //[DataMember]
        //public string Color { get; set; }

        //[DataMember]
        //public int TransponderType { get; set; }

    }

    [DataContract]
    public enum TripStatus
    {
        Other = 0,
        Reserved = 1,
        ResCancelled = 2,  // not used right now but can be used in future
        Fulfilled = 3,  //
        Started = 4,  // trip started
        Cancelled = 5,  // trip cancelled forn ipad app
        Pending = 6,   //trip before entering vehicle info
        Voided = 7,    // trip voided at any point
        Ended = 8,     // trip finished but all payment transaction not finished
        Closed = 9 // basically after the trip is payed for and closed 
    }

    

    public enum TripStage
    {
        Reservation = 0,
        Trip = 1
    }

    public enum TripAction
    {
        Update = 100,
        Cancel = 101, // This status will not come from database but will be used internally
        Extend = 102, // This status will not come from database but will be used internally
        ChangeEmail = 103, // This status will not come from database but will be used internally
        End = 104 // This status will not come from database but will be used internally
    }
}