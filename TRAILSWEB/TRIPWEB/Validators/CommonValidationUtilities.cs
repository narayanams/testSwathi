using OOCEA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TRIPWEB.Models;

namespace TRIPWEB.Validators
{
    public static class CommonValidationUtilities
    {
        public enum UsaStates { AA, AB, AE, AK, AL, AP, AR, AS, AZ, BC, CA, CO, CT, DC, DE, FL, GA, GS, GU, HI, IA, ID, IL, IN, KS, KY, LA, MA, MB, MD, ME, MI, MN, MO, MS, MT, NB, NC, ND, NE, NH, NJ, NL, NM, NS, NT, NU, NV, NY, OH, OK, ON, OP, OR, PA, PE, PR, QC, RI, SC, SD, SK, TN, TX, US, UT, VA, VI, VT, WA, WI, WV, WY, XX, YT, ZZ };
        public static string ValidateZipCode(this string zipCode)
        {

            return string.Empty;
        }

        public static string ValidateZipCodeExtension(this string zipCodeExt)
        {
            return string.Empty;
        }

        /// <summary>
        /// Checks if the Email format is valid or not
        /// </summary>
        /// <returns>true if valid email, false otherwise</returns>
        public static bool IsValidEmail(string emailValue)
        {
            // No email is valid
            if (emailValue == "")
                return true;

            if (emailValue.Substring(emailValue.Length - 1, 1) == ".")
                return false;
            Regex emailFormat = new Regex(Constants.RegEx.Email);
            return emailFormat.IsMatch(emailValue);
        }

        public static bool IsValidRequiredEmail(string emailValue)
        {
            // No email is not valid if empty
            if (string.IsNullOrEmpty(emailValue))
                return false;

            if (emailValue.Substring(emailValue.Length - 1, 1) == ".")
                return false;
            Regex emailFormat = new Regex(Constants.RegEx.Email);
            return emailFormat.IsMatch(emailValue);
        }

        /// <summary>
        /// Checks if the Email format is valid or not.Empty email is not valid email
        /// </summary>
        /// <returns>true if valid email, false otherwise</returns>
        public static bool ValidateEmail(string emailValue)
        {
            if (emailValue.Substring(emailValue.Length - 1, 1) == ".")
                return false;
            Regex emailFormat = new Regex(Constants.RegEx.Email);
            return emailFormat.IsMatch(emailValue);
        }

        /// <summary>
        /// Checks if the Account Number format is valid or not
        /// </summary>
        /// <returns>true if valid account number, false otherwise</returns>
        public static bool IsValidAccountNumber(string accountNumber)
        {
            Regex accountNumberFormat = new Regex(Constants.RegEx.CustomerAccount);

            return accountNumberFormat.IsMatch(accountNumber);
        }

        public static bool IsValidRequiredAccountNumber(string accountNumber)
        {
            // account no cannot be empty in resetusername and password
            if (string.IsNullOrEmpty(accountNumber))
                return false;

            Regex accountNumberFormat = new Regex(Constants.RegEx.CustomerAccount);

            return accountNumberFormat.IsMatch(accountNumber);
        }

        /// <summary>
        /// Checks any invalid special characters in Address1 and Address 2
        /// </summary>
        /// <param name="name">string contains the Address1 or Address2</param>
        /// <returns>returns false if valid address </returns>
        public static bool IsValidAddress(string addresss)
        {
            Regex addressFormat = new Regex(Constants.RegEx.Address);
            return addressFormat.IsMatch(addresss);

        }

        /// <summary>
        /// Checks any invalid special characters in city name
        /// </summary>
        /// <param name="name">string contains the city name</param>
        /// <returns>returns false if validcity name </returns>
        public static bool IsValidCity(string city)
        {
            Regex cityFormat = new Regex(Constants.RegEx.City);
            return cityFormat.IsMatch(city);
        }

        /// <summary>
        /// Checks if the Credit Card Number format is valid or not
        /// </summary>
        /// <returns>true if valid credit card number, false otherwise</returns>
        public static bool IsValidCreditCardNumber(string creditCardNumber)
        {
            Regex creditCardNumberFormat = new Regex(Constants.RegEx.CreditCardNumber);

            return creditCardNumberFormat.IsMatch(creditCardNumber);
        }

        /// <summary> Checks if the bank account number is valid or not </summary>
        /// <param name="bankAccountNumber"></param>
        /// <returns></returns>
        public static bool IsValidBankAccount(string bankAccountNumber)
        {
            Regex bankAccountNumberFormat = new Regex(Constants.RegEx.BankAccountNumber);

            return bankAccountNumberFormat.IsMatch(bankAccountNumber);
        }

        /// <summary> Checks if the routing number is valid or not</summary>
        /// <param name="routingNumber"></param>
        /// <returns></returns>
        public static bool IsValidRoutingNumber(string routingNumber)
        {
            Regex routingNumberFormat = new Regex(Constants.RegEx.BankRoutingNumber);

            return routingNumberFormat.IsMatch(routingNumber);
        }

        /// <summary>Checks if the amount entered is valid decimal amount</summary>
        /// <param name="amount">Dollar amount to be validated</param>
        /// <returns> true if the amount is valid, false otherwise</returns>
        public static bool IsValidAmount(string amount)
        {
            Regex dollarAmontFormat = new Regex(Constants.RegEx.Amount);
            return dollarAmontFormat.IsMatch(amount);
        }

        /// <summary>Checks if the amount entered is greater than or equal to web minimum payment amount for making onetime payment</summary>
        /// <param name="amount">Dollar amount to be validated</param>
        /// <returns> true if the amount is valid, false otherwise</returns>
        public static bool IsValidMinimumPaymentAmount(string amount, out decimal epassWebMimimumPaymentAmount)
        {
            epassWebMimimumPaymentAmount =
                decimal.Parse(ConfigurationManager.AppSettings["EPASSWebMinimumPayment"].ToString());
            return decimal.Parse(amount) >= epassWebMimimumPaymentAmount;
        }

        /// <summary>Checks if the amount entered is greater than or equal to web minimum payment amount for making onetime payment</summary>
        /// <param name="amount">Dollar amount to be validated</param>
        /// <returns> true if the amount is valid, false otherwise</returns>
        public static bool IsValidMaximumPaymentAmount(string amount, out decimal epassWebMaximumPaymentAmount)
        {
            epassWebMaximumPaymentAmount = decimal.Parse(ConfigurationManager.AppSettings["EPASSWebMaximiumPayment"]);
            return decimal.Parse(amount) <= epassWebMaximumPaymentAmount;
        }

        //public static bool IsValidMaximumPaymentAmount(decimal amount, out decimal epassWebMaximumPaymentAmount)
        //{
        //    epassWebMaximumPaymentAmount = decimal.Parse(ConfigurationManager.AppSettings["EPASSWebMaximiumPayment"]);
        //    return amount <= epassWebMaximumPaymentAmount;
        //}


        public static bool IsValidState(string state)
        {
            if (state == String.Empty)
            {
                return false;
            }
            if (Enum.IsDefined(typeof(UsaStates), state.ToUpper()))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidSecurityQuestion(int securityQuestion)
        {
            if (securityQuestion == 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidZip1(string zip1)
        {
            Regex zipFormat = new Regex(Constants.RegEx.Zip1);
            return zipFormat.IsMatch(zip1);
        }
        public static bool IsValidInternationalZip(string zip1)
        {
            Regex zipFormat = new Regex(Constants.RegEx.InternationalZip);
            return zipFormat.IsMatch(zip1);
        }

        public static bool IsValidZip2(string zip2)
        {
            Regex zipFormat = new Regex(Constants.RegEx.Zip2);
            return zipFormat.IsMatch(zip2);
        }

        public static bool ValidatePhoneExtensionField(string phExtNo)
        {
            Regex PhExtFormat = new Regex(Constants.RegEx.PhoneExtension);
            return PhExtFormat.IsMatch(phExtNo);
        }

        public static bool ValidateNameField(string name)
        {
            Regex NameFormat = new Regex(Constants.RegEx.Name);
            return NameFormat.IsMatch(name);
        }

        public static bool ValidateBusinessNameField(string name)
        {
            Regex NameFormat = new Regex(Constants.RegEx.BusinessName);
            return NameFormat.IsMatch(name);
        }

        public static bool ValidateMiddleInitial(string name)
        {
            Regex NameFormat = new Regex(Constants.RegEx.MiddleInitial);
            return NameFormat.IsMatch(name);
        }

        public static bool ValidateSecurityAnswer(string securityAnswer)
        {
            Regex SecurityAnswerFormat = new Regex(Constants.RegEx.SecurityAnswer);
            return SecurityAnswerFormat.IsMatch(securityAnswer);
        }

        public static bool IsValideTaxID(string taxId)
        {
            Regex NameFormat = new Regex(Constants.RegEx.TaxID);
            return NameFormat.IsMatch(taxId);
        }

        public static bool IsValidPassword(string password)
        {
            Regex passwordFormat = new Regex(Constants.RegEx.PasswordCustomerAccount);

            return passwordFormat.IsMatch(password);
        }

        public static bool IsValidLoginPassword(string password)
        {
            Regex passwordFormat = new Regex(Constants.RegEx.LoginPasswordCustomerAccount);

            return passwordFormat.IsMatch(password);
        }

        public static bool IsValidPIN(string pin)
        {
            Regex pinFormat = new Regex(Constants.RegEx.PIN);
            return pinFormat.IsMatch(pin);
        }

        public static bool IsValidUserName(string userName)
        {
            Regex userNameFormat = new Regex(Constants.RegEx.UserNameCustomerAccount);

            return userNameFormat.IsMatch(userName);
        }

        public static bool IsValidColor(string color)
        {
            Regex userNameFormat = new Regex(Constants.RegEx.Color);

            return userNameFormat.IsMatch(color);
        }
        public static bool IsValidName(string userName)
        {
            Regex userNameFormat = new Regex(Constants.RegEx.Name);

            return userNameFormat.IsMatch(userName);
        }

        /// <summary>
        /// Checks if the Email format is valid or not
        /// </summary>
        /// <returns>true if valid email, false otherwise</returns>
        public static bool IsBusinessName(string businessName)
        {
            Regex businessNameFormat = new Regex(Constants.RegEx.BusinessName);

            return businessNameFormat.IsMatch(businessName);
        }

        public static bool IsValidMonth(short month)
        {
            if ((month >= 1) && (month <= 12))
            {
                return true;
            }
            return false;
        }



        public static bool IsValidStatementYear(string year)
        {
            Regex yearFormat = new Regex(Constants.RegEx.YEAR);
            if (yearFormat.IsMatch(year))
            {
                if (int.Parse(year) <= int.Parse(DateTime.Now.Year.ToString()))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool IsValidCCYear(string year)
        {
            Regex yearFormat = new Regex(Constants.RegEx.YEAR);
            if (yearFormat.IsMatch(year))
            {
                if (int.Parse(year) >= int.Parse(DateTime.Now.Year.ToString()))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool IsValidYear(string year)
        {
            Regex yearFormat = new Regex(Constants.RegEx.YEAR);
            return yearFormat.IsMatch(year);
        }

        /// <summary>Validates the supplied phone number string matches the North American Phone Number standard</summary>
        /// <param name="fullTenDigigtPhoneNumber">Is expected to be all numbers no spaces, special characters or letters.</param>
        /// <returns>true if the passed number matches the North American Phone Number statndard (RegEx)</returns>
        public static bool ValidateNationalPhoneField(string fullTenDigigtPhoneNumber)
        {
            Regex phoneFormat = new Regex(Constants.RegEx.NorthAmericanPhoneNumbers);

            return phoneFormat.IsMatch(fullTenDigigtPhoneNumber);
        }

        public static bool ValidateInterNationalPhoneField(string fullTenDigigtPhoneNumber)
        {
            Regex phoneFormat = new Regex(Constants.RegEx.InternationalPhoneNumbers);

            return phoneFormat.IsMatch(fullTenDigigtPhoneNumber);
        }

        /// <summary>Validate Drivers licence.Florida had 13 characters first should be a alpa character and remaining should be numbers.
        /// For all others states drivers license should be validated for alpha numeric characters</summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">An EventArgs that contains the event data</param>
        public static Constants.Common.DriversLicenseValidationResult ValidateDriversLicense(string driversLicense,
            string driversLicenseState)
        {
            if (driversLicenseState == Constants.Common.DefaultStateCode)
            {
                if (driversLicense.Length != Constants.Common.FloridaDriversLicenceCharacterCount)
                {
                    return Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate;
                }

                else
                {
                    IEnumerable<char> stringQuery = driversLicense.Substring(1, driversLicense.Length - 1)
                        .TakeWhile(tchar => Constants.Common.NumericCharacters.Contains(tchar));
                    if (
                        !(Constants.Common.AlphabeticCharacters.Contains(driversLicense.Substring(0, 1)) &&
                          driversLicense.Length - 1 == stringQuery.Count()))
                    {
                        return Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate;
                    }
                }
                return Constants.Common.DriversLicenseValidationResult.ValidFloridaDriversLicensePlate;
            }
            else
            {
                IEnumerable<char> stringQuery = driversLicense
                    .TakeWhile(tchar => Constants.Common.AlphaNumericCharacters.Contains(tchar));
                if (driversLicense.Length != stringQuery.Count())
                {
                    return Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate;
                }
                return Constants.Common.DriversLicenseValidationResult.ValidDriversLicensePlate;
            }

        }

        /// <summary>
        /// Checks if license plate has only alpha numeric characters
        /// </summary>
        /// <param name="inputString">Gets the license plate as input string.</param>
        /// <returns>LicensePlateValidationResult enumeration based on validation.</returns>
        public static Constants.Common.LicensePlateValidationResult IsValidLicensePlateNumber(string licensePlate,
            string licensePlateState)
        {
            if (licensePlate == string.Empty)
            {
                return Constants.Common.LicensePlateValidationResult.InValidLicensePlate;
            }

            //check florida license does not have 'o' in them 
            if (licensePlateState == Constants.Common.DefaultStateCode)
            {
                if (licensePlate.ToUpper().Contains(Constants.Common.InValidFlLicensePlateCharacter))
                {
                    return Constants.Common.LicensePlateValidationResult.InValidFloridaLicensePlateWithO;
                }
            }

            IEnumerable<char> stringQuery = licensePlate
                .TakeWhile(tchar => Constants.Common.AlphaNumericCharacters.Contains(tchar));
            if (licensePlate.Length == stringQuery.Count())
            {
                return Constants.Common.LicensePlateValidationResult.ValidLicensePlate;
            }
            else
            {
                return Constants.Common.LicensePlateValidationResult.InValidLicensePlate;
            }

        }

        public static bool IsValidVehicleMake(string vehiclemake)
        {
            Regex userNameFormat = new Regex(Constants.RegEx.MakeModel);
            return userNameFormat.IsMatch(vehiclemake);
        }

        public static bool IsValidVehicleModel(string vehiclemodel)
        {
            Regex userNameFormat = new Regex(Constants.RegEx.MakeModel);
            return userNameFormat.IsMatch(vehiclemodel);
        }

        /// <summary>
        /// Checks any invalid special characters in city name
        /// </summary>
        /// <param name="name">string contains the city name</param>
        /// <returns>returns false if validcity name </returns>
        public static bool IsValidEnteredDate(string enteredDate)
        {
            if (DateTime.Parse(enteredDate) <= DateTime.Now)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Checks any invalid  or special characters in date
        /// </summary>
        /// <param name="name">string contains the date</param>
        /// <returns>returns false if invalid date </returns>
        public static bool IsValidDate(string enteredDate)
        {
            DateTime date;
            if (DateTime.TryParse(enteredDate, out date))
                return true;
            else
                return false;

        }

        /// <summary> Validate the Credit Card Number against the provided credit card type. </summary>
        /// <param name="ccNumber">Credit Card Number</param>
        /// <param name="ccType">Credit Card type</param>
        /// <returns>True or False</returns>
        public static bool IsValidCrediCardType(string ccNumber, string ccType)
        {
            Constants.Payments.CardType actualCCType = DetermineCardType(ccNumber);
            if (string.IsNullOrEmpty(ccType) || actualCCType.ToString().ToUpper() != ccType.ToUpper())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary> Retruns the Credit Card Type for the provideed credit card number. </summary>
        /// <param name="sCCNo">Credit Card Number</param>
        /// <returns> Credit card Type</returns>
        public static Constants.Payments.CardType DetermineCardType(string sCCNo)
        {
            //CardTypes              Prefix          Width
            //American Express       34, 37            15
            //Diners Club            300 to 305, 36    14
            //Carte Blanche          38                14
            //Discover               6011              16
            //EnRoute                2014, 2149        15
            //JCB                    3                 16
            //JCB                    2131, 1800        15
            //Master Card            51 to 55          16
            //Visa                   4                 13, 16

            try
            {
                switch (Convert.ToInt32(sCCNo.Substring(0, 2)))
                {
                    case 34:
                    case 37:
                        return Constants.Payments.CardType.AMEX;
                    case 36:
                        return Constants.Payments.CardType.Diners_Club;
                    case 38:
                        return Constants.Payments.CardType.Carte_Blanche;
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                        return Constants.Payments.CardType.MC;
                    case 65:
                        return Constants.Payments.CardType.DISC;
                    default:
                        switch (Convert.ToInt32(sCCNo.Substring(0, 4)))
                        {
                            case 2014:
                            case 2149:
                                return Constants.Payments.CardType.EnRoute;
                            case 2131:
                            case 1800:
                                return Constants.Payments.CardType.JCB;
                            case 6011:
                                return Constants.Payments.CardType.DISC;
                            default:
                                switch (Convert.ToInt16(sCCNo.Substring(0, 3)))
                                {
                                    case 300:
                                    case 301:
                                    case 302:
                                    case 303:
                                    case 304:
                                    case 305:
                                        return Constants.Payments.CardType.Diners_Club;
                                    case 644:
                                        return Constants.Payments.CardType.DISC;
                                    default:
                                        switch (Convert.ToInt32(sCCNo.Substring(0, 1)))
                                        {
                                            case 3:
                                                return Constants.Payments.CardType.JCB;
                                            case 4:
                                                return Constants.Payments.CardType.VISA;
                                            case 2:
                                            case 5:
                                                return Constants.Payments.CardType.MC;
                                            default:
                                                return Constants.Payments.CardType.Unknown;
                                        }
                                }
                        }
                }
            }
            catch
            {
                return Constants.Payments.CardType.Unknown;
            }
        }


        //public static bool ValidateACHPaymentInfo(PaymentInformation model, out StringBuilder errorMessage)
        //{
        //    errorMessage = new StringBuilder();

        //    if (string.IsNullOrEmpty(model.AccountHolderFirstName))
        //    {
        //        errorMessage = errorMessage.Append("First Name cannot be empty<BR>");
        //        return false;
        //    }
        //    if (CommonValidationUtilities.IsValidName(model.AccountHolderFirstName) == false)
        //    {
        //        errorMessage = errorMessage.Append("First Name is invalid.<BR>");
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(model.AccountHolderLastName))
        //    {
        //        errorMessage = errorMessage.Append("Last Name cannot be empty<BR>");
        //        return false;
        //    }
        //    if (CommonValidationUtilities.IsValidName(model.AccountHolderLastName) == false)
        //    {
        //        errorMessage = errorMessage.Append("Last Name is invalid.<BR>");
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(model.BankAccountNumber))
        //    {
        //        errorMessage = errorMessage.Append("Account Number cannot be empty<BR>");
        //        return false;
        //    }
        //    if (CommonValidationUtilities.IsValidBankAccount(model.BankAccountNumber) == false)
        //    {
        //        errorMessage = errorMessage.Append("Account Number is invalid.<BR>");
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(model.RoutingNumber))
        //    {
        //        errorMessage = errorMessage.Append("Routing Number cannot be empty<BR>");
        //        return false;
        //    }
        //    if (CommonValidationUtilities.IsValidRoutingNumber(model.RoutingNumber) == false)
        //    {
        //        errorMessage = errorMessage.Append("Routing Number is invalid.<BR>");
        //        return false;
        //    }
        //    if (decimal.Parse(model.PrepaidTollsAmount.ToString()) < 0)
        //    {
        //        errorMessage = errorMessage.Append("Please enter a valid payment amount.");
        //        return false;
        //    }

        //    decimal minmaxAmount = 0M;
        //    double paymentAmount = model.PaymentAmount > 0 ? model.PaymentAmount : model.PrepaidTollsAmount;
        //    if (!CommonValidationUtilities.IsValidMaximumPaymentAmount(paymentAmount.ToString(), out minmaxAmount))
        //    {
        //        errorMessage.Append(string.Format("Invalid amount {0}. Amount must be equal to or less than {1}.",
        //                                    paymentAmount.ToString("C"), minmaxAmount.ToString("C")));
        //        return false;
        //    }

        //    return true;
        //}

        
    }

}// namespace
