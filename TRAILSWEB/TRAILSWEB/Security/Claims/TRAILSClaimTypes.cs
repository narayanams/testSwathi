using System.ComponentModel;

namespace TRAILSWEB.Security.Claims
{
    public static class TRAILSClaimTypes
    {
        /// <summary>
        /// UserName provided for current Claim Identity.
        /// </summary>
        [Description("UserName provided for current Claim Identity.")]
        public const string UserName = "UserName";

        /// <summary>
        /// Session GUID provided as a result of a successful Login to the System.
        /// </summary>
        [Description("Session GUID provided as a result of a successful Login to the System.")]
        public const string SessionId = "SessionId";

        /// <summary>
        /// Request Number associated with current Session.
        /// </summary>
        [Description("Request Number associated with current Session.")]
        public const string RequestNumber = "RequestNumber";

        /*
        /// <summary>
        /// Transponder Number associated with current User Session.
        /// </summary>
        [Description("Transponder Number associated with current User Session.")]
        public const string TransponderNumber = "TransponderNumber";
        */
        /// <summary>
        /// First Name of current User.
        /// </summary>
        [Description("First Name of current User.")]
        public const string FirstName = "FirstName";

        /// <summary>
        /// Last Name of current User.
        /// </summary>
        [Description("Last Name of current User.")]
        public const string LastName = "LastName";

        /// <summary>
        /// Business Name of current User.
        /// </summary>
        [Description("Business Name of current User")]
        public const string BusinessName = "BusinessName";

        /// <summary>
        /// Email Address of current User.
        /// </summary>
        [Description("Email Address of current User.")]
        public const string EmailAddress = "EmailAddress";

        /// <summary>
        /// Address Line 1 of current User.
        /// </summary>
        [Description("Address Line 1 of current User.")]
        public const string AddressLine1 = "AddressLine1";

        /// <summary>
        /// Address Line 2 of current User.
        /// </summary>
        [Description("Address Line 2 of current User.")]
        public const string AddressLine2 = "AddressLine2";

        /// <summary>
        /// Address Line 2 of current User.
        /// </summary>
        [Description("Address Line 2 of current User.")]
        public const string City = "City";

        /// <summary>
        /// State of current User.
        /// </summary>
        [Description("State of current User.")]
        public const string State = "State";

        /// <summary>
        /// Zip Code of current User.
        /// </summary>
        [Description("Zip Code of current User.")]
        public const string ZipCode = "ZipCode";

        /// <summary>
        /// Workflow Activity
        /// </summary>
        [Description("Workflow Activity")]
        public const string Activity = "Activity";

        /// <summary>
        /// Check Is New Customer
        /// </summary>
        [Description("Check Is New Customer")]
        public const string IsNewCustomer = "IsNewCustomer";

        /// <summary>
        /// Identifies the entrypoint to where the user got started.
        /// </summary>
        [Description("Entrypoint Activity")]
        public const string EntryPointActivity = "EntryPointActivity";

        /// <summary>
        /// Identifies the last successful Activity.
        /// </summary>
        [Description("Previous Activity")]
        public const string PreviousActivity = "PreviousActivity";

        /// <summary>
        /// Payment Confirmation Token
        /// </summary>
        [Description("Payment Confirmation")]
        public const string PaymentConfirmation = "PaymentConfirmation";

        /// <summary>
        /// Identifies the user's new or existing Account Number.
        /// </summary>
        [Description("Account Number")]
        public const string AccountNumber = "AccountNumber";

        /// <summary>
        /// Identifies the user's new or existing Account Number.
        /// </summary>
        [Description("Original Account Number")]
        public const string OriginalAccountNumber = "OriginalAccountNumber";

        /// <summary>
        /// Identifies the user's Current Account Balance.
        /// </summary>
        [Description("Current Account Balance")]
        public const string CurrentBalance = "CurrentBalance";

        /// <summary>
        /// Identifies if a Payment is Required
        /// </summary>
        [Description("Payment Required")]
        public const string PaymentRequired = "PaymentRequired";

        /// <summary>
        /// Indicates that previous Payment was Successful
        /// </summary>
        [Description("Payment Successful")]
        public const string PaymentSuccessful = "PaymentSuccessful";

        /// <summary>
        /// Indicates valid Payment Tracking Identifier
        /// </summary>
        [Description("Payment Tracking Identifier")]
        public const string PaymentId = "PaymentId";

        /// <summary>
        /// Indicates the Transponder Type Selected
        /// </summary>
        [Description("Transponder Type Selected")]
        public const string TransponderTypeSelected = "TransponderTypeSelected";

        /// <summary>
        ///  Indicates the Price for a given Transponder
        /// </summary>
        [Description("Transponder Price")]
        public const string TransponderPrice = "TransponderPrice";

        /// <summary>
        /// Indicates the Sales Tax to Add for Transponder Purchase
        /// </summary>
        [Description("Sales Tax")]
        public const string SalesTax = "SalesTax";

        /// <summary>
        /// Prepaid Tolls Amount
        /// </summary>
        [Description("Prepaid Tolls Amount")]
        public const string PrepaidTollsAmount = "PrepaidTollsAmount";

        /// <summary>
        /// Comma Separated List of License Plates
        /// </summary>
        [Description("Comma Separated List of License Plates")]
        public const string LicensePlateCSVList = "LicensePlateCSVList";

        /// <summary>
        /// Indicates the Transponder Tag to Replace (i.e. Switch)
        /// </summary>
        [Description("Transponder To Replace")]
        public const string TransponderToReplace = "TransponderToReplace";

        /// <summary>
        /// Indicates an Error Message being Transferred from another Page
        /// </summary>
        [Description("Transfer Error Message")]
        public const string TransferError = "TransferError";

        /// <summary>
        /// Indicates payment CC type
        /// </summary>
        [Description("Payment CC Type")]
        public const string PaymentCCType = "PaymentCCType";

        /// <summary>
        /// Indicates payment CC Number
        /// </summary>
        [Description("Payment CC Number")]
        public const string PaymentCCNumber = "PaymentCCNumber";

        /// <summary>
        /// Indicates payment amount
        /// </summary>
        [Description("Payment Amount")]
        public const string PaymentAmount = "PaymentAmount";

        /// <summary>
        /// Web Balance, this is balance coming from Database to makeup a minimum balance for account
        /// </summary>
        [Description("Web Balance")]
        public const string WebBalance = "WebBalance";

        /// <summary>
        /// FinalizeAction, this is claim data makes decision whether finalize for Reload, Replenish, Payment or Activation
        /// </summary>
        [Description("Finalize Action")]
        public const string FinalizeAction = "FinalizeAction";

        /// <summary>
        /// This constat will be used to store temp data for PageNavigationErrors
        /// </summary>
        public const string PageNavigationError = "PageNavigationError";

    }
}