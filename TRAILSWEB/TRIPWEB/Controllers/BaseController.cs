using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using log4net;
using log4net.Config;
using OOCEA.Framework;
using TRIPWEB.Models;
using TRAILSWEB.TRAILSWEBServiceReference;
using UIModels = TRIPWEB.Models;
using ServiceModels = TRAILSWEB.TRAILSWEBServiceReference;

namespace TRIPWEB.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(String.Empty);
        private readonly int logLevel = int.Parse(ConfigurationManager.AppSettings["LoggingLevel"]);



        public const int TimeInterval = 15;
        public const int DefaultArrivalTime = 540;
        public const int DefaultDepartureTime = 1080;


        
        [HttpPost]
        [Authorize]
        public bool EnrollmentLogin()
        {
            bool validLogin = false;
            ServiceModels.DBSessionInformation loginInfo;
            try
            {
                RequiredParameters reqParameters = GetRequiredParameters();

                Guid sessionIdReturned = Guid.NewGuid();

                using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager =new TRAILSWEBIServiceClient())
                {
                    ServiceModels.RequesterInformation requester = new ServiceModels.RequesterInformation();

                    // Convert Password to MD5 Hash before processing
                    //string hashedPassword = (string.IsNullOrEmpty(model.Password)) ? null : model.Password.ToMD5Hash();
                    requester.BrowserType = reqParameters.Client.BrowserType;
                    requester.ClientIP = reqParameters.Client.IPAddress;
                    requester.OperatingSystem = reqParameters.Client.Platform;

                    loginInfo = serviceManager.LoginWithDatabase(requester, "TRIPWEB");
                    validLogin = loginInfo.IsValid;
                }

                if (validLogin)
                {
                    if (string.IsNullOrEmpty(loginInfo.SessionID))
                    {
                        return false;
                    }
                    else
                    {
                        Session["sessionID"] = loginInfo.SessionID;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogMessage(Session["sessionID"], "Class Library: EnrollmentLogin", "Exception", FormatString("Exception occured getting Session information from DB when login with DB: ", ex));
                return false;
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExtendSession()
        {
            Guid sessionID = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                sessionID = (System.Guid)Session["sessionID"];
            }
            var creditCardTypes = new List<CreditCardTypeCode>();
            string returnMessage;

            using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
            {
                creditCardTypes = serviceManager.RetrieveCreditCardTypeLookup(sessionID, out returnMessage).ToList();
            }
            if (returnMessage == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Thread.Sleep(2000);
                using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    creditCardTypes = serviceManager.RetrieveCreditCardTypeLookup(sessionID, out returnMessage).ToList();
                }
                if (returnMessage == null)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        protected RequiredParameters GetRequiredParameters()
        {
            Guid sessionId = Guid.Empty;
            string requestNumber = string.Empty;
            string clientIPAddress = string.Empty;
            string clientBrowserType = Request.Browser.Type;
            string clientOperatingSystem = Request.Browser.Platform;

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                clientIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (Request.UserHostAddress.Length != 0)
            {
                clientIPAddress = Request.UserHostAddress;
            }

            if (!(Session["sessionID"] == null))
            {
                sessionId = Guid.Parse(Session["sessionID"].ToString());
            }
            

            var clientData = new ClientData
            {
                IPAddress = clientIPAddress,
                BrowserType = clientBrowserType,
                Platform = clientOperatingSystem
            };

            return new RequiredParameters
            {
                SessionId = sessionId,
                RequestNumber = requestNumber,
                Client = clientData
            };
        }



        internal List<SelectListItem> GetExpirationMonthListItems(string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "Month",
                Value = "",
                Selected = defaultSelection
            });

            string monthName = string.Empty;

            // Build Month List
            for (int month = 1; month <= 12; month++)
            {
                // Get Month Name
                //monthName = DateTime.Parse(month.ToString() + "/1/2016").ToShortMonthName();

                selectListItems.Add(new SelectListItem
                {
                    Text = month.ToString("00"), // string.Format("{0} ({1})", monthName, month.ToString("D2")),
                    Value = month.ToString(),
                    Selected = month.ToString() == initialSelection
                });
            }

            return selectListItems;
        }


        internal List<SelectListItem> GetFilteredLicensePlateTypeList(string stateCode, string initialSelection = null)
        {
            // Retrieve Required Service Parameters from Authenticated User Session
            RequiredParameters serviceParams = GetRequiredParameters();

            var plateTypeList = new List<SelectListItem>();

            if (stateCode != null)
            {
                using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    var plateTypes = serviceManager.GetPlateTypeCodes(serviceParams.SessionId.ToString(), stateCode);
                    plateTypeList.AddRange(plateTypes.Select(pt => new SelectListItem() { Value = pt.Key, Text = pt.Value }));
                }

                if (plateTypeList.IsNullOrEmpty())
                {
                    // Add Default Selection
                    plateTypeList.Add(new SelectListItem
                    {
                        Text = "* No Plate Types Returned Verify Your Selection *",
                        Value = "",
                        Selected = true
                    });
                }
            }
            else
            {
                // Add Default Selection
                plateTypeList.Add(new SelectListItem
                {
                    Text = "-- Make a Selection --",
                    Value = "",
                    Selected = true
                });
            }

            return plateTypeList;
        }


        internal List<SelectListItem> GetStateListItems(List<State> stateList, string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "-- Make a Selection --",
                Value = "",
                Selected = defaultSelection
            });

            // Generate SelectListItems from given List
            var items = stateList.Select(s => new SelectListItem
            {
                Text = s.Value.ToTitleCase().Replace("Us ", "US "),
                Value = s.ID,
                Selected = s.ID == initialSelection
            }).ToList();

            // Add Given Data To List
            selectListItems.AddRange(items);

            return selectListItems;
        }

        internal List<SelectListItem> GetStateListItems(string initialSelection = null)
        {
            // Repopulate Lists
            var stateSelectListItems = new List<SelectListItem>();

            bool validLogin = true;
            Guid sessionId = Guid.Empty;

            UIModels.RequiredParameters reqParams = this.GetRequiredParameters();

           
            sessionId = reqParams.SessionId;
            using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
            {
               List<State> stateList = serviceManager.GetStateCodes(sessionId.ToString()).ToList();

               stateSelectListItems = GetStateListItems(stateList, initialSelection);

            }

            return stateSelectListItems;
        }


        internal List<SelectListItem> GetCountryListItems(string initialSelection = null)
        {
            // Repopulate Lists
            var countrySelectListItems = new List<SelectListItem>();

            Guid sessionId = Guid.Empty;

            RequiredParameters reqParams = this.GetRequiredParameters();

            sessionId = reqParams.SessionId;
            using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
            {
                
                List<ServiceModels.Country> countryList = serviceManager.GetCountryCodes(sessionId.ToString()).ToList();

                countrySelectListItems = GetCountryListItems(countryList, initialSelection);

            }
            return countrySelectListItems;
        }



        internal List<SelectListItem> GetCountryListItems(List<ServiceModels.Country> countryList, string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "-- Make a Selection --",
                Value = "",
                Selected = defaultSelection
            });

            // Generate SelectListItems from given List
            var items = countryList.Select(s => new SelectListItem
            {
                Text = s.ID, //s.Value.ToTitleCase().Replace("Us ", "US "),
                Value = s.Value,
                Selected = s.Value == initialSelection
            }).ToList();

            // Add Given Data To List
            selectListItems.AddRange(items);

            return selectListItems;
        }

        internal List<SelectListItem> GetRentalAgencyListItems(string initialSelection = null)
        {
            // Repopulate Lists
            var rentalSelectListItems = new List<SelectListItem>();

            Guid sessionId = Guid.Empty;

            RequiredParameters reqParams = this.GetRequiredParameters();

            sessionId = reqParams.SessionId;
            using (TRAILSWEB.TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
            {

                List<KeyValuePair<string, string>> rentalAgencyList = serviceManager.GetRentalAgencies(sessionId.ToString()).ToList();

                rentalSelectListItems = GetRentalAgencyListItems(rentalAgencyList, initialSelection);

            }
           
            return rentalSelectListItems;
        }



        internal List<SelectListItem> GetRentalAgencyListItems(List<KeyValuePair<string, string>> rentalAgencyList, string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "-- Make a Selection --",
                Value = "",
                Selected = defaultSelection
            });

            // Generate SelectListItems from given List
            var items = rentalAgencyList.Select(s => new SelectListItem
            {
                Text = s.Value, //s.Value.ToTitleCase().Replace("Us ", "US "),
                Value = s.Key,
                Selected = s.Key == initialSelection
            }).ToList();

            // Add Given Data To List
            selectListItems.AddRange(items);

            return selectListItems;
        }


        //Credit Card stuff
        internal List<SelectListItem> GetCreditCardTypeListItems(List<CreditCardTypeCode> cardTypes, string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "-- Make a Selection --",
                Value = "",
                Selected = defaultSelection
            });

            // Generate SelectListItems from given List
            var items = cardTypes.Select(c => new SelectListItem
            {
                Text = c.CreditCardName.ToTitleCase(),
                Value = c.CreditCardType,
                Selected = c.CreditCardType == initialSelection
            }).ToList();

            // Add Given Data To List
            selectListItems.AddRange(items);

            return selectListItems;
        }

        //internal List<SelectListItem> GetExpirationMonthListItems(string initialSelection = null)
        //{
        //    var selectListItems = new List<SelectListItem>();

        //    bool defaultSelection = string.IsNullOrEmpty(initialSelection);

        //    // Add Default Selection
        //    selectListItems.Add(new SelectListItem
        //    {
        //        Text = "Month",
        //        Value = "",
        //        Selected = defaultSelection
        //    });

        //    string monthName = string.Empty;

        //    // Build Month List
        //    for (int month = 1; month <= 12; month++)
        //    {
        //        // Get Month Name
        //        //monthName = DateTime.Parse(month.ToString() + "/1/2016").ToShortMonthName();

        //        selectListItems.Add(new SelectListItem
        //        {
        //            Text = month.ToString("00"), // string.Format("{0} ({1})", monthName, month.ToString("D2")),
        //            Value = month.ToString(),
        //            Selected = month.ToString() == initialSelection
        //        });
        //    }

        //    return selectListItems;
        //}

        internal List<SelectListItem> GetExpirationYearListItems(string initialSelection = null)
        {
            var selectListItems = new List<SelectListItem>();

            bool defaultSelection = string.IsNullOrEmpty(initialSelection);

            // Add Default Selection
            selectListItems.Add(new SelectListItem
            {
                Text = "Year",
                Value = "",
                Selected = defaultSelection
            });

            // Build a 15 Year List from Today
            var yearList = new SelectList(Enumerable.Range(DateTime.Now.Year, 15));

            // Set Year Item Value to Item Text
            foreach (var item in yearList)
            {
                item.Value = item.Text;
                item.Selected = item.Value == initialSelection;
            }

            selectListItems.AddRange(yearList);

            return selectListItems;
        }



        protected List<SelectListItem> GetTimeIntervals(int defaultTime = 0)
        {

            int valueForInternvalTime = DefaultArrivalTime;
            List<SelectListItem> timeList = new List<SelectListItem>();

            DateTime today = DateTime.Today;
            TimeSpan tsToTime = new TimeSpan(24, 00, 00);
            today = today + tsToTime;

            DateTime date = DateTime.Today;
            TimeSpan tsFromTime = new TimeSpan(9, 00, 00);
            date = date + tsFromTime;

            while (date <= today)
            {
                timeList.Add(new SelectListItem() { Text = date.ToString("hh:mm tt"), Value = valueForInternvalTime.ToString(), Selected = valueForInternvalTime == defaultTime? true: false });
                date = date.AddMinutes(TimeInterval);
                valueForInternvalTime += TimeInterval;
            }
            return timeList;
        }

        protected void LogMessage(object sessionID, object functionName, object errorType, object error, int level = 0)
        {

            switch (level)
            {
                case 0:
                    Log.Error(string.Format("{0} \t {1} \t {2} \t {3}", sessionID, functionName, errorType, error));
                    break;
                case 1:
                    Log.Info(string.Format("{0} \t {1} \t {2} \t {3}", sessionID, functionName, errorType, error));
                    break;
                case 2:
                    Log.Info(string.Format("{0} \t {1} \t {2} \t {3}", sessionID, functionName, errorType, error));
                    break;
                default:
                    Log.Error(string.Format("{0} \t {1} \t {2} \t {3}", sessionID, functionName, errorType, error));
                    break;
            }

        }

        protected string FormatString(object one, object two)
        {
            return string.Format("{0} {1}", one, two);
        }

        protected string FormatString(object one, object two, object three)
        {
            return string.Format("{0} {1}{2}", one, two, three);
        }

        protected string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }

    }

}