using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TRAILSWEB.Validators.BoAServiceReference
{
    public partial class Consumer
    {


        //MakeCCPayment   
        public Consumer(string paymentID, string cardType, string cardNumber, DateTime cardExpiryMMYYY, string merchantID, decimal amount,
           string accountNoDesc, string firstName, string lastName, string address1, string address2, string city, string state,
            UInt32 zipcode, string countryCode)
        {

            this.PaymentID = paymentID;               //order no
            this.CardType = cardType.ToUpper();       //mop
            this.CardNumber = cardNumber;             //Account No
            this.ExpiryMMYYYY = cardExpiryMMYYY;     //expdate MMYY
            this.MerchantID = merchantID;                //division No
            this.Amount = amount;             //amount
            this.AccountNoDesc = accountNoDesc;


            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address1 = address1;
            this.Address2 = address2;
            this.City = city;
            this.State = state;
            this.ZipCode = zipcode;
            this.CountryCode = countryCode;

        }

        //ReverseCCPayment  
        public Consumer(string paymentID, string cardType, string cardNumber, DateTime cardExpiryMMYYY, string merchantID, decimal amount,
            string authorizationCode, DateTime responseDate)
        {

            this.PaymentID = paymentID;               //order no
            this.CardType = cardType.ToUpper();       //mop
            this.CardNumber = cardNumber;             //Account No
            this.ExpiryMMYYYY = cardExpiryMMYYY;     //expdate MMYY
            this.MerchantID = merchantID;                //division No
            this.Amount = amount;             //amount


            this.ResponseDate = responseDate;
            this.AuthorizationCode = authorizationCode;


        }

        //MakeACHPayment 
        public Consumer(string paymentID, string achAccountNo, string achRoutingNo, string merchantID, decimal amount,
            string firstName, string lastName, string address1, string address2, string city, string state,
            UInt32 zipcode, string countryCode,
             string email, string ipAddress)
        {

            this.PaymentID = paymentID;     //order no
            this.ACHAccountNumber = achAccountNo.ToUpper();       //ACH Account Number
            this.MerchantID = merchantID;                //division No
            this.Amount = amount;             //amount

            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address1 = address1;
            this.Address2 = address2;
            this.City = city;
            this.State = state;
            this.ZipCode = zipcode;
            this.CountryCode = countryCode;

            this.Email = email;
            this.IPAddress = ipAddress;
            this.ACHRoutingNumber = achRoutingNo;
        }

        //ReverseACHPayment  
        public Consumer(string paymentID, string achAccountNo, string achRoutingNo, string merchantID, decimal amount,
           string authorizationCode, DateTime responseDate, string teleCheckTraceID)
        {

            this.PaymentID = paymentID;               //order no
            this.ACHAccountNumber = achAccountNo;     //Account No
            this.ACHRoutingNumber = achRoutingNo;    //Routing No
            this.MerchantID = merchantID;                //division No
            this.Amount = amount;             //amount

            this.ResponseDate = responseDate;
            this.AuthorizationCode = authorizationCode;
            this.TelecheckTraceID = teleCheckTraceID;

        }

        //Tokanizer   
        public Consumer(string cardType, string cardNumber, DateTime cardExpiryMMYYY, string merchantID)
        {

            this.CardType = cardType.ToUpper();       //mop
            this.CardNumber = cardNumber;             //Account No
            this.ExpiryMMYYYY = cardExpiryMMYYY;     //expdate MMYY
            this.MerchantID = merchantID;                //division No

        }


    }



    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class ConsumerValidation
    {
        private bool _isValid;
        private Dictionary<string, string> errors;

        public bool Isvalid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }


        public Dictionary<string, string> Errors { get { return errors; } }
        public ConsumerValidation() { errors = new Dictionary<string, string>(); }

        public ConsumerValidation ValidateMakeCCPayment(Consumer thisClass)
        {
            ConsumerValidation thisReturn = new ConsumerValidation();
            List<string> getErrors = new List<string>();
            string[] validateArray = new string[] { "paymentid", "cardtype","cardnumber","cardexpirymmyyy","merchantid","amount",
            "accountnodesc","zipcode"};
            //string[] validateArray = new string[] { "paymentid", "cardType","cardnumber","merchantID","accountNoDesc"};

            getErrors = ValidateProperties(validateArray, thisClass);

            foreach (string s in getErrors)
            {
                thisReturn.Errors.Add(s, "Null or Empty");

            }

            thisReturn = ValidateMakeCCKeyComponents(thisClass, thisReturn);
            if (thisReturn.Errors.Count > 0)
            {
                thisReturn.Isvalid = false;
            }
            else
            {
                thisReturn.Isvalid = true;
            }
            return thisReturn;

        }

        public ConsumerValidation ValidateReverseCCPayment(Consumer thisClass)
        {
            ConsumerValidation thisReturn = new ConsumerValidation();
            List<string> getErrors = new List<string>();
            string[] validateArray = new string[] {"paymentid", "cardtype", "cardnumber", "cardexpirymmyyy", "merchantid", "amount",
            "authorizationcode", "responsedate"};

            getErrors = ValidateProperties(validateArray, thisClass);

            foreach (string s in getErrors)
            {
                thisReturn.Errors.Add(s, "Null or Empty");

            }

            thisReturn = ValidateReverseCCKeyComponents(thisClass, thisReturn);
            if (thisReturn.Errors.Count > 0)
            {
                thisReturn.Isvalid = false;
            }
            else
            {
                thisReturn.Isvalid = true;
            }
            return thisReturn;
        }

        public ConsumerValidation ValidateMakeAchPayment(Consumer thisClass)
        {
            ConsumerValidation thisReturn = new ConsumerValidation();
            List<string> getErrors = new List<string>();
            string[] validateArray = new string[] {"paymentid", "achaccountno","achroutingno","merchantid","amount",
            "firstname", "lastname","zipcode", "countrycode","email","ipaddress"};

            getErrors = ValidateProperties(validateArray, thisClass);

            foreach (string s in getErrors)
            {
                thisReturn.Errors.Add(s, "Null or Empty");

            }

            thisReturn = ValidateMakeACHKeyComponents(thisClass, thisReturn);
            if (thisReturn.Errors.Count > 0)
            {
                thisReturn.Isvalid = false;
            }
            else
            {
                thisReturn.Isvalid = true;
            }
            return thisReturn;
        }
        public ConsumerValidation ValidateReverseAchPayment(Consumer thisClass)
        {
            ConsumerValidation thisReturn = new ConsumerValidation();
            List<string> getErrors = new List<string>();
            string[] validateArray = new string[] {"paymentid","achaccountno","achroutingno","merchantid", "amount",
           "authorizationcode","responsedate", "telechecktraceid"};

            getErrors = ValidateProperties(validateArray, thisClass);

            foreach (string s in getErrors)
            {
                thisReturn.Errors.Add(s, "Null or Empty");

            }

            thisReturn = ValidateReverseACHKeyComponents(thisClass, thisReturn);
            if (thisReturn.Errors.Count > 0)
            {
                thisReturn.Isvalid = false;
            }
            else
            {
                thisReturn.Isvalid = true;
            }
            return thisReturn;

        }
        public ConsumerValidation ValidateTokenize(Consumer thisClass)
        {
            ConsumerValidation thisReturn = new ConsumerValidation();
            List<string> getErrors = new List<string>();
            string[] validateArray = new string[] { "cardType", "cardnumber", "cardexpirymmyyy", "merchantid" };

            getErrors = ValidateProperties(validateArray, thisClass);

            foreach (string s in getErrors)
            {
                thisReturn.Errors.Add(s, "Null or Empty");

            }

            thisReturn = ValidateTokenizeKeyComponents(thisClass, thisReturn);
            if (thisReturn.Errors.Count > 0)
            {
                thisReturn.Isvalid = false;
            }
            else
            {
                thisReturn.Isvalid = true;
            }
            return thisReturn;

        }


        List<string> ValidateProperties(string[] ValidateAgainst, Consumer ValidateFor)
        {
            List<string> thisReturn = new List<string>();

            PropertyInfo[] properties = typeof(Consumer).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(ValidateFor, null);
                if (ValidateAgainst.Contains(property.Name.ToLower()))
                {
                    if (value == null)
                    {
                        thisReturn.Add(property.Name);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(value.ToString()))
                        {
                            thisReturn.Add(property.Name);
                        }
                    }
                }

            }

            return thisReturn;
        }

        //Validate Key MakeCCPayment stuff   
        public ConsumerValidation ValidateMakeCCKeyComponents(Consumer thisClass, ConsumerValidation thisReturn)
        {
            if (string.IsNullOrEmpty(thisClass.ExpiryMMYYYY.ToString()))
            {
                thisReturn.Errors.Add("ExpiryMMYYYY", "CardExpiraton Date cannot be null or empty");
            }

            if (string.IsNullOrEmpty(thisClass.Amount.ToString()))
            {
                thisReturn.Errors.Add("Amount", "Amount cannot be null or empty");
            }

            if (string.IsNullOrEmpty(thisClass.ZipCode.ToString()))
            {
                thisReturn.Errors.Add("ZipCode", "ZipCode cannot be null or empty");
            }


            if (string.IsNullOrEmpty(thisClass.PaymentID) != true)
            {
                if (thisClass.PaymentID.Length > 22)
                {
                    thisReturn.Errors.Add("paymentID", "Payment ID can be only 22 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.CardNumber) != true)
            {
                if (thisClass.CardNumber.Length > 19)
                {
                    thisReturn.Errors.Add("cardNumber", "Credit Card Number can only be 19 characters long");

                }
            }


            if (string.IsNullOrEmpty(thisClass.MerchantID) != true)
            {
                if (thisClass.MerchantID.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("merchantID", "Merchant Id can only have 10 characters");

                }
            }
            if (string.IsNullOrEmpty(thisClass.Amount.ToString()) != true)
            {
                if (thisClass.Amount.ToString().Length > 12)
                {
                    thisReturn.Errors.Add("amount", "Amount can only have 12 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.AccountNoDesc) != true)
            {
                if (thisClass.AccountNoDesc.ToString().Length > 3)
                {
                    thisReturn.Errors.Add("accountNumberDesc", "Account Number descriptor can only have 3 characters");

                }
            }

            if ((string.IsNullOrEmpty(thisClass.FirstName) != true) && (string.IsNullOrEmpty(thisClass.LastName) != true))
            {
                if (thisClass.FirstName.ToString().Length + thisClass.LastName.ToString().Length > 29)
                {
                    thisReturn.Errors.Add("firstName*lastName", "firstName and Last Name together should only have 29 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.Address1) != true)
            {
                if (thisClass.Address1.ToString().Length > 30)
                {
                    thisReturn.Errors.Add("Address1", "Address1 can only have 30 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.Address2) != true)
            {
                if (thisClass.Address2.ToString().Length > 28)
                {
                    thisReturn.Errors.Add("Address2", "Address2 can only have 28 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.City) != true)
            {
                if (thisClass.City.ToString().Length > 20)
                {
                    thisReturn.Errors.Add("City", "City can only have 20 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.State) != true)
            {
                if (thisClass.State.ToString().Length > 2)
                {
                    thisReturn.Errors.Add("state", "state can only have 2 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.ZipCode.ToString()) != true)
            {
                if (thisClass.ZipCode.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("zipCode", "ZipCode can only have 10 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.CountryCode) != true)
            {
                if (thisClass.CountryCode.ToString().Length > 2)
                {
                    thisReturn.Errors.Add("countryCode", "Country Code can only have 2 characters");

                }
            }

            return thisReturn;
        }


        //Validate Key ReverseCCPayment stuff   
        public ConsumerValidation ValidateReverseCCKeyComponents(Consumer thisClass, ConsumerValidation thisReturn)
        {

            if (string.IsNullOrEmpty(thisClass.PaymentID) != true)
            {
                if (thisClass.PaymentID.Length > 22)
                {
                    thisReturn.Errors.Add("paymentID", "Payment ID can be only 22 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.CardNumber) != true)
            {
                if (thisClass.CardNumber.Length > 19)
                {
                    thisReturn.Errors.Add("cardNumber", "Credit Card Number can only be 19 characters long");

                }
            }

            if (string.IsNullOrEmpty(thisClass.MerchantID) != true)
            {
                if (thisClass.MerchantID.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("merchantID", "Merchant Id can only have 10 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.Amount.ToString()) != true)
            {
                if (thisClass.Amount.ToString().Length > 12)
                {
                    thisReturn.Errors.Add("amount", "Amount can only have 12 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.AuthorizationCode) != true)
            {
                if (thisClass.AuthorizationCode.ToString().Length > 6)
                {
                    thisReturn.Errors.Add("AuthorizationCode", "AuthorizationCode can only have 6 characters");

                }
            }

            return thisReturn;
        }

        //Validate Key MakeACHPayment stuff   
        public ConsumerValidation ValidateMakeACHKeyComponents(Consumer thisClass, ConsumerValidation thisReturn)
        {
            if (string.IsNullOrEmpty(thisClass.PaymentID) != true)
            {
                if (thisClass.PaymentID.Length > 22)
                {
                    thisReturn.Errors.Add("paymentID", "Payment ID can be only 22 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.ACHAccountNumber.ToString()) != true)
            {
                if (thisClass.ACHAccountNumber.Length > 19)
                {
                    thisReturn.Errors.Add("achAccountNo", "ACH Account No can be only 19 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.ACHRoutingNumber) != true)
            {
                if (thisClass.ACHRoutingNumber.Length > 9)
                {
                    thisReturn.Errors.Add("achRoutingNo", "ACH Routing No can be only 9 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.MerchantID) != true)
            {
                if (thisClass.MerchantID.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("merchantID", "Merchant Id can only have 10 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.Amount.ToString()) != true)
            {
                if (thisClass.Amount.ToString().Length > 12)
                {
                    thisReturn.Errors.Add("amount", "Amount can only have 12 characters");

                }
            }

            if ((string.IsNullOrEmpty(thisClass.FirstName) != true) && (string.IsNullOrEmpty(thisClass.LastName) != true))
            {
                if (thisClass.FirstName.ToString().Length + thisClass.LastName.ToString().Length > 29)
                {
                    thisReturn.Errors.Add("firstName*lastName", "firstName and Last Name together should only have 29 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.Address1) != true)
            {
                if (thisClass.Address1.ToString().Length > 30)
                {
                    thisReturn.Errors.Add("Address1", "Address1 can only have 30 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.Address2) != true)
            {
                if (thisClass.Address2.ToString().Length > 28)
                {
                    thisReturn.Errors.Add("Address2", "Address2 can only have 28 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.City) != true)
            {
                if (thisClass.City.ToString().Length > 20)
                {
                    thisReturn.Errors.Add("City", "City can only have 20 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.State) != true)
            {
                if (thisClass.State.ToString().Length > 2)
                {
                    thisReturn.Errors.Add("state", "state can only have 2 characters");
                }
            }


            if (string.IsNullOrEmpty(thisClass.CountryCode) != true)
            {
                if (thisClass.CountryCode.ToString().Length > 2)
                {
                    thisReturn.Errors.Add("countryCode", "Country Code can only have 2 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.ZipCode.ToString()) != true)
            {
                if (thisClass.ZipCode.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("zipCode", "ZipCode can only have 10 characters");
                }
            }

            if (string.IsNullOrEmpty(thisClass.Email) != true)
            {
                if (thisClass.Email.ToString().Length > 50)
                {
                    thisReturn.Errors.Add("email", "Email Address can only have 50 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.IPAddress) != true)
            {
                if (thisClass.IPAddress.ToString().Length > 45)
                {
                    thisReturn.Errors.Add("ipAddress", "IpAddress can only have 45 characters");

                }
            }

            return thisReturn;
        }

        //Validate Key ReverseACHPayment stuff   
        public ConsumerValidation ValidateReverseACHKeyComponents(Consumer thisClass, ConsumerValidation thisReturn)
        {
            if (string.IsNullOrEmpty(thisClass.PaymentID) != true)
            {
                if (thisClass.PaymentID.Length > 22)
                {
                    thisReturn.Errors.Add("paymentID", "Payment ID can be only 22 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.ACHAccountNumber) != true)
            {
                if (thisClass.ACHAccountNumber.Length > 19)
                {
                    thisReturn.Errors.Add("achAccountNo", "ACH Account No can be only 19 characters long");
                }
            }

            if (string.IsNullOrEmpty(thisClass.ACHRoutingNumber) != true)
            {
                if (thisClass.ACHRoutingNumber.Length > 9)
                {
                    thisReturn.Errors.Add("achRoutingNo", "ACH Routing No can be only 9 characters long");
                }
            }
            if (string.IsNullOrEmpty(thisClass.MerchantID) != true)
            {
                if (thisClass.MerchantID.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("merchantID", "Merchant Id can only have 10 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.Amount.ToString()) != true)
            {
                if (thisClass.Amount.ToString().Length > 12)
                {
                    thisReturn.Errors.Add("amount", "Amount can only have 12 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.AuthorizationCode) != true)
            {
                if (thisClass.AuthorizationCode.ToString().Length > 6)
                {
                    thisReturn.Errors.Add("AuthorizationCode", "AuthorizationCode can only have 6 characters");

                }
            }

            if (string.IsNullOrEmpty(thisClass.TelecheckTraceID) != true)
            {
                if (thisClass.TelecheckTraceID.ToString().Length > 22)
                {
                    thisReturn.Errors.Add("TelecheckTraceID", "TelecheckTraceID can only have 22 characters");

                }
            }

            return thisReturn;
        }

        //Validate Key Tokanize stuff   
        public ConsumerValidation ValidateTokenizeKeyComponents(Consumer thisClass, ConsumerValidation thisReturn)
        {

            if (string.IsNullOrEmpty(thisClass.CardNumber) != true)
            {
                if (thisClass.CardNumber.Length > 19)
                {
                    thisReturn.Errors.Add("cardNumber", "Credit Card Number can only be 19 characters long");

                }
            }

            if (string.IsNullOrEmpty(thisClass.MerchantID) != true)
            {
                if (thisClass.MerchantID.ToString().Length > 10)
                {
                    thisReturn.Errors.Add("merchantID", "Merchant Id can only have 10 characters");

                }
            }

            return thisReturn;
        }

    }
}
