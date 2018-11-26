using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using TRAILSWEB.Validators;

namespace TRAILSWEB.Models
{
    public class LoginAndRegister //: LoginModel
    {
        public LoginModel LoginModel { get; set; }
        public RegisterModel RegisterModel { get; set; }

    }
    public class LoginModel
    {
        [StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
        [Display(Name = "Transponder Number")]
        public string TransponderNumber { get; set; }

        public bool AgreementConfirmation { get; set; }

        [Display(Name = "User name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.UserNameCustomerAccount, ErrorMessage = "Please enter a valid User Name")]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.PasswordCustomerAccount, ErrorMessage = "Please enter a valid Password")]
        public string Password { get; set; }

    
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "PIN Number")]
        public string PinNumber { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Display(Name = "Entry Point")]
        public string EntryPoint { get; set; }

        public bool NoEncryption { get; set; }

        [Display(Name = "Security Question")]
        public IEnumerable<SelectListItem> SecurityQuestions { get; set; }

        public string SecurityQuestionSelected { get; set; }

        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }

        [Display(Name = "Do Account Login")]
        public bool DoAccountLogin { get; set; }
    
}

    public class RegisterModel
    {
        public string TransponderNumber { get; set; }
        public bool AgreementConfirmation { get; set; }

        [Display(Name = "Account Number")]
        //[Required]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "Confirm User name")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.UserNameCustomerAccount, ErrorMessage = "Please enter a valid Confirm User Name")]
        //[System.ComponentModel.DataAnnotations.Compare("UserName", ErrorMessage = "The UserName and confirmation UserName do not match.")]
        public string ConfirmUserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [RegularExpression(OOCEA.Framework.Common.Constants.RegEx.Email, ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }

        [Required]
        [Range(1000, 9999, ErrorMessage = "The PIN Number must be only of 4 digits.")]
        [StringLength(4, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "PIN Number")]
        public string PinNumber { get; set; }

        [Display(Name = "Security Question")]
        public IEnumerable<SelectListItem> SecurityQuestions { get; set; }

        public string SecurityQuestionSelected { get; set; }
        public string SecurityQuestion { get; set; }

        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }

        
        public string SessionId { get; set; }

        public string EntryPoint { get; set; }
    }

    public class ForgotUsernamePassword
    {
        //[ForgotPasswordValidator(ErrorMessage = "Account Number or Username must be specified.")]
        public string AccountNumber { get; set; }

        public string UserName { get; set; }
        public string TransponderNumber { get; set; }
        public string LicensePlateNumber { get; set; }
        public string LicenseStateSelected { get; set; }

        [DataMember]
        public List<SelectListItem> LicenseState { get; set; }

        public string RegisteredEmail { get; set; }

        public bool IsForgotPassword { get; set; }

    }

    public class ResetPassword
    {
        public string GUID { get; set; }
        [ResetPasswordValidator(ErrorMessage = "Pin or License last five must be specified.")]
        public string Pin { get; set; }
        public string LicenseLastFive { get; set; }
        public string NewPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password do not match.")]
        public string ConfirmNewPassword { get; set; }

        public ResetPassword(string guid)
        {
            GUID = guid;
        }

        public ResetPassword()
        {

        }

    }

    public class SecurityQuestion
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }

    public class EmailRequest
    {
        public string Email { get; set; }

        public enum Option
        {
            ResetUsername,
            ResetPassword
        }

        public Option OptionFlag { get; set; }
        public List<string> AccountNumbers { get; set; }
        public string AccountNumber { get; set; }
        public string ErrorMessage { get; set; }
        public bool CheckEmailSuccess { get; set; }

        public bool PasswordLinkSent { get; set; }

        public EmailRequest()
        {
            this.AccountNumbers = new List<string>();
        }

        public bool HasMultipleAccounts { get; set; }
    }



}