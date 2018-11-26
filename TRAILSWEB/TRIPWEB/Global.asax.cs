using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


using TRAILSWEB.TRAILSWEBServiceReference;

namespace TRIPWEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          GlobalFilters.Filters.Add(new MaintanenceMode());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Remove Server Header
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }

            // OWASP Recommended Response Header to prevent "ClickJacking" attacks
            Response.AddHeader("X-Frame-Options", "DENY");

            // OWASP Cache Control Directives
            // Do NOT allow Cacheing of any pages as a precaution for when publicly shared computers are used.
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.

            EnableCrossDomainAjaxCall();
        }

        private void EnableCrossDomainAjaxCall()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        //    public RequiredParameters GetRequiredParameters()
        //    {
        //        Guid sessionId = Guid.Empty;
        //        string requestNumber = string.Empty;
        //        string clientIPAddress = string.Empty;
        //        string clientBrowserType = Request.Browser.Type;
        //        string clientOperatingSystem = Request.Browser.Platform;

        //        if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        //        {
        //            clientIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //        }
        //        else if (Request.UserHostAddress.Length != 0)
        //        {
        //            clientIPAddress = Request.UserHostAddress;
        //        }

        //        if (User.Identity.IsAuthenticated)
        //        {
        //            //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        //            //var claims = identity.Claims;

        //            sessionId = (System.Guid) Session["sessionID"];
        //                //identity.Claims.Where(claim => claim.Type == TRAILSClaimTypes.SessionId)
        //                //.Select(claim => claim.Value).SingleOrDefault().ToGuid();

        //            requestNumber = Session["requestNumber"].ToString();
        //            //identity.Claims.Where(claim => claim.Type == TRAILSClaimTypes.RequestNumber)
        //            //.Select(claim => claim.Value).SingleOrDefault();
        //        }

        //        var clientData = new ClientData
        //        {
        //            IPAddress = clientIPAddress,
        //            BrowserType = clientBrowserType,
        //            Platform = clientOperatingSystem
        //        };

        //        return new RequiredParameters
        //        {
        //            SessionId = sessionId,
        //            RequestNumber = requestNumber,
        //            Client = clientData
        //        };
        //    }
        //}

        public class MaintanenceMode : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var ipAddress = HttpContext.Current.Request.UserHostAddress;

                string requestURL = filterContext.HttpContext.Request.Url.ToString();

                //If requests comes from HomePage (Like redirect from Offline page, don't check.
                //This condition will avoid cyclic calls
                //Also if error page has been requested then don't call DB to get Maintemamce message
                if (!(filterContext.HttpContext.Request.RawUrl.Equals("/") ||
                    filterContext.HttpContext.Request.RawUrl.Equals("/TRIPWEB/") ||
                    filterContext.HttpContext.Request.RawUrl.Contains("Error") ||
                      filterContext.HttpContext.Request.RawUrl.ToLower().Equals("/VisitorTollPass/")))
                {
                    bool isSiteInMaintenance = IsInMaintenance();

                    if (isSiteInMaintenance)
                    {

                        filterContext.Result = new ViewResult
                        {
                            ViewName = "Offline"
                        };
                        var response = filterContext.HttpContext.Response;
                        response.StatusCode = (int) System.Net.HttpStatusCode.ServiceUnavailable;
                        response.TrySkipIisCustomErrors = true;
                        return;
                    }
                }

                //otherwise we let this through as normal
                base.OnActionExecuting(filterContext);
            }

            private bool IsInMaintenance()
            {

                string maintenanceMessage = string.Empty;
                string warningMessage = string.Empty;
                string alertMessage = string.Empty;
                string applicationName = ConfigurationManager.AppSettings["ApplicationName"].ToString();

                // Check for Maintenance Mode Messages
                using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    string response = serviceManager.InMaintenanceModeByAppName(applicationName,out maintenanceMessage, out warningMessage,out alertMessage);

                    if (!string.IsNullOrEmpty(response))
                    {
                        if (AuthorizeUserInMaintenanceMode())
                            return false;
                    }

                    // Process Maintenance Message
                    if (!string.IsNullOrEmpty(maintenanceMessage)) //|| !string.IsNullOrEmpty(warningMessage) )
                    {
                        return true;
                    }
                    return false;
                }
            }

            private bool AuthorizeUserInMaintenanceMode()
            {
                string clientIPAddressString = HttpContext.Current.Request.UserHostAddress;
                IPAddress clientIP = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);

                bool isEnableMaintenanceSecurity =
                    bool.Parse(ConfigurationManager.AppSettings["EnableMaintenanceSecurity"].ToString());

                string allowedIPAddressList = ConfigurationManager.AppSettings["AllowedIPAddressList"].ToString();

                bool isInIPAddressList = allowedIPAddressList.Contains(clientIPAddressString);

                if (isInIPAddressList && isEnableMaintenanceSecurity)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
