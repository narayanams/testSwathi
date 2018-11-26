using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace TRAILSWEB.Models
{
    public class Vehicle
    {
        public List<SelectListItem> Make { get; set; }

        [DataMember]
        [Required]
        public string MakeSelected { get; set; }

        public List<SelectListItem> Model { get; set; }

        [DataMember]
        [Required]
        public string ModelSelected { get; set; }

        public List<SelectListItem> Color { get; set; }

        [DataMember]
        [Required]
        public string ColorSelected { get; set; }

        public List<SelectListItem> Year { get; set; }

        [DataMember]
        [Required]
        public int YearSelected { get; set; }

        [DataMember]
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string LicensePlateNumber { get; set; }

        [DataMember]
        [Required]
        public long? TransponderNumber { get; set; }


        [DataMember]
        public List<SelectListItem> LicenseState { get; set; }

        [DataMember]
        [Required]
        public string LicenseStateSelected { get; set; }

        /// <summary>
        /// Indicates the Type of Transponder (2 = Mini, 5 = Portable, etc.)
        /// </summary>
        [Description("Transponder Type (2 = Mini, 5 = Portable, etc.)")]
        [DisplayName("Transponder Type")]
        public int TransponderType { get; set; }

        /// <summary>
        /// Indicates that a Transponder Switch / Replace operation should occur.
        /// </summary>
        [Description("Boolean Flag indicating that a Transponder Switch / Replace operation should occur.")]
        [DisplayName("Replace Transponder Flag")]
        public bool ReplaceTransponder { get; set; }

        /// <summary>
        /// Indicates the existing Transponder to be Replaced
        /// </summary>
        [Description("Transponder Number to be Replaced with New Purchase")]
        [DisplayName("E-PASS Transponder to Replace")]
        public string TransponderToReplace { get; set; }

        [Description("Vehicle Transponder Status")]
        [DisplayName("Status")]
        public string Status { get; set; }

        [Description("Vehicle Transponder Status Code")]
        [DisplayName("Status Code")]
        public string StatusCode { get; set; }

        [Description("Flag showing if Vehicle Transponder can be edited")]
        [DisplayName("Edit Enabled")]
        public string EditEnabled { get; set; }
        public  string CouponCode { get; set; }
        public string PromotionCode { get; set; }
        public string LimitCheck { get; set; }
    }

    public class TransponderToPurchase
    {
        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string LicensePlateState { get; set; }

        [DataMember]
        public decimal TranponderPrice { set; get; }

        [DataMember]
        public string TransponderType { get; set; }

        [DataMember]
        public string TransponderTypeName { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string ValidationError { get; set; }

        [DataMember]
        public string TransponderNumber { get; set; }

        [DataMember]
        public string TransponderNumberToReplace { get; set; }

        [DataMember]
        public string VehicleMake { get; set; }
        [DataMember]
        public string VehicleModel { get; set; }
        [DataMember]
        public string VehicleYear { get; set; }
        [DataMember]
        public string VehicleColor { get; set; }
        [DataMember]
        public string CouponCode { get; set; }
        [DataMember]
        public string PromotionCode { get; set; }
        [DataMember]
        public string LimitCheck { get; set; }


    }
}