using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TRAILSWEB.Models
{
	public class ReviewOrderEPASS
	{
		public OrderSummary OrderSummaryModel { get; set; }
		public AccountLogin AccountLoginModel { get; set; }
		public ContactInfo ContactInfoModel { get; set; }
		public PaymentInfo PaymentInfoModel { get; set; }
		public List<Cart> ShoppingCart { get; set; }
	}
	public class OrderSummary
	{
		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Item")]
		public string Item { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Price")]
		public Decimal Price { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Tax")]
		public decimal Tax { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "AddToBalance")]
		public decimal AddToBalance { get; set; }
	}
	public class AccountLogin
	{
		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "UserName")]
		public string UserName { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
	public class ContactInfo
	{
		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Firstname")]
		public string Firstname { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Lastname")]
		public string Lastname { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Address2")]
		public string Address2 { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "City")]
		public string City { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "State")]
		public string State { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Zip")]
		public string Zip { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "PhoneNumber")]
		public string PhoneNumber { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "License")]
		public string License { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "LicenseState")]
		public string LicenseState { get; set; }
	}
	public class PaymentInfo
	{
		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "CreditCardType")]
		public string CreditCardType { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "CardEndingIn")]
		public int CardEndingIn { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "Expires")]
		public DateTime Expires { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "AllowAutomaticBilling")]
		public bool AllowAutomaticBilling { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "LowBalanceAmount")]
		public decimal LowBalanceAmount { get; set; }

		[StringLength(13, ErrorMessage = "The {0} must be at least {2} digits.", MinimumLength = 7)]
		[Display(Name = "ReplenishAmount")]
		public decimal ReplenishAmount { get; set; }		
	}
}