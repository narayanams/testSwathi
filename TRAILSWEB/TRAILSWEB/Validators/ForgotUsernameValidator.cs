using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TRAILSWEB.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForgotUsernameValidator : ValidationAttribute, IClientValidatable
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var param1 = validationContext.ObjectInstance.GetType().GetProperty("AccountNumber");
            var param2 = validationContext.ObjectInstance.GetType().GetProperty("UserName");

            string accountHasValue = param1.GetValue(validationContext.ObjectInstance, null) as string;
            string userHasValue = param2.GetValue(validationContext.ObjectInstance, null) as string;

            int accountNum;
            int.TryParse(accountHasValue, out accountNum);

            if (!String.IsNullOrWhiteSpace(accountHasValue) && accountNum == 0)
            {
                return new ValidationResult("Account Number must be numeric");
            }

            if (accountHasValue == null && String.IsNullOrWhiteSpace(userHasValue))
            {

                return new ValidationResult("No account Number or password provided");
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //Do custom client side validation hook up

            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "validparam"
            };
        }
    }
}