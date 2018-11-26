using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TRAILSWEB.TRAILSWEBServiceReference;
using TRAILSWEB.Models;
using TRAILSWEB.Validators;
using OOCEA.Framework.Common;
using TRAILSWEB.ViewModels;

public static class AccountValidator
{
    #region Read-Only Properties
    public static string AccountNumberValidatorExpression { get { return @"^([0-9]{1,7})$"; } }
    public static string PaymentIDValidatorExpression { get { return @"^([0-9]{1,12})$"; } }
    public static string EmailValidatorExpression { get { return @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"; } }
    public static string StreetAddressValidatorExpression { get { return @"^[a-zA-Z0-9\s\-\'\#\&\,\.\/]*$"; } }
    public static string CityValidatorExpression { get { return @"^[a-zA-Z\s\-\'\,\.]*$"; } }
    public static string ZipCodeValidatorExpression { get { return @"^\d{5}$|^\d(5)-\d{4}$"; } }
    public static string PersonNameValidatorExpression { get { return @"^[a-zA-Z\s\-\']+$"; } }
    public static string LicensePlateValidatorExpression { get { return @"^[a-zA-Z0-9]+$"; } }
    public static string DriverLicenseValidatorExpression { get { return @"^[a-zA-Z0-9\s]+$"; } }
    #endregion

    public static bool IsUniqueUserName(string value, RequiredParameters requiredParams)
    {
        bool isUnique = false;
        string response = string.Empty;

        using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
        {
            // NOTE: The Service as it exists 4/25 returns a boolean but also has a string OUT parameter. I'm told this will change.
            isUnique = serviceManager.UniqueUserName(requiredParams.SessionId, requiredParams.RequestNumber, value, out response);
        }

        if (string.IsNullOrEmpty(response))
        {
            return isUnique;
        }
        else
        {
            return false;
        }
    }

    public static bool IsValidEmail(string value, RequiredParameters requiredParams)
    {
        if (value.Substring(value.Length - 1, 1) == ".")
            return false;

        bool validEmailAddress = false;

        Regex emailFormat = new Regex(EmailValidatorExpression);
        validEmailAddress = emailFormat.IsMatch(value);

        return validEmailAddress;
    }

    public static bool IsValidAccountNumber(string accountNumber)
    {
        Regex accountNumberFormat = new Regex(AccountNumberValidatorExpression);

        return accountNumberFormat.IsMatch(accountNumber);
    }

    public static bool IsValidPaymentID(string paymentId)
    {
        Regex accountNumberFormat = new Regex(PaymentIDValidatorExpression);

        return accountNumberFormat.IsMatch(paymentId);
    }

    public static bool IsValidCity(string city)
    {
        Regex cityFormat = new Regex(CityValidatorExpression);
        return cityFormat.IsMatch(city);
    }

    public static bool IsValidAddress(string addressLine)
    {
        Regex addressFormat = new Regex(StreetAddressValidatorExpression);
        return addressFormat.IsMatch(addressLine);

    }

    public static bool IsValidZipCode(string zipCode)
    {
        Regex addressFormat = new Regex(ZipCodeValidatorExpression);
        return addressFormat.IsMatch(zipCode);
    }

    public static bool IsValidLicensePlate(string licensePlate, string state)
    {
        bool validLicensePlate = false;

        Regex licensePlateFormat = new Regex(LicensePlateValidatorExpression);

        // Validate Format
        validLicensePlate = licensePlateFormat.IsMatch(licensePlate);

        if (validLicensePlate)
        {
            // Verify Length
            validLicensePlate = licensePlate.Length <= 10;

            if (validLicensePlate)
            {
                // Apply special State of Florida Validation
                if (state == "FL")
                {
                    if (licensePlate.ToUpper().Contains('O'))
                        validLicensePlate = false;
                    else
                        validLicensePlate = true;
                }
            }
        }

        return validLicensePlate;
    }

    public static bool IsValidDriverLicense(string driverLicense)
    {
        bool isValid = false;

        Regex driverLicenseFormat = new Regex(DriverLicenseValidatorExpression);

        // Validate Format
        isValid = driverLicenseFormat.IsMatch(driverLicense);

        return isValid;
    }

    public static bool IsUniqueDriverLicense(string driverLicense, string state, RequiredParameters requiredParams)
    {
        string response = string.Empty;

        bool isUnique = false;

        using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
        {
            isUnique = serviceManager.UniqueDriversLicense(requiredParams.SessionId, driverLicense, state, out response);
        }

        if (string.IsNullOrEmpty(response))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUniqueLicensePlate(string licensePlate, string state, RequiredParameters requiredParams)
    {
        string response = string.Empty;

        bool isUnique = false;

        using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
        {
            isUnique = serviceManager.UniqueLicensePlate(requiredParams.SessionId, licensePlate, state, out response);
        }

        if (string.IsNullOrEmpty(response))
        {
            return isUnique;
        }
        else
        {
            return false;
        }
    }

    public static bool IsUniqueTransponderNumber(long transponderNumber, int transponderType, RequiredParameters requiredParams, out int returnTransponderType)
    {
        string response = string.Empty;

        int transType = -1;

        bool isUnique = false;

        using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
        {
            isUnique = serviceManager.UniqueTransponderNumber(requiredParams.SessionId, transponderNumber, out transType);
        }
        returnTransponderType = transType;

        return isUnique;

    }

    public static bool ValidateLogin(LoginModel model, out StringBuilder errorMessage, bool relaxValidation = false)
    {
        errorMessage = new StringBuilder();
        if (!model.DoAccountLogin)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                errorMessage = errorMessage.Append("User Name cannot be empty<BR>");
                return false;
            }
            else if (!relaxValidation && CommonValidationUtilities.IsValidUserName(model.UserName) == false)
            {
                errorMessage = errorMessage.Append("User name is invalid.<BR>");
                return false;
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                errorMessage = errorMessage.Append("Password cannot be empty<BR>");
                return false;
            }
            else if (CommonValidationUtilities.IsValidLoginPassword(model.Password) == false)
            {
                errorMessage = errorMessage.Append("Password is invalid.<BR>");
                return false;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(model.AccountNumber))
            {
                errorMessage = errorMessage.Append("Account Number cannot be empty<BR>");
                return false;
            }
            else if (CommonValidationUtilities.IsValidAccountNumber(model.AccountNumber) == false)
            {
                errorMessage = errorMessage.Append("Account Number is invalid.<BR>");
                return false;
            }
            if (string.IsNullOrEmpty(model.PinNumber))
            {
                errorMessage = errorMessage.Append("PIN Number cannot be empty<BR>");
                return false;
            }
            else if (CommonValidationUtilities.IsValidPIN(model.PinNumber) == false)
            {
                errorMessage = errorMessage.Append("PIN Number is invalid.<BR>");
                return false;
            }
        }
        return true;

    }

    public static bool ValidatePinLogin(LoginModel model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();

        if (string.IsNullOrEmpty(model.AccountNumber))
        {
            errorMessage = errorMessage.Append("Account Number cannot be empty<BR>");
            return false;
        }
        if (string.IsNullOrEmpty(model.PinNumber))
        {
            errorMessage = errorMessage.Append("Pin cannot be empty<BR>");
            return false;
        }
        return true;

    }

    public static bool ValidateForgotUsername(ForgotUsernamePassword model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();

        if (string.IsNullOrEmpty(model.RegisteredEmail))
        {
            errorMessage = errorMessage.Append("Registered Email cannot be empty<BR>");
            return false;
        }
        else if (CommonValidationUtilities.IsValidEmail(model.RegisteredEmail) == false)
        {
            errorMessage = errorMessage.Append("Registered Email is invalid.<BR>");
            return false;
        }
        if (((!string.IsNullOrEmpty(model.LicensePlateNumber)) && (!string.IsNullOrEmpty(model.LicenseStateSelected))) || (!string.IsNullOrEmpty(model.TransponderNumber)))
        {

            if ((!string.IsNullOrEmpty(model.LicensePlateNumber)) && (!string.IsNullOrEmpty(model.LicenseStateSelected)))
            {
                OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult validationResults =
                    CommonValidationUtilities.IsValidLicensePlateNumber(model.LicensePlateNumber,
                        model.LicenseStateSelected);

                if (validationResults ==
                    OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidFloridaLicensePlateWithO)
                {
                    errorMessage = errorMessage.Append("License Plate Number is Invalid<BR>");
                    return false;
                }
                else if (validationResults ==
                         OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidLicensePlate)
                {
                    errorMessage = errorMessage.Append("License Plate Number is Invalid<BR>");
                    return false;
                }

            }
            else if (!string.IsNullOrEmpty(model.TransponderNumber))
            {
                if (TransponderValidator.IsValid(model.TransponderNumber) == false)
                {
                    errorMessage = errorMessage.Append("Transponder Number is invalid.<BR>");
                    return false;
                }
            }
        }
        return true;

    }

    public static bool ValidateForgotPassword(ForgotUsernamePassword model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();


        if ((!string.IsNullOrEmpty(model.AccountNumber)) || (!string.IsNullOrEmpty(model.UserName)))
        {

            if (!string.IsNullOrEmpty(model.AccountNumber))
            {
                if (CommonValidationUtilities.IsValidAccountNumber(model.AccountNumber) == false)
                {
                    errorMessage = errorMessage.Append("Account Number is invalid.<BR>");
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                if (CommonValidationUtilities.IsValidUserName(model.UserName) == false)
                {
                    errorMessage = errorMessage.Append("User Name is invalid.<BR>");
                    return false;
                }
            }
        }
        else
        {
            errorMessage = errorMessage.Append("Either Account Number Or User Name should be provided");
            return false;
        }


        return true;

    }
    /*
    public static bool ValidateNewCustomerInfo(TransponderActivationViewModel model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();
        StringBuilder errorMessageReceived;

        if (!ValidateSaveCustomer(model.CustomerData, out errorMessageReceived))
        {
            errorMessage = errorMessageReceived;
            return false;
        }
        if (!ValidateSaveSecurityPreferences(model.SecurityPreferences, out errorMessageReceived))
        {
            errorMessage = errorMessageReceived;
            return false;
        }
        return true;
    }
    */
    /*
    public static bool ValidateSaveVehicleInfo(Vehicle model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();

        if (string.IsNullOrEmpty(model.LicensePlateNumber))
        {
            errorMessage = errorMessage.Append("License Plate Number cannot be empty.");
            return false;
        }
        if (string.IsNullOrEmpty(model.LicenseStateSelected))
        {
            errorMessage = errorMessage.Append("License Plate state cannot be empty.");
            return false;
        }
        if ((!string.IsNullOrEmpty(model.LicensePlateNumber)) && (!string.IsNullOrEmpty(model.LicenseStateSelected)))
        {
            OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult validationResults =
                CommonValidationUtilities.IsValidLicensePlateNumber(model.LicensePlateNumber,
                    model.LicenseStateSelected);

            if (validationResults ==
                OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidFloridaLicensePlateWithO)
            {
                errorMessage = errorMessage.Append("License Plate Number is Invalid<BR>");
                return false;
            }
            else if (validationResults ==
                     OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidLicensePlate)
            {
                errorMessage = errorMessage.Append("License Plate Number is Invalid<BR>");
                return false;
            }
        }
        if (string.IsNullOrEmpty(model.MakeSelected))
        {
            errorMessage = errorMessage.Append("Vehicle Make cannot be empty.");
            return false;
        }
        if (CommonValidationUtilities.IsValidVehicleMake(model.MakeSelected) == false)
        {
            errorMessage = errorMessage.Append("Vehicle Make is invalid.");
            return false;
        }

        if (string.IsNullOrEmpty(model.ModelSelected))
        {
            errorMessage = errorMessage.Append("Vehicle Model cannot be empty.");
            return false;
        }
        if (CommonValidationUtilities.IsValidVehicleModel(model.ModelSelected) == false)
        {
            errorMessage = errorMessage.Append("Vehicle Model is invalid.");
            return false;
        }
        if (string.IsNullOrEmpty(model.YearSelected.ToString()))
        {
            errorMessage = errorMessage.Append("Vehicle Year cannot be empty.");
            return false;
        }
        if (CommonValidationUtilities.IsValidYear(model.YearSelected.ToString()) == false)
        {
            errorMessage = errorMessage.Append("Vehicle Year is invalid.");
            return false;
        }
        if (string.IsNullOrEmpty(model.ColorSelected))
        {
            errorMessage = errorMessage.Append("Vehicle Color cannot be empty.");
            return false;
        }
        if (CommonValidationUtilities.IsValidColor(model.ColorSelected) == false)
        {
            errorMessage = errorMessage.Append("Vehicle Color is invalid.");
            return false;
        }

        return true;
    }
    */

    public static List<string> ValidateSaveSecurityData(SecurityInfoModel model)
    {

        bool returnFlag = false;
        List<string> errors = new List<string>();

        if (string.IsNullOrEmpty(model.UserName))
        {
            errors.Add("Username field cannot be empty.");
        }
        else if(!CommonValidationUtilities.IsValidUserName(model.UserName))
        {
            errors.Add("Username field cannot contain space and certain special characters.");
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            errors.Add("Password field cannot be empty.");

        }
        else if (!CommonValidationUtilities.IsValidPassword(model.Password))
        {
            errors.Add("Password field cannot contain space.");
        }
        
        if (string.IsNullOrEmpty(model.ConfirmPassword))
        {
            errors.Add("Confirm Password field cannot be empty.");

        }
        else if (model.Password != model.ConfirmPassword)
        {
            errors.Add("Password do not match.");

        }

        if (string.IsNullOrEmpty(model.Email))
        {
            errors.Add("Email field cannot be empty.");
        }
        else if (CommonValidationUtilities.IsValidEmail(model.Email) == false)
        {
            errors.Add("Email is invalid.");
        }

        return errors;

    }

    public static List<string> ValidateSaveCustomer(CustomerInfoModel model)
    {
        List<string> errors = new List<string>();
        const string isEmpty = "cannot be empty";
        const string isInvalid = "is invalid";

        // if customer is a person
        if (string.IsNullOrEmpty(model.BusinessName) && string.IsNullOrEmpty(model.BusinessContactName) && string.IsNullOrEmpty(model.BusinessTIN))
        {
            // validate primary user
            if (string.IsNullOrEmpty(model.PrimaryFirstName))
            {
                errors.Add($"First Name {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsValidName(model.PrimaryFirstName))
            {
                errors.Add($"First Name {isInvalid}");
            }

            if (string.IsNullOrEmpty(model.PrimaryLastName))
            {
                errors.Add($"Last Name {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsValidName(model.PrimaryLastName))
            {
                errors.Add($"Last Name {isInvalid}");
            }

            if (string.IsNullOrEmpty(model.PrimaryLicense))
            {
                errors.Add($"Driver's License {isEmpty}");
            }
            else
            {
                // add a check if the driver's license state is FL, then we valaidate it. If it is not
                // FL then we do not do any validation against the license number since as of this 
                // time we do not validate no FL driver's license number
                if (model.PrimaryLicenseState == Constants.Common.DefaultStateCode)
                {
                    Constants.Common.DriversLicenseValidationResult result =
                   CommonValidationUtilities.ValidateDriversLicense(model.PrimaryLicense, model.PrimaryLicenseState);
                    if (result == Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate ||
                        result == Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate)
                    {
                        errors.Add($"Driver's License {isInvalid}");
                    }
                }               
            }

            if (string.IsNullOrEmpty(model.Pin))
            {
                errors.Add($"Pin number {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsValidPIN(model.Pin))
            {
                errors.Add($"Pin number {isInvalid}");
            }

            // validate secondary user
            bool allBlank = string.IsNullOrEmpty(model.SecondaryFirstName) &&
                            string.IsNullOrEmpty(model.SecondaryLastName) &&
                            string.IsNullOrEmpty(model.SecondaryLicense) &&
                            string.IsNullOrEmpty(model.SecondaryLicenseState);
            bool allFilledIn = !string.IsNullOrEmpty(model.SecondaryFirstName) &&
                               !string.IsNullOrEmpty(model.SecondaryLastName) &&
                               !string.IsNullOrEmpty(model.SecondaryLicense) &&
                               !string.IsNullOrEmpty(model.SecondaryLicenseState);
            if (allFilledIn)
            {
                if (!CommonValidationUtilities.IsValidName(model.SecondaryFirstName))
                    errors.Add($"Secondary user's First Name {isInvalid}");
                if (!CommonValidationUtilities.IsValidName(model.SecondaryLastName))
                    errors.Add($"Secondary user's Last Name {isInvalid}");
                
                Constants.Common.DriversLicenseValidationResult result =
                    CommonValidationUtilities.ValidateDriversLicense(model.SecondaryLicense, model.SecondaryLicenseState);
                if (result == Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate ||
                    result == Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate)
                {
                    errors.Add($"Co-applicant's Driver's License {isInvalid}");
                }
                if ((model.PrimaryLicense == model.SecondaryLicense) && (model.PrimaryLicenseState == model.SecondaryLicenseState))
                { errors.Add($"Applicant, Co-applicant's Driver's License and State cannot be the same. {isInvalid}"); }
            }
            else if (!allBlank)
            {
                if (string.IsNullOrEmpty(model.SecondaryFirstName))
                    errors.Add($"Secondary user's First Name {isEmpty}");
                else
                {
                    if (!CommonValidationUtilities.IsValidName(model.SecondaryFirstName))
                        errors.Add($"Secondary user's First Name {isInvalid}");
                }
                if (string.IsNullOrEmpty(model.SecondaryLastName))
                    errors.Add($"Secondary user's Last Name {isEmpty}");
                else
                {
                    if (!CommonValidationUtilities.IsValidName(model.SecondaryLastName))
                        errors.Add($"Secondary user's Last Name {isInvalid}");
                }
                if (string.IsNullOrEmpty(model.SecondaryLicense) || string.IsNullOrEmpty(model.SecondaryLicenseState))
                    errors.Add($"Secondary user's Driver's License or state {isEmpty}");
                else
                {
                    Constants.Common.DriversLicenseValidationResult result =
                        CommonValidationUtilities.ValidateDriversLicense(model.SecondaryLicense, model.SecondaryLicenseState);
                    if (result == Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate ||
                        result == Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate)
                    {
                        errors.Add($"Secondary user's Driver's License information {isInvalid}");
                    }
                }
            }

        }
        else
        {
            // validate business fields
            if (string.IsNullOrEmpty(model.BusinessName))
            {
                errors.Add($"Business Name {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsBusinessName(model.BusinessName))
            {
                errors.Add($"Business Name {isInvalid}");
            }

            if (string.IsNullOrEmpty(model.BusinessContactName))
            {
                errors.Add($"Business Contact Name {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsBusinessName(model.BusinessContactName))
            {
                errors.Add($"Business Contact Name {isInvalid}");
            }

            if (string.IsNullOrEmpty(model.BusinessTIN))
            {
                errors.Add($"Business EIN# {isEmpty}");
            }
            else if (!CommonValidationUtilities.IsValideTaxID(model.BusinessTIN))
            {
                errors.Add($"Business EIN# {isInvalid}");
            }
        }


        return errors;
    }

    public static List<string> ValidateSaveContact(ContactInfoModel model)
    {

        List<string> errors = new List<string>();

        if (string.IsNullOrEmpty(model.AddressLine1))
        {
            errors.Add("Address Line 1 cannot be empty.");
        }
        else if (CommonValidationUtilities.IsValidAddress(model.AddressLine1) == false)
        {
            errors.Add("Address Line 1 is invalid.");
        }
        if (!string.IsNullOrEmpty(model.AddressLine2))
        {
            if (CommonValidationUtilities.IsValidAddress(model.AddressLine2) == false)
            {
                errors.Add("Address Line 2 is invalid.");
            }
        }
        if (string.IsNullOrEmpty(model.City))
        {
            errors.Add("City cannot be empty.");
        }
        else if (CommonValidationUtilities.IsValidCity(model.City) == false)
        {
            errors.Add("City is invalid.");
        }

        if (string.IsNullOrEmpty(model.State))
        {
            errors.Add("Address State cannot be empty.");
        }
        else if (CommonValidationUtilities.IsValidState(model.State) == false)
        {
            errors.Add("Address State is invalid.");
        }

        if (string.IsNullOrEmpty(model.ZipCode))
        {
            errors.Add("Zip Code Cannot be empty.");
        }
        else if (CommonValidationUtilities.IsValidZip1(model.ZipCode) == false)
        {
            errors.Add("Zip Code is invalid.");
        }

        if (!string.IsNullOrEmpty(model.EveningPhone))
        {
            if (CommonValidationUtilities.ValidatePhoneField(model.EveningPhone) == false)
            {
                errors.Add("Phone Number is invalid.");
            }
        }
        if (!string.IsNullOrEmpty(model.DayPhone))
        {
            if (CommonValidationUtilities.ValidatePhoneField(model.DayPhone) == false)
            {
                errors.Add("Day Phone Number is invalid.");
            }
        }
        if (!string.IsNullOrEmpty(model.DayPhoneExt))
        {
            if (CommonValidationUtilities.ValidatePhoneExtensionField(model.DayPhoneExt) == false)
            {
                errors.Add("Day Phone extention Number is invalid.");
            }
        }

        return errors;
    }
    public static bool ValidateSaveCustomer(Customer model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();

        if (model.IsPersonalAccount)
        {
            if (string.IsNullOrEmpty(model.FirstName))
            {
                errorMessage = errorMessage.Append("First Name is required.");
                return false;
            }
            else if (CommonValidationUtilities.IsValidName(model.FirstName) == false)
            {
                errorMessage = errorMessage.Append("First Name is invalid.");
                return false;
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                errorMessage = errorMessage.Append("Last Name is required.");
                return false;
            }
            else if (CommonValidationUtilities.IsValidName(model.LastName) == false)
            {
                errorMessage = errorMessage.Append("Last Name is invalid.");
                return false;
            }
            if (!string.IsNullOrEmpty(model.CoUserFirstName))
            {
                if (CommonValidationUtilities.IsValidName(model.CoUserFirstName) == false)
                {
                    errorMessage = errorMessage.Append("Secondary User First Name is invalid.");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(model.CoUserLastName))
            {
                if (CommonValidationUtilities.IsValidName(model.CoUserLastName) == false)
                {
                    errorMessage = errorMessage.Append("Secondary User Last Name is invalid.");
                    return false;
                }
            }

        }
        else
        {
            if (string.IsNullOrEmpty(model.BusinessName))
            {
                errorMessage = errorMessage.Append("Business Name is required.");
                return false;
            }
            else if (CommonValidationUtilities.ValidateBusinessNameField(model.BusinessName) == false)
            {
                errorMessage = errorMessage.Append("Business Name is invalid.");
                return false;
            }
            if (string.IsNullOrEmpty(model.BusinessAttentionName))
            {
                errorMessage = errorMessage.Append("Business Contact Name is required.");
                return false;
            }
            else if (CommonValidationUtilities.ValidateNameField(model.BusinessAttentionName) == false)
            {
                errorMessage = errorMessage.Append("Business Contact Name is invalid..");
                return false;
            }
            if (string.IsNullOrEmpty(model.BusinessTaxID))
            {
                errorMessage = errorMessage.Append("Business TaxID is required.");
                return false;
            }
            else if (CommonValidationUtilities.IsValideTaxID(model.BusinessTaxID) == false)
            {
                errorMessage = errorMessage.Append("Business TaxID is invalid.");
                return false;
            }
        }
        if (string.IsNullOrEmpty(model.AddressLine1))
        {
            errorMessage = errorMessage.Append("Address Line 1 is required");
            return false;
        }
        else if (CommonValidationUtilities.IsValidAddress(model.AddressLine1) == false)
        {
            errorMessage = errorMessage.Append("Address Line 1 is invalid.");
            return false;
        }
        if (!string.IsNullOrEmpty(model.AddressLine2))
        {
            if (CommonValidationUtilities.IsValidAddress(model.AddressLine2) == false)
            {
                errorMessage = errorMessage.Append("Address Line 2 is invalid.");
                return false;
            }
        }
        if (string.IsNullOrEmpty(model.City))
        {
            errorMessage = errorMessage.Append("City is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidCity(model.City) == false)
        {
            errorMessage = errorMessage.Append("City is invalid.");
            return false;
        }

        if (string.IsNullOrEmpty(model.AddressStateSelected))
        {
            errorMessage = errorMessage.Append("Address State is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidState(model.AddressStateSelected) == false)
        {
            errorMessage = errorMessage.Append("Address State is invalid.");
            return false;
        }

        if (string.IsNullOrEmpty(model.ZipCode))
        {
            errorMessage = errorMessage.Append("Zip Code is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidZip1(model.ZipCode) == false)
        {
            errorMessage = errorMessage.Append("Zip Code is invalid.");
            return false;
        }

        if (!string.IsNullOrEmpty(model.PhoneNumber) && !CommonValidationUtilities.ValidatePhoneField(model.PhoneNumber))
        {
            errorMessage = errorMessage.Append("Phone Number is invalid.");
            return false;
        }

        if (string.IsNullOrEmpty(model.WorkPhoneNumber))
        {
            errorMessage = errorMessage.Append("Day Phone Number is required.");
            return false;
        }
        else
        {
            if (CommonValidationUtilities.ValidatePhoneField(model.WorkPhoneNumber) == false)
            {
                errorMessage = errorMessage.Append("Day Phone Number is invalid.");
                return false;
            }
        }
        if (!string.IsNullOrEmpty(model.WorkPhoneNumberExtn))
        {
            if (CommonValidationUtilities.ValidatePhoneExtensionField(model.WorkPhoneNumberExtn) == false)
            {
                errorMessage = errorMessage.Append("Day Phone extention Number is invalid.");
                return false;
            }
        }

        if (string.IsNullOrEmpty(model.EmailAddress))
        {
            errorMessage = errorMessage.Append("Email is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidEmail(model.EmailAddress) == false)
        {
            errorMessage = errorMessage.Append("Email is invalid.");
            return false;
        }

        if (model.IsPersonalAccount)
        {
            if (string.IsNullOrEmpty(model.DriversLicense))
            {
                errorMessage = errorMessage.Append("Drivers License is required.");
                return false;
            }
            if (AccountValidator.IsValidDriverLicense(model.DriversLicense) == false)
            {
                errorMessage = errorMessage.Append("Drivers License is invalid.");
                return false;
            }
            if (string.IsNullOrEmpty(model.DriversLicenseStateSelected))
            {
                errorMessage = errorMessage.Append("Drivers License State is required.");
                return false;
            }
            else if (CommonValidationUtilities.IsValidState(model.DriversLicenseStateSelected) == false)
            {
                errorMessage = errorMessage.Append("Drivers License State is invalid.");
                return false;
            }
            else
            {
                Constants.Common.DriversLicenseValidationResult driversLicenseValidationResults =
                    CommonValidationUtilities.ValidateDriversLicense(model.DriversLicense,
                        model.DriversLicenseStateSelected);
                if (driversLicenseValidationResults ==
                    Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate)
                {
                    errorMessage = errorMessage.Append("Florida Drivers License is Invalid.");
                    return false;
                }

                if (driversLicenseValidationResults ==
                    Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate)
                {
                    errorMessage = errorMessage.Append("Drivers License is Invalid.");
                    return false;
                }
            }

            if ((!string.IsNullOrEmpty(model.CoUserFirstName)) || (!string.IsNullOrEmpty(model.CoUserLastName)))
            {
                if (string.IsNullOrEmpty(model.CoUserLicensePlateNumber))
                {
                    errorMessage = errorMessage.Append("Secondary User Driver's License cannot be blank if Secondary User's First or Last Names are provided.");
                    return false;
                }
                if (string.IsNullOrEmpty(model.CoUserLicenseStateSelected))
                {
                    errorMessage = errorMessage.Append("Secondary User Driver's License State cannot be blank if Secondary User's First or Last Names are provided.");
                    return false;
                }
            }


            if (!string.IsNullOrEmpty(model.CoUserLicensePlateNumber))
            {
                if (AccountValidator.IsValidDriverLicense(model.CoUserLicensePlateNumber) == false)
                {
                    errorMessage = errorMessage.Append("Secondary User's Drivers License is invalid.");
                    return false;
                }
            }
            else if (!string.IsNullOrEmpty(model.CoUserLicenseStateSelected))
            {
                if (CommonValidationUtilities.IsValidState(model.CoUserLicenseStateSelected) == false)
                {
                    errorMessage = errorMessage.Append("Secondary User's Drivers License State is invalid.");
                    return false;
                }
            }
            else if (!string.IsNullOrEmpty(model.CoUserLicensePlateNumber) && !string.IsNullOrEmpty(model.CoUserLicenseStateSelected))
            {
                Constants.Common.DriversLicenseValidationResult driversLicenseValidationResults = CommonValidationUtilities.ValidateDriversLicense(model.CoUserLicensePlateNumber, model.CoUserLicenseStateSelected);
                if (driversLicenseValidationResults == Constants.Common.DriversLicenseValidationResult.InValidFloridaDriversLicensePlate)
                {
                    errorMessage = errorMessage.Append("Secondary User's Florida Drivers License is Invalid.");
                    return false;
                }

                if (driversLicenseValidationResults == Constants.Common.DriversLicenseValidationResult.InValidDriversLicensePlate)
                {
                    errorMessage = errorMessage.Append("Secondary User's Drivers License is Invalid.");
                    return false;
                }

            }
        }

        return true;
    }


    public static bool ValidateSaveSecurityPreferences(RegisterModel model, out StringBuilder errorMessage)
    {
        errorMessage = new StringBuilder();

        if (string.IsNullOrEmpty(model.UserName))
        {
            errorMessage = errorMessage.Append("User Name is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidUserName(model.UserName) == false)
        {
            errorMessage = errorMessage.Append("User Name is invalid.");
            return false;
        }
        if (string.IsNullOrEmpty(model.Password))
        {
            errorMessage = errorMessage.Append("Password cannot be empty.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidPassword(model.Password) == false)
        {
            errorMessage = errorMessage.Append("Password is invalid.");
            return false;
        }
        if (string.IsNullOrEmpty(model.ConfirmPassword))
        {
            errorMessage = errorMessage.Append("Confirm Password is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidPassword(model.ConfirmPassword) == false)
        {
            errorMessage = errorMessage.Append("Confirm Password is invalid.");
            return false;
        }
        if (string.IsNullOrEmpty(model.ConfirmPassword))
        {
            errorMessage = errorMessage.Append("Confirm Password is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidPassword(model.ConfirmPassword) == false)
        {
            errorMessage = errorMessage.Append("Confirm Password is invalid.");
            return false;
        }
        if (model.Password != model.ConfirmPassword)
        {
            errorMessage = errorMessage.Append("passwords do not match.");
            return false;
        }
        if (string.IsNullOrEmpty(model.Email))
        {
            errorMessage = errorMessage.Append("Email is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidEmail(model.Email) == false)
        {
            errorMessage = errorMessage.Append("Email is invalid..");
            return false;
        }

        if (string.IsNullOrEmpty(model.PinNumber))
        {
            errorMessage = errorMessage.Append("PIN Number is required.");
            return false;
        }
        else if (CommonValidationUtilities.IsValidPIN(model.PinNumber) == false)
        {
            errorMessage = errorMessage.Append("PIN Number is invalid.");
            return false;
        }


        return true;
    }


    public static List<string> ValidateRegisterModel(LoginAndRegister loginRegisterModel)
    {
        bool returnFlag = true;
        Guid newParsedSessionId;
        List<string> errors = new List<string>();

        returnFlag = Guid.TryParse(loginRegisterModel.RegisterModel.SessionId, out newParsedSessionId);

        if (!returnFlag)
            errors.Add("Your Login session has expired, pleae login again with Account and Pin");

        if (string.IsNullOrEmpty(loginRegisterModel.RegisterModel.UserName) || string.IsNullOrEmpty(loginRegisterModel.RegisterModel.ConfirmUserName))
        {
            returnFlag = false;
            errors.Add("Please provide Username or Confirm Username.");
        }
        else if (loginRegisterModel.RegisterModel.UserName != loginRegisterModel.RegisterModel.ConfirmUserName)
        {
            returnFlag = false;
            errors.Add("The Username and Confirm Username do not match.");
        }
        else if (!CommonValidationUtilities.IsValidUserName(loginRegisterModel.RegisterModel.UserName))
        {
            returnFlag = false;
            errors.Add("Username is invalid, Username can not contain space and certain special characters");
        }

        if (string.IsNullOrEmpty(loginRegisterModel.RegisterModel.Password) || string.IsNullOrEmpty(loginRegisterModel.RegisterModel.ConfirmPassword))
        {
            returnFlag = false;
            errors.Add("Please provide Password and Confirm Password.");
        }
        else if (loginRegisterModel.RegisterModel.Password != loginRegisterModel.RegisterModel.ConfirmPassword)
        {
            returnFlag = false;
            errors.Add("The Password and Confirm Password do not match.");
        }
        else if (!CommonValidationUtilities.IsValidPassword(loginRegisterModel.RegisterModel.Password))
        {
            returnFlag = false;
            errors.Add("Password is invalid, password should not contain space");
        }

        if (string.IsNullOrEmpty(loginRegisterModel.RegisterModel.Email))
        {
            returnFlag = false;
            errors.Add("Email cannot be empty.");
        }
        else if (!CommonValidationUtilities.IsValidEmail(loginRegisterModel.RegisterModel.Email))
        {
            returnFlag = false;
            errors.Add("Email is invalid.");
        }

        return errors;
    }

}
