using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Security.Principal;
using log4net;
using TRAILSWEB.Models;
using TRAILSWEB.Security.Claims;
//using TRAILSWEB.Entitys;

namespace TRAILSWEB.Security
{
    public class AuthManager
    {
        public static bool Login(LoginModel model, out Guid sessionId, out string requestNumber)
        {
            bool validLogin = false;
            string returnMessage = string.Empty;
            var request = System.Web.HttpContext.Current.Request;
            string clientIPAddress = string.Empty;
            string clientBrowserType = request.Browser.Type;
            string clientOperatingSystem = request.Browser.Platform;

            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                clientIPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (request.UserHostAddress.Length != 0)
            {
                clientIPAddress = request.UserHostAddress;
            }

            Guid sessionIdReturned = Guid.NewGuid();
            string requestNumberReturned = string.Empty;

            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {
                // Convert Password to MD5 Hash before processing
                string hashedPassword = (string.IsNullOrEmpty(model.Password)) ? null : model.Password.ToMD5Hash();

                // NOTE: Client front-end requires full 13 digit Transponder for validation but the back-end only works with the first 7 digits
                string transponderNumber = (string.IsNullOrEmpty(model.TransponderNumber)) ? null : model.TransponderNumber.Substring(0, 8);

                // Attempt to Authenticate User
                // NOTE: After further clarification and some Service and/or Database Changes, the cases for "Login" will change some more
                if (model.EntryPoint == ActivityTypes.GetEPASS)
                {
                    returnMessage = serviceManager.LoginReplenishAndGetTransponder(
                            clientIPAddress,
                            clientBrowserType,
                            clientOperatingSystem,
                            model.UserName,
                            hashedPassword,
                            model.AccountNumber,
                            model.PinNumber,
                            out sessionIdReturned
                        );
                }
                else
                {
                    returnMessage = serviceManager.LoginActivate(
                                        transponderNumber,
                                        clientIPAddress,
                                        clientBrowserType,
                                        clientOperatingSystem,
                                        model.UserName,
                                        hashedPassword,
                                        model.AccountNumber,
                                        model.PinNumber,
                                        out sessionIdReturned,
                                        out requestNumberReturned
                                    );
                }

                // Expected Error upon failure: There is a db problem Logging into the account

                // Determine if Authentication was successful (if failure, a message is returned)
                validLogin = string.IsNullOrEmpty(returnMessage) ? true : false;
                //if (string.IsNullOrEmpty(returnMessage))
                //{
                //    validLogin = true;
                //}
                //else
                //{
                //    if (returnMessage == " See System Administrator")
                //    {
                //        requestNumber = "The action is not permitted.Please call the Service Center for assistance";
                //        validLogin = false;
                //        sessionId = sessionIdReturned;
                //        return validLogin;
                //    }
                //    validLogin = false;
                    
                //}

                hashedPassword = string.Empty;
            }
            
                // Initialize Output Parameters
                sessionId = sessionIdReturned;
                requestNumber = requestNumberReturned;
           
            
            return validLogin;
        }

        public static TRAILSWEB.TRAILSWEBServiceReference.LoginResponse LoginWithPin(LoginModel model)
        {
            var request = System.Web.HttpContext.Current.Request;
            string clientIPAddress = string.Empty;
            string clientBrowserType = request.Browser.Type;
            string clientOperatingSystem = request.Browser.Platform;

            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                clientIPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (request.UserHostAddress.Length != 0)
            {
                clientIPAddress = request.UserHostAddress;
            }

            Guid sessionIdReturned = Guid.NewGuid();
            string requestNumberReturned = string.Empty;
            TRAILSWEB.TRAILSWEBServiceReference.LoginResponse loginResponse = new TRAILSWEB.TRAILSWEBServiceReference.LoginResponse() { IsValid = false };

            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {
                // Convert Password to MD5 Hash before processing
                string hashedPin = (string.IsNullOrEmpty(model.PinNumber)) ? null : model.PinNumber.ToMD5Hash();

                // NOTE: Client front-end requires full 13 digit Transponder for validation but the back-end only works with the first 7 digits
                string transponderNumber = (string.IsNullOrEmpty(model.TransponderNumber)) ? null : model.TransponderNumber.Substring(0, 8);

                TRAILSWEB.TRAILSWEBServiceReference.LoginResponse servieResponse = serviceManager.LoginWithPin( 
                                new TRAILSWEBServiceReference.LoginInformation {  AccountNumber = model.AccountNumber, Pin = hashedPin },
                                new TRAILSWEBServiceReference.RequesterInformation {  ClientIP = clientIPAddress , BrowserType = clientBrowserType, OperatingSystem = clientOperatingSystem } 
                                );

                loginResponse.IsValid = servieResponse.IsValid;
                loginResponse.SessionID = servieResponse.SessionID;
                loginResponse.IsUserNameAvailable = servieResponse.IsUserNameAvailable;
                loginResponse.IsAccountSecurityChangeRequired = servieResponse.IsAccountSecurityChangeRequired;
                loginResponse.CustomerName = servieResponse.CustomerName;
                loginResponse.Error = servieResponse.Error == null ? null: servieResponse.Error;

                // Expected Error upon failure: There is a db problem Logging into the account
            }


            return loginResponse;
        }

        public static TRAILSWEBServiceReference.Customers CreateUserAndGetCustomer(LoginAndRegister loginRegister )
        {

            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {
                int securityQuesionId = 0;
                TRAILSWEBServiceReference.SessionInformation saveSession =  serviceManager.SaveSecurityInformation(Guid.Parse(loginRegister.RegisterModel.SessionId),
                        new TRAILSWEBServiceReference.SecurityInformation()
                        { AccountNumber = loginRegister.RegisterModel.AccountNumber, Email = loginRegister.RegisterModel.Email, Password = loginRegister.RegisterModel.Password.ToMD5Hash()
                            , Pin = loginRegister.RegisterModel.PinNumber, UserName = loginRegister.RegisterModel.UserName, SecretQuestionId = int.TryParse( loginRegister.RegisterModel.SecurityQuestion, out securityQuesionId) ? securityQuesionId : 0
                            , SecretAnswer = loginRegister.RegisterModel.SecurityAnswer
                        });

                return serviceManager.GetCustomerInformation(Guid.Parse(saveSession.SessionID), saveSession.RequestNumber);
            }


        }
        internal static bool Login(string transponderNumber, out Guid sessionId, out string requestNumber)
        {
            LoginModel model = new LoginModel { TransponderNumber = transponderNumber };

            return Login(model, out sessionId, out requestNumber);
        }

        internal static bool Login(string transponderType, string entryPoint, out Guid sessionId, out string requestNumber)
        {
            LoginModel model = new LoginModel { EntryPoint = entryPoint };

            return Login(model, out sessionId, out requestNumber);
        }
    }
}