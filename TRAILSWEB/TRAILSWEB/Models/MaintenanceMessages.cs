using System.Configuration;
using System.Net;
using System.Web;


namespace TRAILSWEB.Models
{
    public class MaintenanceMessages
    {
        public string MaintenanceMessage { get; set; }
        public string WarningMessage { get; set; }
        public string AlertMessage { get; set; }
        
        private void PopulateMaintenanceMessage()
        {
            string maintenanceMessage = string.Empty;
            string warningMessage = string.Empty;
            string alertMessage = string.Empty;
            string response = string.Empty;
            
            // Check for Maintenance Mode Messages
            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {

                response = serviceManager.InMaintenanceMode(out maintenanceMessage, out warningMessage, out alertMessage);


                if (!string.IsNullOrEmpty(response))
                {
                    string clientIPAddressString = HttpContext.Current.Request.UserHostAddress;
                    IPAddress clientIP = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);

                    bool isEnableMaintenanceSecurity = bool.Parse(ConfigurationManager.AppSettings["EnableMaintenanceSecurity"].ToString());

                    string allowedIPAddressList = ConfigurationManager.AppSettings["AllowedIPAddressList"].ToString();

                    bool isInIPAddressList = allowedIPAddressList.Contains(clientIPAddressString);

                    if (isInIPAddressList && isEnableMaintenanceSecurity)
                    {
                        response = string.Empty;
                        maintenanceMessage = string.Empty;
                        warningMessage = string.Empty;
                        alertMessage = string.Empty;
                    }
                }


                // Process Maintenance Message
                if (!string.IsNullOrEmpty(maintenanceMessage))
                {
                    this.MaintenanceMessage = maintenanceMessage;
                }

                // Process Maintenance Message
                if (!string.IsNullOrEmpty(warningMessage))
                {
                    this.WarningMessage = warningMessage;
                }

                // Process Alert Message
                if (!string.IsNullOrEmpty(alertMessage))
                {
                    this.AlertMessage = alertMessage;
                }



            }


        }

        public MaintenanceMessages()
        {
            PopulateMaintenanceMessage();
        }
    }
}