using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace TRAILSWEB.Models
{
    public class Driver
    {
        [DataMember]
        [Required]
        [StringLength(13, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string DriversLicense { get; set; }

        [DataMember]
        public List<SelectListItem> DriversLicenseState { get; set; }

        [DataMember]
        [Required]
        [StringLength(2, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string DriversLicenseStateSelected { get; set; }

        public List<SelectListItem> LicensePlateType { get; set; }

        public string LicensePlateTypeSelected { get; set; }

        public List<Vehicle> Vehicles { get; set; }

        public Driver()
        {
            Vehicles = new List<Vehicle>();
        }
    }
}