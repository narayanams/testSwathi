using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using TRAILSWEB.Security.Claims;
using TRAILSWEB.TRAILSWEBServiceReference;
//using TRAILSWEB.Entitys;

namespace TRAILSWEB.Models
{
    public static class StaticData
    {

        #region Select List Helpers
        public static List<SelectListItem> GetSecurityQuestionListFromService(RequiredParameters serviceParams, string initialSelection = null)
        {
            //ToDo: Implement a Service Call here but for now, Mock the data

            List<SecretQuestions> securityQuestions = new List<SecretQuestions>();

            using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
            {
                Guid sessionIdForRegister = Guid.Empty;
                string errorResponse = "";

                errorResponse = serviceManager.LoginGetTransponder(serviceParams.Client.IPAddress, serviceParams.Client.BrowserType, serviceParams.Client.Platform, out sessionIdForRegister);


                securityQuestions = serviceManager.GetSecretQuestions(sessionIdForRegister, out errorResponse);
            }



            // Build Security Questions Select List
            return GetSecurityQuestionListItems(securityQuestions);

        }

        private static List<SelectListItem> GetSecurityQuestionListItems(List<SecretQuestions> questionList, string initialSelection = null)
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
            var items = questionList.Select(s => new SelectListItem
            {
                Text = s.Value,
                Value = s.ID,
                Selected = s.ID == initialSelection
            }).ToList();

            // Add Given Data To List
            selectListItems.AddRange(items);

            return selectListItems;
        }

        public static RequiredParameters GetStaticParameters()
        {

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var claims = identity.Claims;

            var sessionId = identity.Claims.Where(claim => claim.Type == TRAILSClaimTypes.SessionId)
                .Select(claim => claim.Value).SingleOrDefault().ToGuid();

            var clientData = new Client
            {
                IPAddress = "",
                BrowserType = "",
                Platform = ""
            };

            return new RequiredParameters
            {
                SessionId = sessionId,
                RequestNumber = "",
                Client = clientData
            };


        }
        #endregion
    }
}