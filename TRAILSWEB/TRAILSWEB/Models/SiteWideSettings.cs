using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace TRAILSWEB.Models
{
    public sealed class SiteWideSettings
    {
        //public bool IsReferalActive{ get; set ;}

        public decimal SalesTax { get; set; }

        //public ReferralWidgetSettings ReferralWidget { get; }

        //In private constructor get values from settings/config 
        private SiteWideSettings()
        {
            //this.IsReferalActive = (bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ReferralActive")));
            decimal salesTax;
            this.SalesTax = decimal.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("SalesTax"), out salesTax)? salesTax: 6.5M;
            /*
            if (this.IsReferalActive)
            {
                this.ReferralWidget = new ReferralWidgetSettings();
            }*/
        }

        private static readonly Lazy<SiteWideSettings> lazy = new Lazy<SiteWideSettings>(() => new SiteWideSettings());

        public static SiteWideSettings Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }

    public class ReferralWidgetSettings
    {
        public string SiteId { get; set; }
        public string WidgetId { get; set; }

        public ReferralWidgetSettings()
        {
            this.SiteId = System.Configuration.ConfigurationManager.AppSettings.Get("ReferralSiteId");
            this.WidgetId = System.Configuration.ConfigurationManager.AppSettings.Get("ReferralWidgetId");
        }
    }

    public class LoggedinUserInformation
    {
        public string LoggedinUserEmail { get; set; }
        public string LoggedinUserAccountNo { get; set; }
        public string LoggedinUserFirstName { get; set; }
        public string LoggedinUserLastName { get; set; }
    }
    public sealed class PerSessionSettings
    {
        public bool IsUserLoggedin { get; set; }
        public bool IsUserReferred { get; set; }

        public int PreferredProductSKU { get; set; }

        public LoggedinUserInformation UserInformation { get; set; }

        private static object _syncRoot = new object();

        public static PerSessionSettings Instance
        {
            get
            {
                HttpSessionState session = HttpContext.Current.Session;

                lock (_syncRoot)
                {
                    if (session["PerSessionInstance"] == null)
                    {
                        session["PerSessionInstance"] = new PerSessionSettings();
                    }
                }

                return (PerSessionSettings)session["PerSessionInstance"];
            }
        }

    }
}