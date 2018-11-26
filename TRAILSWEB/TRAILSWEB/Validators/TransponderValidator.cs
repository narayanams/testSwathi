using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using TRAILSWEB.Models;
using TRAILSWEB.Validators;
using TRAILSWEB.ViewModels;

public static class TransponderValidator
{
    private static Tuple<int, int> GetTotals(string numbers)
    {
        int count = 0;
        int number = 0;
        int oddTotal = 0;
        int evenTotal = 0;

        foreach (var character in numbers)
        {
            count += 1;

            int.TryParse(character.ToString(), out number);

            if (count.IsOdd())
            {
                oddTotal += number;
            }
            else
            {
                evenTotal += number;
            }
        }

        return new Tuple<int, int>(oddTotal, evenTotal);
    }

    public static bool IsValid(string numberToCheck)
    {
        if (numberToCheck.Length == 13)
        {
            if (numberToCheck.IsNumeric())
            {
                string transponderNumber = numberToCheck.Substring(0, 8);
                string issuingAuthority = numberToCheck.Substring(8, 2);
                string stateCode = numberToCheck.Substring(10, 2);
                string checkDigitProvided = numberToCheck.Substring(12, 1);

                int checkDigit = 0;

                int.TryParse(checkDigitProvided, out checkDigit);

                string numberToValidate = numberToCheck.Substring(0, 12);

                Tuple<int, int> totals = GetTotals(numberToValidate);

                int oddValue = totals.Item1;
                int evenValue = totals.Item2;

                int validCheckDigit = (Math.Abs((((oddValue * 3) + evenValue) % 10) - 10) % 10);

                return checkDigit == validCheckDigit && issuingAuthority == "05";
            }
        }

        return false;
    }
    /*
     
         {

        bool returnFlag = false;
        List<string> errors = new List<string>();

        if (string.IsNullOrEmpty(model.UserName))
        {
            errors.Add("Username field cannot be empty.");
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            errors.Add("Password field cannot be empty.");

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
         */

    public static List<string>  ValidateReviewAddTransponderInfo(PreviewTransaction model)
    {
        bool isValid = true;
        List<string> errors = new List<string>();

        foreach (var element in model.Transponders)
        {
            if (string.IsNullOrEmpty(element.LicensePlateState))
            {
                errors.Add("License Plate State cannot be empty.<BR>");
                element.ValidationError = "License Plate State cannot be empty.";
                isValid = false;
            }
            else
            {
                if (CommonValidationUtilities.IsValidState(element.LicensePlateState) == false)
                {
                    errors.Add("License Plate State is invalid.<BR>");
                    element.ValidationError = "License Plate State is invalid.";
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(element.LicensePlate))
            {
                errors.Add("License Plate cannot be empty");
                element.ValidationError = "License Plate cannot be empty.";
                isValid = false;
            }
            else
            {
                if ((!string.IsNullOrEmpty(element.LicensePlate)) && (!string.IsNullOrEmpty(element.LicensePlateState)))
                {
                    OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult validationResults =
                        CommonValidationUtilities.IsValidLicensePlateNumber(element.LicensePlate, element.LicensePlateState);

                    if (validationResults ==
                        OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult
                            .InValidFloridaLicensePlateWithO)
                    {
                        errors.Add("License Plate Number is Invalid<BR>");
                        element.ValidationError = "License Plate Number is Invalid.";
                        isValid = false;

                    }
                    else if (validationResults ==
                             OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidLicensePlate)
                    {
                        errors.Add("License Plate Number is Invalid<BR>");
                        element.ValidationError = "License Plate Number is Invalid.";
                        isValid = false;

                    }
                }
            }
        }

        if (model.Payment.IsPaymentRequired) // Check if Payment information has been provided 
        {
            if (model.Payment.PaymentAmount == 0)
            {
                errors.Add("Payment Amount can not be zero.");
                model.ValidationError = "Payment Amount can not be zero.";
            }
            isValid = false;



            if (model.Payment.IsCCPayment) // Validation for CC Payment
            {
                //Validate CC Payment Data
                StringBuilder errorMessage = null;
                if (!TRAILSWEB.Validators.CommonValidationUtilities.ValidateCreditCardPaymentInformation(model.Payment.CreditCardType, model.Payment.CreditCardNumber, model.Payment.NameOnCard,
                        model.Payment.ExpirationMonth, model.Payment.ExpirationYear, Convert.ToDecimal(model.Payment.PrepaidTollsAmount), model.Payment.UseCardOnFile, out errorMessage))

                {
                    errors.Add(errorMessage.ToString());
                    model.ValidationError = errorMessage.ToString();
                    isValid = false;
                }
            }

            else //Validation for ACH Payment
            {
                StringBuilder errorMessage = null;
                if (!TRAILSWEB.Validators.CommonValidationUtilities.ValidateACHPaymentInfo(model.Payment, out errorMessage))
                {
                    errors.Add(errorMessage.ToString());
                    model.ValidationError = errorMessage.ToString();
                    isValid = false;
                }

            }

        }

        return errors;
    }
}
