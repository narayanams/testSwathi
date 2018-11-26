using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OOCEA.Framework.Common;
using TRIPWEB.Models;
using System.Web.WebPages;

namespace TRIPWEB.Validators
{
    public static class ReservationValidator
    {
        public static bool ValidateNewResesrvationInfo(Reservation model,out List<string> errors)
        {
            errors = null;
            errors = new List<string>();
            var isValid = true;
            if ((model.TripCurrentStatus == TripStatus.Other)||(model.TripCurrentStatus == TripStatus.Reserved))
            {
                if (string.IsNullOrEmpty(model.ContactInfo.FirstName))
                {
                    errors.Add("First Name is required.");
                    return false;
                }
                else if (CommonValidationUtilities.IsValidName(model.ContactInfo.FirstName) == false)
                {
                    errors.Add("First Name is invalid.");
                    return false;
                }

                if (string.IsNullOrEmpty(model.ContactInfo.LastName))
                {
                    errors.Add("Last Name is required.");
                    return false;
                }
                else if (CommonValidationUtilities.IsValidName(model.ContactInfo.LastName) == false)
                {
                    errors.Add("Last Name is invalid.");
                    return false;
                }

                if (string.IsNullOrEmpty(model.ContactInfo.AddressLine1))
                {
                    errors.Add("Address Line 1 cannot be empty.");
                    return false;
                }
                else if (CommonValidationUtilities.IsValidAddress(model.ContactInfo.AddressLine1) == false)
                {
                    errors.Add("Address Line 1 is invalid.");
                    return false;
                }

                if (!string.IsNullOrEmpty(model.ContactInfo.AddressLine2) &&
                    (CommonValidationUtilities.IsValidAddress(model.ContactInfo.AddressLine2) == false))
                {
                        errors.Add("Address Line 2 is invalid.");
                        return false;
                }

                if (string.IsNullOrEmpty(model.ContactInfo.City))
                {
                    errors.Add("City cannot be empty.");
                    return false;
                }
                else if (CommonValidationUtilities.IsValidCity(model.ContactInfo.City) == false)
                {
                    errors.Add("City is invalid.");
                    return false;
                }

                if (string.IsNullOrEmpty(model.ContactInfo.AddressCountrySelected))
                {
                    errors.Add("Address country cannot be empty.");
                    return false;
                }

                if (model.ContactInfo.AddressCountrySelected.ToUpper() == "USA")
                {
                    if (string.IsNullOrEmpty(model.ContactInfo.AddressStateSelected))
                    {
                        errors.Add("Address State cannot be empty.");
                        return false;
                    }
                    else if (CommonValidationUtilities.IsValidState(model.ContactInfo.AddressStateSelected) == false)
                    {
                        errors.Add("Address State is invalid.");
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(model.ContactInfo.ZipCode))
                {
                    errors.Add("Zip Code Cannot be empty.");
                    return false;
                }
                else
                {
                    if (model.ContactInfo.AddressCountrySelected.ToUpper() == "USA")
                    {
                        if (CommonValidationUtilities.IsValidZip1(model.ContactInfo.ZipCode) == false)
                        {
                            errors.Add("Zip Code is invalid.");
                            return false;
                        }
                    }
                    else
                    {
                        if (CommonValidationUtilities.IsValidInternationalZip(model.ContactInfo.ZipCode) == false)
                        {
                            errors.Add("Zip Code is invalid.");
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(model.ContactInfo.PhoneNumber))
                {
                    errors.Add("Phone Number is required.");
                    return false;
                }
                else
                {
                    if (model.ContactInfo.AddressCountrySelected.ToUpper() == "USA")
                    {
                        if (!CommonValidationUtilities.ValidateNationalPhoneField(model.ContactInfo.PhoneNumber))
                        {
                            errors.Add("Please enter a valid USA Phone Number.");
                            return false;
                        }
                    }
                    else
                    {
                        if (!CommonValidationUtilities.ValidateInterNationalPhoneField(model.ContactInfo.PhoneNumber))
                        {
                            errors.Add("Please enter a valid International Phone Number.");
                            return false;
                        }
                    }

                }
            }

            //Validation for Closed Trip
            
            if (!(model.TripCurrentStatus == TripStatus.Closed))
            {
                if (string.IsNullOrEmpty(model.ContactInfo.EmailAddress))
                {
                    errors.Add("Email is required.");
                    return false;
                }
                else if (CommonValidationUtilities.IsValidEmail(model.ContactInfo.EmailAddress) == false)
                {
                    errors.Add("Email is invalid.");
                    return false;

                }

                if (string.IsNullOrEmpty(model.ContactInfo.PhoneNumber))
                {
                    errors.Add("PhoneNumber is required.");
                    return false;
                }
                else 
                {

                    if (!CommonValidationUtilities.ValidateInterNationalPhoneField(model.ContactInfo.PhoneNumber))
                    {
                        errors.Add("Please enter a valid International Phone Number.");
                        return false;
                    }

                }
            }

        
            if ((model.TripCurrentStatus == TripStatus.Other) || (model.TripCurrentStatus == TripStatus.Reserved))
            {
                if (string.IsNullOrEmpty(model.ArrivalDate))
                {
                    errors.Add("Arrival Date is required.");
                    return false;
                }
                if (string.IsNullOrEmpty(model.ArrivalTimeSelected))
                {
                    errors.Add("Arrival Time is required.");
                    return false;
                }
                DateTime arrivalDate = model.ArrivalDate.AsDateTime().AddMinutes(int.Parse(model.ArrivalTimeSelected));
                if(arrivalDate <= DateTime.Now)
                {
                    errors.Add("All reservations must be made for a future date and time (EST). Please check your Start Date and try again.");
                    return false;
                }
            }
            if ((model.TripCurrentStatus == TripStatus.Other) || (model.TripCurrentStatus == TripStatus.Reserved) ||(model.TripCurrentStatus == TripStatus.Started) )
            {
                if (string.IsNullOrEmpty(model.DepartureDate))
                {
                    errors.Add("Departure Date is required.");
                    return false;
                }
                if (string.IsNullOrEmpty(model.DepartureTimeSelected))
                {
                    errors.Add("Departure Time is required.");
                    return false;
                }
                DateTime departureDate = model.DepartureDate.AsDateTime().AddMinutes(int.Parse(model.DepartureTimeSelected));
                if (departureDate <= DateTime.Now)
                {
                    errors.Add("All reservations must be made for a future date and time (EST). Please ensure your End Date is greater than your Start Date and try again.");
                    return false;
                }
            }
            if ((model.TripCurrentStatus == TripStatus.Other) || (model.TripCurrentStatus == TripStatus.Reserved))
                {
                if (string.IsNullOrEmpty(model.RentalAgencySelected))
                {
                    errors.Add("Rental Agency cannot be empty.");
                    return false;
                }
            }
            if (model.TripCurrentStatus == TripStatus.Started)
            {
                OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult validationResults = CommonValidationUtilities.IsValidLicensePlateNumber(model.VehicleInfo.LicensePlate, model.VehicleInfo.LicenseStateSelected);
                if (string.IsNullOrEmpty(model.VehicleInfo.LicensePlate))
                {
                    errors.Add("License Plate Number is Required<BR>");
                    return false;
                }
                if (string.IsNullOrEmpty(model.VehicleInfo.LicenseStateSelected))
                {
                    errors.Add("License Plate State is required<BR>");
                    return false;
                }
                if (validationResults ==
                    OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidFloridaLicensePlateWithO)
                {
                    errors.Add("License Plate Number is Invalid<BR>");
                    return false;
                }
                else if (validationResults ==
                         OOCEA.Framework.Common.Constants.Common.LicensePlateValidationResult.InValidLicensePlate)
                {
                    errors.Add("License Plate Number is Invalid<BR>");
                    return false;
                }
            }

            if (!(model.TripCurrentStatus == TripStatus.Closed))
            {
                List<string> errorString = new List<string>();
                if (!(model.PaymentInfo.UseCardOnFile))
                {
                    isValid = ValidateCreditCardPaymentInformation(model.PaymentInfo.CreditCardType,
                        model.PaymentInfo.CreditCardNumber, model.PaymentInfo.NameOnCard,
                        model.PaymentInfo.ExpirationMonth,
                        model.PaymentInfo.ExpirationYear, out errorString);
                }
                if ((!isValid) && (errorString.Count > 0))
                {
                    errors = errors.Concat(errorString).ToList();
                    return false;
                }
            }

            return true;
        }

        

        public static bool ValidateCreditCardPaymentInformation(string cardType, string cardNumber, string nameOnCard, string expirationMonth, string expirationYear, out List<string> errors)
        {
            Nullable<DateTime> expirationDate;
            //st = new StringBuilder();
            errors = new List<string>();

            if (cardType == Constants.Common.WebSelectOne)
            {
                errors.Add("Please select a valid credit card type.");
                return false;
            }

            if (string.IsNullOrEmpty(cardNumber)
                || !CommonValidationUtilities.IsValidCreditCardNumber(cardNumber)
                || !CommonValidationUtilities.IsValidCrediCardType(cardNumber, cardType))
            {
                errors.Add("Please enter a valid credit card number.");
                return false;
            }

            if (string.IsNullOrEmpty(nameOnCard) || (!CommonValidationUtilities.IsValidName(nameOnCard)))
            {
                errors.Add("Please enter valid name on Credit Card.");
                return false;
            }

            if (expirationMonth.ToString() == Constants.Common.SelectMonth || expirationYear.ToString() == Constants.Common.SelectYear)
            {
                errors.Add("Expiration Date should not be less than today's date.");
                return false;
            }
            expirationDate = new DateTime(int.Parse(expirationYear.ToString()), int.Parse(expirationMonth.ToString()),
                                   DateTime.DaysInMonth(int.Parse(expirationYear.ToString()), int.Parse(expirationMonth.ToString())));
            if (expirationDate < DateTime.Now.Date)
            {
                errors.Add("Please enter a valid expiration date.");
                return false;
            }

            //Validate Expiration Month/Years
            int expMonth = expirationMonth.ToInt();
            int expYear = expirationYear.ToInt();

            // Validate Card Expiration
            if (!expMonth.IsValidExpirationMonth(expYear))
            {
                // Add Error to ModelState
                errors.Add("Please enter a valid expiration date.");
                return false;
            }


            return true;
        }

    }
}