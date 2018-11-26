using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TRAILSWEB.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ResetPasswordValidator : ValidationAttribute, IClientValidatable
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
        //    var param1 = validationContext.ObjectInstance.GetType().GetProperty("Pin");
        //    var param2 = validationContext.ObjectInstance.GetType().GetProperty("LicenseLastFive");

        //    string pinHasValue = param1.GetValue(validationContext.ObjectInstance, null) as string;
        //    string licenseHasValue = param2.GetValue(validationContext.ObjectInstance, null) as string;



        //    if (pinHasValue == null && String.IsNullOrWhiteSpace(licenseHasValue))
        //    {

        //        return new ValidationResult("No pin or license last five provided");
        //    }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
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