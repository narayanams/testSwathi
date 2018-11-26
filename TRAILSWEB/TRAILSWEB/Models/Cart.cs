using System;


namespace TRAILSWEB.Models
{
    public enum CartActivityType
    {
        None = 0,
        AddNew = 1,
        Replace = 2,
        Delete = 3,
        Undo = 4
    };

    public class Cart
    {
        private string _status;

        public Transponder TransponderDetail { get; set; }

        public Vehicle VehicleInfo { get; set; }

        public bool Processed { get; set; }

        public string Status
        {
            get
            {
                if (_status == string.Empty)
                {
                    _status = VehicleInfo.Status;
                }
                return _status;
            }
            set { _status = value; }
        }

        public CartActivityType Activity { get; set; }

        public string InitialTransponderDescription { get; set; }

        public string InitialStatus { get; set; }

        public string InitialStatusCode { get; set; }
    }
}