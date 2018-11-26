using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace TRAILSWEB.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string TransponderNumber { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string PaymentID { get; set; }

        [DataMember]
        //[Required] //Removed this check as now we don't need check for Business Account
        [Description("First Name")]
        [Display(Name = "First Name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Name, ErrorMessage = "Please enter a valid First Name")]
        public string FirstName { get; set; }

        [DataMember]
        //[Required] //Removed this check as now we don't need check for Business Account
        [Description("Last Name")]
        [Display(Name = "Last Name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Name, ErrorMessage = "Please provide valid Last Name.")]
        public string LastName { get; set; }


        [DataMember]
        //[Required] //Removed this check as now we don't need check for Business Account
        [Description("CoUser First Name")]
        [Display(Name = "CoUser First Name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Name, ErrorMessage = "Please enter a valid Secondary First Name")]

        public string CoUserFirstName { get; set; }

        [DataMember]
        //[Required] //Removed this check as now we don't need check for Business Account
        [Description("CoUser Last Name")]
        [Display(Name = "CoUser Last Name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Name, ErrorMessage = "Please enter a valid Secondary Last Name")]
        public string CoUserLastName { get; set; }

        [DataMember]
        [Description("Business Name")]
        [Display(Name = "Business Name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.BusinessName, ErrorMessage = "Please enter a valid Secondary Last Name")]
        public string BusinessName { get; set; }

        [DataMember]
        [Description("Business Attention")]
        [Display(Name = "Business Attention")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Name, ErrorMessage = "Please enter a valid Business Attention Name")]
        public string BusinessAttentionName { get; set; }

        [DataMember]
        [Description("Business Tax ID")]
        [Display(Name = "Business Tax ID")]
        public string BusinessTaxID { get; set; }


        [DataMember]
        //[Required]
        [Description("Address Line 1")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [DataMember]
        [Description("Address Line 2")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [DataMember]
        [Required]
        [Description("City")]
        [Display(Name = "City")]
        public string City { get; set; }

        [DataMember]
        [Description("Address State")]
        [Display(Name = "Address State")]
        public List<SelectListItem> AddressState { get; set; }

        [DataMember]
        [Required]
        [Description("Address State")]
        [Display(Name = "Address State")]
        public string AddressStateSelected { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.PostalCode)]
        [Description("Zip Code")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [DataMember]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Description("Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [DataMember]
        [DataType(DataType.PhoneNumber)]
        [Description("Phone Number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataMember]
        [DataType(DataType.PhoneNumber)]
        [Description("Work Phone Number")]
        [Display(Name = "Work  Phone Number")]
        public string WorkPhoneNumber { get; set; }


        [DataMember]
        [Description("Phone Number Extn")]
        [Display(Name = "Phone Number Extn")]
        public string WorkPhoneNumberExtn { get; set; }


        [DataMember]
        [Description("License Plate Number")]
        [Display(Name = "License Plate Number")]
        public string LicensePlateNumber { get; set; }

        [DataMember]
        [Description("License Plate Issuing State")]
        [Display(Name = "License Plate Issuing State")]
        public List<SelectListItem> LicenseState { get; set; }

        [DataMember]
        [Description("License Plate Issuing State")]
        [Display(Name = "License Plate Issuing State")]
        public string LicenseStateSelected { get; set; }


        [DataMember]
        [Description("CoUser License Plate Number")]
        [Display(Name = "CoUser License Plate Number")]
        public string CoUserLicensePlateNumber { get; set; }

        [DataMember]
        [Description("CoUser License Plate Issuing State")]
        [Display(Name = "CoUser License Plate Issuing State")]
        public string CoUserLicenseStateSelected { get; set; }

        [DataMember]
        //[Required]
        [Description("Driver License Number")]
        [Display(Name = "Driver License Number")]
        public string DriversLicense { get; set; }

        [DataMember]
        [Description("State Driver License Was Issued From")]
        [Display(Name = "State Driver License Was Issued From")]
        public List<SelectListItem> DriversLicenseState { get; set; }

        [DataMember]
        //[Required]
        [Description("State Driver License Was Issued From")]
        [Display(Name = "State Driver License Was Issued From")]
        public string DriversLicenseStateSelected { get; set; }

        [DataMember]
        public string Pin { get; set; }

        public PaymentInformation PaymentInfo { get; set; }

        public List<Cart> ShoppingCart { get; set; }

        [DataMember]
        public bool IsPaymentRequired { get; set; }

        [DataMember]
        public bool IsPersonalAccount { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string MaintenanceMessage { get; set; }

        [DataMember]
        public string WarningMessage { get; set; }

        [DataMember]
        public string AlertMessage { get; set; }
    }

    public class PaymentInformation
    {

        [DataMember]
        public bool IsCCPayment { get; set; }

        [DataMember]

        public string CreditCardType { get; set; }

        public List<SelectListItem> CreditCardTypeList { get; set; }

        [DataMember]

        public string CreditCardNumber { get; set; }

        [DataMember]
        public DateTime CreditCardExpiration { get; set; }

        [DataMember]

        public string ExpirationMonth { get; set; }

        public List<SelectListItem> ExpirationMonthList { get; set; }

        [DataMember]

        public string ExpirationYear { get; set; }

        public List<SelectListItem> ExpirationYearList { get; set; }

        [DataMember]
        public string NameOnCard { get; set; }

        [DataMember]
        public string AccountHolderLastName { get; set; }

        [DataMember]
        public string AccountHolderFirstName { get; set; }

        [DataMember]
        public string BankAccountNumber { get; set; }


        [DataMember]
        public string RoutingNumber { get; set; }


        [DataMember]
        public bool UseCardOnFile { get; set; }

        [DataMember]
        public bool AutoReload { get; set; }

        [DataMember]
        public bool SaveCardOnFile { get; set; }

        [DataMember]
        public double Balance { get; set; }

        [DataMember]
        public double WebBalance { get; set; }

        [DataMember]
        public bool IsPaymentRequired { get; set; }

        [Description("Prepaid Tolls Amount")]
        [Display(Name = "Prepaid Tolls Amount")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double PrepaidTollsAmount { get; set; }

        [DataMember]
        [Required]
        [Description("Payment Amount")]
        [Display(Name = "Payment Amount")]
        public double PaymentAmount { get; set; }

        [Description("Transponder Price")]
        [Display(Name = "E-PASS Price")]
        public double? TransponderPrice { get; set; }

        /// <summary>
        /// Sales Tax may be applied to Transponder Purchases, but not Prepaid Tolls
        /// </summary>
        [Description("Sales Tax to be added to Purchase Amount")]
        [Display(Name = "Sales Tax")]
        public decimal SalesTax { get; set; }

        /// <summary>
        /// Minimum Prepaid Tolls which has to be display on screen
        /// </summary>
        [Description("Minimum Prepaid Tolls which has to be display on screen")]
        [Display(Name = "Minimum Prepaid Tolls")]
        public decimal MinimumPrepaidTolls { get; set; }

        /// <summary>
        /// Minimum Prepaid Tolls which has to be display on screen
        /// </summary>
        [Description("Zero Dollar Payment Flag")]
        [Display(Name = "Zero Dollar Payment")]
        public bool IsZeroPayment { get; set; }

        public decimal MinLowBalanceAmount { get; set; }
        public decimal MinReplenishAmount { get; set; }

        private decimal minValue = decimal.Parse(ConfigurationManager.AppSettings["EPASSMinAutoReplenishAmt"]);
        public decimal DisplayMinReplenishAmount
        {
            get { return minValue; }
            set { minValue = 40M; }
        }

        public decimal LowBalanceAmount { get; set; }
        public decimal ReplenishAmount { get; set; }

        public PaymentWarnings PaymentWarningInfo { get; set; }

        public PromotionsInformation PromotionInfo { get; set; }

    }

    public class PaymentInformationExtn
    {
        [DataMember]
        public string State { get; set; }

        [DataMember]
        public PaymentInformation PaymentInformation { get; set; }

        [DataMember]
        public string Action { get; set; }

    }

    public class PromotionsInformation
    {
        [DataMember]
        public bool IsPromotionActive { get; set; }

        //[DataMember]
        //public string PromotionEventCode { get; set; }

        //[DataMember]
        //public string PromotionCode { get; set; }

        [DataMember]
        public string CouponCode { get; set; }

        [DataMember]
        public List<PromotionTransponderInformation> PromotionTransponderInformation { get; set; }

    }

    public class PromotionTransponderInformation
    {
        [DataMember]
        public bool IsValidCoupon { get; set; }

        [DataMember]
        public string PromotionCode { get; set; }

        [DataMember]
        public int TransponderType { get; set; }

        [DataMember]
        public int NumberOfTransponders { get; set; }

        [DataMember]
        public decimal PromotionPrice { get; set; }

        [DataMember]
        public int CountNumberOfTransponders { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }
}
