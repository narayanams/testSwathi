using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TRAILSWEB.Models
{
    public class Transponder : ITransponder
    {
        public string TransponderNumber { get; set; }
        public int TransponderType { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public string Message { get; set; }

        public string LongDescription { get; set; }
        public bool AddNew { get; set; }
        public bool Replace { get; set; }
        public bool Activate { get; set; }
        public string LicensePlateNumber { get; set; }
        public string LicensePlateState { get; set; }
    }

    public class SelectTransponder
    {
        public List<Transponder> Transponders { get; set; }
        public bool AddNew { get; set; }

        //public bool AddNew { get; set; }

        public List<SelectListItem> StateCodes { set; get; }
    }

    public class SelectTransponder_Purchase
    {
        public string TransponderType { get; set; }
        public string TransponderName { get; set; }
        public int Index { get; set; }

        //public bool AddNew { get; set; }
    }

}