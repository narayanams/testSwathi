using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TRAILSWEB.Models;
using TRAILSWEB.Security.Claims;

namespace TRAILSWEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalFilters.Filters.Add(new OfflineActionFilter());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.RegisterDataMapping();

            //this model binder will decrypt any encrypted hidden post fields
            //ModelBinders.Binders.DefaultBinder = new Helpers.EncryptedModelBinder();

            // OWASP Recommended Response Header to prevent "ClickJacking" attacks
            // MVC5 generates the "X-Frame-Options SAMEORIGIN" header by default, the following line disables the default behavior
            System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            //Remove X-AspNetMvc-Version Header
            MvcHandler.DisableMvcResponseHeader = true;
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

        /// <summary>
        /// Expect the unexpected. Log any errors that weren't caught along the way.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(Object sender, EventArgs e)
        {/*
            Exception ex = Server.GetLastError();

            // Test for AntiForgery Token Errors
            if (ex is HttpAntiForgeryException)
            {
                //_logger.Error("Anti-Forgery Token Error", ex);
                Response.Clear();
                Server.ClearError();
                Server.TransferRequest("~/Error?refId=aft");
                return;
            }

            if (ex.Message.Contains("was not found or does not implement IController"))
            {
                Response.Clear();
                Server.ClearError();
                Server.TransferRequest("~/Error?refId=404");
                return;
            }

            // Test for Session Timeout Errors
            if (Session["LoginTime"] == null)
            {
                Response.Clear();
                Server.ClearError();
                Server.TransferRequest("~/Error?refId=sess");
                return;
            }

            // Test for HTTP Exceptions
            if ((ex is HttpException) || (ex is InvalidOperationException))
            {
                Response.Clear();
                Server.ClearError();

                int httpErrorCode;

                int.TryParse(((HttpException)ex).GetHttpCode().ToString(), out httpErrorCode);

                Server.TransferRequest("~/Error?refId=" + httpErrorCode);

                return;
            }

            if (ex is ThreadAbortException)
                return;

            //_logger.Error("Unexpected Error", ex);
            Response.Clear();
            Server.ClearError();
            Server.TransferRequest("~/Error");*/
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

        protected void Session_Abandon(object sender, EventArgs e)
        {
            var owinContext = Request.GetOwinContext();
            var authenticationManager = owinContext.Authentication;

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            RequiredParameters serviceParams = null;
            try
            {
                serviceParams = GetRequiredParameters();

                Session.RemoveAll();
            }
            catch
            { }

            if (serviceParams != null)
            {
                // Verify we have a valid SessionId before attempting a Database Logout
                if (serviceParams.SessionId != Guid.Empty)
                {
                    // Log out of Database Session
                    using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
                    {
                        serviceManager.Logout(serviceParams.SessionId);
                    }
                }
            }

            // Remove Application Specific Claims
            identity.RemoveClaim(TRAILSClaimTypes.SessionId);
            identity.RemoveClaim(TRAILSClaimTypes.RequestNumber);
            identity.RemoveClaim(TRAILSClaimTypes.Activity);
            identity.RemoveClaim(TRAILSClaimTypes.EntryPointActivity);
            identity.RemoveClaim(TRAILSClaimTypes.PreviousActivity);
            identity.RemoveClaim(TRAILSClaimTypes.TransponderTypeSelected);
            identity.RemoveClaim(TRAILSClaimTypes.TransponderPrice);
            identity.RemoveClaim(TRAILSClaimTypes.AccountNumber);
            identity.RemoveClaim(TRAILSClaimTypes.FirstName);
            identity.RemoveClaim(TRAILSClaimTypes.LastName);
            identity.RemoveClaim(TRAILSClaimTypes.AddressLine1);
            identity.RemoveClaim(TRAILSClaimTypes.AddressLine2);
            identity.RemoveClaim(TRAILSClaimTypes.City);
            identity.RemoveClaim(TRAILSClaimTypes.State);
            identity.RemoveClaim(TRAILSClaimTypes.ZipCode);
            identity.RemoveClaim(TRAILSClaimTypes.EmailAddress);
            identity.RemoveClaim(TRAILSClaimTypes.CurrentBalance);
            identity.RemoveClaim(TRAILSClaimTypes.PaymentRequired);
            identity.RemoveClaim(TRAILSClaimTypes.PaymentSuccessful);
            identity.RemoveClaim(TRAILSClaimTypes.TransferError);
            identity.RemoveClaim(TRAILSClaimTypes.SalesTax);
            identity.RemoveClaim(TRAILSClaimTypes.PrepaidTollsAmount);
            identity.RemoveClaim(TRAILSClaimTypes.PaymentConfirmation);

            // Sign user out of Session
            authenticationManager.SignOut();
        }

        /// <summary>
        /// Each call to the Service requires the same Parameters and retrieving/building them is time consuming. 
        /// Use this Helper to quickly retrieve the necessary Parameters.
        /// </summary>
        private RequiredParameters GetRequiredParameters()
        {
            Guid sessionId = Guid.Empty;
            string requestNumber = string.Empty;
            string returnMessage = string.Empty;
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

            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var claims = identity.Claims;

                sessionId = identity.Claims.Where(claim => claim.Type == TRAILSClaimTypes.SessionId)
                    .Select(claim => claim.Value).SingleOrDefault().ToGuid();

                requestNumber = identity.Claims.Where(claim => claim.Type == TRAILSClaimTypes.RequestNumber)
                    .Select(claim => claim.Value).SingleOrDefault();
            }

            var clientData = new Client
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
    }

    public class OfflineActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ipAddress = HttpContext.Current.Request.UserHostAddress;


            string requestURL = filterContext.HttpContext.Request.Url.ToString();

            //If requests comes from HomePage (Like redirect from Offline page, don't check.
            //This condition will avoid cyclic calls
            //Also if error page has been requested then don't call DB to get Maintemamce message
            if (!(filterContext.HttpContext.Request.RawUrl.Equals("/") || filterContext.HttpContext.Request.RawUrl.Equals("/TRAILS/") || filterContext.HttpContext.Request.RawUrl.Contains("Error") || filterContext.HttpContext.Request.RawUrl.ToLower().Equals("/epassweb/")))
            {
                bool isSiteInMaintenance = IsInMaintenance();

                    if (isSiteInMaintenance)
                    {   

                        filterContext.Result = new ViewResult
                        {
                            ViewName = "Offline"
                        };
                        var response = filterContext.HttpContext.Response;
                        response.StatusCode = (int)System.Net.HttpStatusCode.ServiceUnavailable;
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

            // Check for Maintenance Mode Messages
            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {
                string response = serviceManager.InMaintenanceMode(out maintenanceMessage, out warningMessage, out alertMessage);

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

            bool isEnableMaintenanceSecurity = bool.Parse(ConfigurationManager.AppSettings["EnableMaintenanceSecurity"].ToString());

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
