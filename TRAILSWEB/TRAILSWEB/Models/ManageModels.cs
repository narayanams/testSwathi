using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using TRAILSWEB.Helpers;

namespace TRAILSWEB.Models
{
    [DataContract]
    public class ManageModel
    {
        [DataMember]
        public IEnumerable<string> StatementYears { set; get; }

        [DataMember]
        public CustomerInfoModel CustomerInfo { set; get; }

        [DataMember]
        public AccountInfoModel AccountInfo { set; get; }

        [DataMember]
        public AccountPreferencesModel AccountPreferences { set; get; }

        [DataMember]
        public ContactInfoModel ContactInfo { set; get; }

        [DataMember]
        public PaymentInfoModel PaymentInfo { set; get; }

        [DataMember]
        public SecurityInfoModel SecurityInfo { set; get; }

        [DataMember]
        public List<TransponderModel> TransponderList { set; get; }

        [DataMember]
        public TransponderModel UpdateTransponder { set; get; }

        [DataMember]
        public List<SecretQuestionModel> SecretQuestions { set; get; }

        [DataMember]
        public List<VehicleMake> VehicleMakes { set; get; }

        [DataMember]
        public List<VehicleModel> VehicleModels { set; get; }

        [DataMember]
        public List<string> VehicleColors { set; get; }

        [DataMember]
        public List<StateCodeModel> StateCodes { set; get; }

        [DataMember]
        public List<EmailStatementIndicatorModel> EmailStatementIndicators { set; get; }

        [DataMember]
        public List<SelectListItem> EmailStatementIndicatorList { set; get; }



        [DataMember]
        public List<VehiclePlateTypeModel> VehiclePlateTypes { set; get; }

        [DataMember]
        public List<PaymentHistory> PaymentHistory { set; get; }

        [DataMember]
        public List<TransactionHistory> TransactionHistory { set; get; }

        [DataMember]
        public List<FinancialTransactionHistory> FinancialTransactionHistory { set; get; }

        [DataMember]
        public IEnumerable<AccountAlertsModel> AccountAlerts { set; get; }

        [DataMember]
        public PageNavigationError NavigationError { get; set; }

        [DataMember]
        public List<SelectListItem> VehicleYears { get; set; }

        [DataMember]
        public List<Transponder> TranspondersForPurchase { set; get; }
    }

    [DataContract]
    public class MonthlyStatementModel
    {
        [DataMember]
        public byte[] Data { set; get; }

        [DataMember]
        public string Format { set; get; }

        [DataMember]
        public bool IsDataAvailable { set; get; }


    }


    [DataContract]
    public class StateCodeModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class EmailStatementIndicatorModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class VehiclePlateTypeModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class SecretQuestionModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    public class VehicleMake
    {
        [DataMember]
        public string MakeName { get; set; }
    }

    [DataContract]
    public class VehicleModel
    {
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
    }

    [DataContract]
    public class CustomerInfoModel
    {
        [DataMember]
        public string PrimaryFirstName { set; get; }

        [DataMember]
        public string PrimaryLastName { set; get; }

        [DataMember]
        public string PrimaryLicense { set; get; }
        [DataMember]
        public string PrimaryLicenseState { set; get; }

        [DataMember]
        public string SecondaryFirstName { set; get; }

        [DataMember]
        public string SecondaryLastName { set; get; }

        [DataMember]
        public string SecondaryLicense { set; get; }

        [DataMember]
        public string SecondaryLicenseState { set; get; }

        [DataMember]
        public string BusinessContactName { set; get; }

        [DataMember]
        public string BusinessName { set; get; }

        [DataMember]
        public string BusinessTIN { set; get; }

        public bool IsBusinessAccount { set; get; }

        [DataMember]
        public string Pin { set; get; }

    }

    [DataContract]
    public class AccountInfoModel
    {
        [DataMember]
        public string AccountNumber { set; get; }

        [DataMember]
        public string AccountType { set; get; }

        [DataMember]
        public DateTime OpenedDate { set; get; }

        [DataMember]
        public DateTime LastPaymentDate { set; get; }

        [DataMember]
        public double AccountBalence { set; get; }

        [DataMember]
        public DateTime RecentStatementDate { set; get; }


        [DataMember]
        public bool HasPendingTransactions { set; get; }

        [DataMember]
        public DateTime PendingTransactionsStart { set; get; }

        [DataMember]
        public DateTime PendingTransactionsEnd { set; get; }

    }

    [DataContract]
    public class ContactInfoModel
    {
        [DataMember]
        public string AddressLine1 { set; get; }

        [DataMember]
        public string AddressLine2 { set; get; }

        [DataMember]
        public string ZipCode { set; get; }

        [DataMember]
        public string ZipCodeFour { set; get; }

        [DataMember]
        public string City { set; get; }


        [DataMember]
        public string State { set; get; }

        [DataMember]
        public string DayPhone { set; get; }

        [DataMember]
        public string DayPhoneExt { set; get; }

        [DataMember]
        public string EveningPhone { set; get; }

        [DataMember]
        public string Email { set; get; }



    }

    [DataContract]
    public class PaymentInfoModel : PaymentInformation
    {
        /*  [DataMember]
          public string CardNumber { set; get; }

          [DataMember]
          public string CardType { set; get; }*/

        /*  [DataMember]
          public DateTime ExpirationDate { set; get; }*/

        [DataMember]
        public decimal LowBalanceAmount { set; get; }

        [DataMember]
        public decimal MinimumLowBalanceAmount { set; get; }

        [DataMember]
        public decimal ReplenishAmount { set; get; }

        [DataMember]
        public decimal MinimumReplenishAmount { set; get; }

        [DataMember]
        public bool AutoBillIndicator { get; set; }

        [DataMember]
        public bool DeleteCard { get; set; }

        [DataMember]
        public string CouponCode { get; set; }

        [DataMember]
        public bool IsCouponCodeApplied { get; set; }

    }

    [DataContract]
    public class SecurityInfoModel
    {
        [DataMember]
        public Guid SessionID { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string RequestNumber { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string ConfirmPassword { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string NewPassword { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string ConfirmEmail { get; set; }

        [DataMember]
        public string SecretQuestion { get; set; }
        [DataMember]
        public int? SecretQuestionId { get; set; }

        [DataMember]
        public string SecretAnswer { get; set; }

        [DataMember]
        public string Pin { get; set; }
    }

    [DataContract]
    public class TransponderModel
    {
        [DataMember]
        public int UpdateAction { get; set; }

        [DataMember]
        public long TransponderNumber { get; set; }

        [DataMember]
        public string TransponderType { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public string TransponderDetails { get; set; }



        [DataMember]
        public int IssuingAuthority { get; set; }

        [DataMember]
        public string TransponderStatus { get; set; }

        [DataMember]
        public string TransponderStatusCode { get; set; }

        //public string Display
        //{
        //    get
        //    {
        //        return $"{Year} {Make} {Model} - {LicensePlateNumber}";
        //    }
        //}

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int Class { get; set; }

        [DataMember]
        public string LicensePlateNumber { get; set; }

        [DataMember]
        public string LicensePlateType { get; set; }

        [DataMember]
        public int LicensePlateTypeId { get; set; }

        [DataMember]
        public string LicenseStateSelected { get; set; }

        [DataMember]
        public decimal SalesAmount { get; set; }

        [DataMember]
        public decimal SalesTax { get; set; }

        [DataMember]
        public int StateCode { get; set; }

        [DataMember]
        public string EditEnabled { get; set; }

        [DataMember]
        public bool SwitchIndicator { get; set; }

        [DataMember]
        public string SwitchTagNumber { get; set; }

    }

    [DataContract]
    public class AccountPreferencesModel
    {
        [DataMember]
        public bool OptedForLowBalanceField { get; set; }

        [DataMember]
        public bool OptedForOutOfFundField { get; set; }

        [DataMember]
        public bool OptedForEmailParkingReceiptField { get; set; }

        [DataMember]
        public bool OptedForMonthlyStatementField { get; set; }

        [DataMember]
        public int MonthlyEmailStatementType { get; set; }

        [DataMember]
        [Description("Email Statement Indicator")]
        [Display(Name = "Email Statement Indicator")]
        public string EmailStatementIndicatorListSelected { get; set; }

    }

    [DataContract]
    public class AccountAlertsModel
    {
        [DataMember]
        public string AlertDetailField { get; set; }

        [DataMember]
        public int AlertIdField { get; set; }

        [DataMember]
        public string AlertTitleField { get; set; }

        [DataMember]
        public string AlertTypeField { get; set; }

        [DataMember]
        public string LinkAddressField { get; set; }

        [DataMember]
        public string LinkTextField { get; set; }

        [DataMember]
        public int OrderField { get; set; }

        [DataMember]
        public string SeverityField { get; set; }



    }
    public class CustomerModel
    {
        public CustomerInfoModel CustomerInfo { get; set; }

        public ContactInfoModel ContactInfo { get; set; }

        public AccountInfoModel AccountInfo { get; set; }

        public CustomerModel()
        {
            this.CustomerInfo = new CustomerInfoModel();
            this.ContactInfo = new ContactInfoModel();
            this.AccountInfo = new AccountInfoModel();
        }

    }

    public class PreviewTransaction : StartTransaction
    {

        [DataMember]
        public string ValidationError { get; set; }
        [DataMember]
        public TransponderToPurchase[] Transponders { get; set; }

        [DataMember]
        public bool HasAValidData { get; set; }

        public string TransponderAction { get; set; }

    }

    public class FinalPayouts
    {
        [DataMember]
        public decimal SalesAmount { get; set; }

        [DataMember]
        public decimal SalesTax { get; set; }

        [DataMember]
        public decimal PrepaidTollsAmount { get; set; }



        public decimal TransponderCost { get; set; }

        public decimal CouponDiscountAmount { get; set; }

        public decimal FinalSalesAmount { get; set; }

        public decimal FinalSalesTax { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TotalPaymentAmount { get; set; }


    }


    public class StartTransaction
    {
        [DataMember]
        public List<Transponder> TranspondersForPurchase { get; set; }

        public SecurityInfoModel Security { get; set; }
        public CustomerModel Customer { get; set; }

        public bool HasSecurityData { get; set; }
        public bool HasCustomerData { get; set; }

        public PaymentInfoModel Payment { get; set; }

        public string SourcePage { get; set; }

        public bool HasAPopup { get; set; }

        [DataMember]
        public List<SelectListItem> StateCodes { set; get; }

        public bool IsNewCustomer { set; get; }

        public string InitialProduct { get; set; }


    }

    public class ValidateDiscountAndCoupons
    {
        public string CouponCode { get; set; }

        public TransponderToPurchase[] Transponders { get; set; }

        public bool IsNewCustomer { get; set; }

        public bool IsCouponCodeApplied { get; set; }

    }

    public class SubmitTransaction
    {
        [DataMember]
        public TransponderToPurchase[] Transponders { get; set; }
        public SecurityInfoModel Login { get; set; }
        public PaymentInformation Payment { get; set; }
    }

    public class AddTransponderModel
    {
        [DataMember]
        public List<Transponder> TranspondersForPurchase { get; set; }

        [DataMember]
        public PaymentInformation Payment { get; set; }

        [DataMember]
        public List<SelectListItem> StateCodes { set; get; }

        [DataMember]
        public bool IsNewCustomer { get; set; }
    }

    public class TransponderSelectionModel
    {
        [DataMember]
        public Transponder Transponder { get; set; }
        [DataMember]
        public List<SelectListItem> StateCodes { set; get; }
    }

    [DataContract]
    public class PaymentWarnings
    {
        [DataMember]
        public string CheckWarningCode { get; set; }

        [DataMember]
        public DateTime CheckWarningDate { get; set; }

        [DataMember]
        public string CheckWarningDescription { get; set; }

        [DataMember]
        public string CCWarningCode { get; set; }

        [DataMember]
        public DateTime CCWarningDate { get; set; }

        [DataMember]
        public string CCWarningDescription { get; set; }

        [DataMember]
        public string ACHWarningCode { get; set; }

        [DataMember]
        public DateTime ACHWarningDate { get; set; }

        [DataMember]
        public string ACHWarningDescription { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }

    public enum Tabselected
    {
        CC = 0,
        ACH = 1,
        Account = 2
    };

    public class PendingTransactions
    {
        [DataMember]
        public DateTime StartDate { set; get; }

        [DataMember]
        public DateTime EndDate { set; get; }


        [DataMember]
        public PaymentInformation Payment { get; set; }

        
        [DataMember]
        public decimal AccountBalence { set; get; }

    }
    public class PayPendingTransactions : PendingTransactions
    {
        [DataMember]
        public List<TransactionHistory> PendingTransactions { get; set; }

        [DataMember]
        public double AccountBalanceAfterTollAmount { set; get; }


        [DataMember]
        public PaymentInformation Payment { get; set; }

    }

    public class ProcessPendingTransactions : PendingTransactions
    {
        [DataMember]
        public List<int> TransactionIDs { get; set; }

        [DataMember]
        public List<int> DisputedTransactionIDs { get; set; }

        [DataMember]
        public decimal TotalPayableAmount { get; set; }

        [DataMember]
        public bool IsPayingFromAccount { get; set; }

        [DataMember]
        public decimal TollAmountSelected { set; get; }

        [DataMember]
        public decimal AccountBalanceAfterTollAmount { set; get; }

    }

    public class ProcessPendingTransactionsSuccess: ProcessPendingTransactions
    {
        [DataMember]
        public TRAILSWEBServiceReference.PaymentReceipt PaymentConfirmationReceipt { get; set; }
        
        [DataMember]
        public decimal IsSuccessfull { get; set; }

        [DataMember]
        public string ThanksMessage { get; set; }

        [DataMember]
        public string PaymentConfirmationNumber { get; set; }

        public string AccountNumber { get; set; }
    }
}