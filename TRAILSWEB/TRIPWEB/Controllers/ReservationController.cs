using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.WebPages;
using Antlr.Runtime;
using TRAILSWEB.TRAILSWEBServiceReference;
using TRIPWEB.Models;
using TRIPWEB.Validators;
using UIModels = TRIPWEB.Models;
using ServiceModels = TRAILSWEB.TRAILSWEBServiceReference;

namespace TRIPWEB.Controllers
{
    public class ReservationController : BaseController
    {

        // GET: Reservation
        public ActionResult Edit(string id)
        {
            Reservation existingReservation = this.GetExistingReservation(id);

            if (existingReservation.ErrorMessage != null)
            {
                ModelState.AddModelError("Error", "Having Problem opening the site try again by refreshing the page.");
                return View("Index",new Reservation());
            }
            return View("Index", existingReservation); //, ();
        }

        private Reservation GetExistingReservation(string tripUUID)
        {

            Reservation existingReservation = new Reservation();
            try
            {
                using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    // Retrieve Required Service Parameters from Authenticated User Session
                    string returnMessage = null;
                    RequiredParameters requiredParams = base.GetRequiredParameters();
                    TRAILSWEB.TRAILSWEBServiceReference.RequesterInformation requester =new TRAILSWEB.TRAILSWEBServiceReference.RequesterInformation(){ClientIP = requiredParams.Client.IPAddress,BrowserType = requiredParams.Client.BrowserType,OperatingSystem = requiredParams.Client.Platform};

                    ServiceModels.TripInformation tripInformation = serviceManager.LoginAndRetrieveTrip(requester,tripUUID);

                    Session["sessionId"] = tripInformation.SessionInformation.SessionID;

                    existingReservation =this.GetBaseReservationModel(tripInformation.ContactInformation.AddressStateSelected);
                    if(existingReservation.ErrorMessage != null)
                    {
                        throw new Exception(existingReservation.ErrorMessage) ;
                    }

                    existingReservation.ReservationSource = tripInformation.ReservationSource;

                    existingReservation.TripUUId = tripInformation.TripUUID;
                    Session["UUId"] = tripInformation.TripUUID;
                    existingReservation.TripId = tripInformation.TripID;
                    Session["TripId"] = tripInformation.TripID;
                    existingReservation.TripStage =(UIModels.TripStage) Enum.Parse(typeof(UIModels.TripStage), tripInformation.Stage.ToString());

                    existingReservation.IsBusinessAccountType = tripInformation.IsBusinessAccount ? true : false;

                    existingReservation.ContactInfo.FirstName = tripInformation.ContactInformation.FirstName;
                    existingReservation.ContactInfo.LastName = tripInformation.ContactInformation.LastName;
                    existingReservation.ContactInfo.AddressLine1 = tripInformation.ContactInformation.AddressLine1;
                    existingReservation.ContactInfo.AddressLine2 = tripInformation.ContactInformation.AddressLine2;
                    existingReservation.ContactInfo.City = tripInformation.ContactInformation.City;
                    existingReservation.ContactInfo.EmailAddress = tripInformation.ContactInformation.EmailAddress;
                    existingReservation.ContactInfo.OldEmailAddress = tripInformation.ContactInformation.EmailAddress;
                    existingReservation.ContactInfo.AddressCountrySelected =tripInformation.ContactInformation.CountryCode;
                    existingReservation.ContactInfo.AddressStateSelected =tripInformation.ContactInformation.AddressStateSelected;
                    existingReservation.ContactInfo.ZipCode = tripInformation.ContactInformation.ZipCode;
                    existingReservation.ContactInfo.PhoneNumber = tripInformation.ContactInformation.PhoneNumber;

                    existingReservation.AccountNumber = tripInformation.AccountNumber != 0? tripInformation.AccountNumber.ToString(): string.Empty;
                    Session["accountNumber"] = existingReservation.AccountNumber;

                    existingReservation.RentalAgencySelected = tripInformation.RentalAgency;
                    existingReservation.RentalAgencySelectedText = tripInformation.RentalAgency;
                    existingReservation.ArrivalDate = tripInformation.ArrivalDate.Date.ToShortDateString();
                    existingReservation.DepartureDate = tripInformation.DepartureDate.Date.ToShortDateString();
                    existingReservation.ArrivalTimeSelected =(tripInformation.ArrivalDate.Hour * 60 +Math.Round(((decimal) tripInformation.ArrivalDate.Minute / BaseController.TimeInterval)) *BaseController.TimeInterval).ToString();
                    existingReservation.DepartureTimeSelected =(tripInformation.DepartureDate.Hour * 60 +Math.Round(((decimal) tripInformation.DepartureDate.Minute / BaseController.TimeInterval)) *BaseController.TimeInterval).ToString();

                    existingReservation.TripCurrentStatus =
                        (UIModels.TripStatus) Enum.Parse(typeof(UIModels.TripStatus), tripInformation.Status.ToString());

                    //If current status is not reserved then pool licensePlate and State and transaction informations.
                    if (existingReservation.TripCurrentStatus != UIModels.TripStatus.Reserved)
                    {
                        existingReservation.VehicleInfo.LicensePlate = tripInformation.VehicleInformation.LicensePlate;
                        existingReservation.VehicleInfo.LicenseStateSelected = tripInformation.VehicleInformation.LicenseState;

                        TransactionSearchFilter filter = new TransactionSearchFilter();
                        filter.SessionID = tripInformation.SessionInformation.SessionID;
                        filter.AccountNumber = tripInformation.AccountNumber;
                        filter.StartDate = tripInformation.ArrivalDate;
                        filter.EndDate = tripInformation.DepartureDate;
                        filter.TransactionType = TransactionType.TollAndParking;

                        //Get toll and parking Transaction
                        List<TollTransactionHistory> tollTransactionsHistory = serviceManager.GetTripTransactions(filter);
                        existingReservation.TollTransactions = tollTransactionsHistory.Select(h =>
                              new UIModels.TransactionHistory()
                              {
                                  Amount = h.Amount,
                                  Lane = h.Lane,
                                  Location = h.Location,
                                  PostingDate = h.PostingDate,
                                  TransactionDate = h.TransactionDate,
                                  TransactionType = h.TransactionType,
                                  TransponderNumber = h.TransponderNumber,
                                  Make = h.Make,
                                  Model = h.Model,
                                  Year = h.Year,
                                  LicensePlate = h.LicensePlate,
                                  LicensePlateState = h.LicensePlateState
                              }
                            ).ToList();

                        //Get financial Transaction
                        filter.TransactionType = TransactionType.Financial;
                        List<TollTransactionHistory> financialTransactionsHistory = serviceManager.GetTripTransactions(filter);
                        existingReservation.FinancialTransactions = financialTransactionsHistory.Select(h =>
                              new UIModels.TransactionHistory()
                              {
                                  Amount = h.Amount,
                                  Lane = h.Lane,
                                  Location = h.Location,
                                  PostingDate = h.PostingDate,
                                  TransactionDate = h.TransactionDate,
                                  TransactionType = h.TransactionType,
                                  TransponderNumber = h.TransponderNumber,
                                  Make = h.Make,
                                  Model = h.Model,
                                  Year = h.Year,
                                  LicensePlate = h.LicensePlate,
                                  LicensePlateState = h.LicensePlateState
                              }
                            ).ToList();
                    }

                    existingReservation.IsCCDeclined = tripInformation.IsCCDeclined;

                    existingReservation.PaymentInfo.NameOnCard = tripInformation.PaymentInformation.NameOnCard;
                    existingReservation.PaymentInfo.CreditCardNumber =tripInformation.PaymentInformation.CreditCardNumber;
                    existingReservation.PaymentInfo.CreditCardType = tripInformation.PaymentInformation.CreditCardType;
                    existingReservation.PaymentInfo.ExpirationMonth =tripInformation.PaymentInformation.CCExpirationDate.Month.ToString();
                    existingReservation.PaymentInfo.ExpirationYear =tripInformation.PaymentInformation.CCExpirationDate.Year.ToString();
                    existingReservation.PaymentInfo.UseCardOnFile =(!(string.IsNullOrEmpty(tripInformation.PaymentInformation.CreditCardNumber))) ? true : false;
                    if (existingReservation.PaymentInfo.UseCardOnFile)
                    {
                        existingReservation.PaymentInfo.OldNameOnCard = tripInformation.PaymentInformation.NameOnCard;
                        existingReservation.PaymentInfo.OldCreditCardNumber =tripInformation.PaymentInformation.CreditCardNumber;
                        existingReservation.PaymentInfo.OldCreditCardType =tripInformation.PaymentInformation.CreditCardType;
                        existingReservation.PaymentInfo.OldExpirationMonth =tripInformation.PaymentInformation.CCExpirationDate.Month.ToString();
                        existingReservation.PaymentInfo.OldExpirationYear =tripInformation.PaymentInformation.CCExpirationDate.Year.ToString();
                    }

                    existingReservation.TermsAndConditions = true;
                }

                return existingReservation;
            }
            catch (Exception ex)
            {
                LogMessage(tripUUID, "Class Library: GetExistingReservation", "Exception", FormatString("Exception occured getting reservation info from DB: ", ex));
                existingReservation.ErrorMessage = "Error:Retrieving information from DB";
                return existingReservation;
            }

        }


        //public List<TransactionHistory> GetTransactions(string sessionID, string transponderNumber)
        //{
        //    string[] transponders = transponderNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    ManageModel model = new ManageModel() { TransactionHistory = new List<TransactionHistory>() };
        //    var serviceParams = GetRequiredParameters();

        //    using (TRAILSWEBServiceReference.TRAILSWEBIServiceClient serviceManager = new TRAILSWEBServiceReference.TRAILSWEBIServiceClient())
        //    {
        //        var filter = new TRAILSWEBServiceReference.TransactionSearchFilter();
        //        filter.TransactionType = TRAILSWEBServiceReference.TransactionType.Toll;
        //        filter.StartDate = start;
        //        filter.EndDate = end;
        //        filter.SessionID = serviceParams.SessionId.ToString();

        //        filter.Transponders = new List<string>(transponders);

        //        try
        //        {
        //            var results = serviceManager.GetTransactionHistory(filter);

        //            if (results.Count > 0)
        //            {
        //                model.TransactionHistory = results.SelectMany(h => h.TollTransactions).Select(
        //                    h =>
        //                       new TransactionHistory()
        //                       {
        //                           Amount = h.Amount,
        //                           Lane = h.Lane,
        //                           Location = h.Location,
        //                           PostingDate = h.PostingDate,
        //                           TransactionDate = h.TransactionDate,
        //                           TransactionType = h.TransactionType,
        //                           Make = h.Make,
        //                           Model = h.Model,
        //                           Year = h.Year,
        //                           LicensePlate = h.LicensePlate,
        //                           LicensePlateState = h.LicensePlateState,
        //                           TransponderNumber = h.TransponderNumber
        //                       }
        //                ).ToList();
        //            }
        //            else
        //            {
        //                var returnMessage = new ReturnMessage
        //                {
        //                    MessageType = MessageType.Warning,
        //                    Title = "Warning",
        //                    Message = "No transactions available. Please review selected range and try again."
        //                };

        //                return Json(returnMessage, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (FaultException ex)
        //        {
        //            var returnMessage = new ReturnMessage
        //            {
        //                MessageType = MessageType.Error,
        //                Title = "Error",
        //                Message = "Error retrieving transactions. Please review selected range and try again."
        //            };

        //            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}


        private UIModels.SessionInformation MakeReservation(Reservation model)
        {

            LogMessage("", "ReservationController: MakeReservation", "Log", string.Format("MakeReservation called for {0} {1} with EmailID = {2}", model.ContactInfo.FirstName, model.ContactInfo.LastName, model.ContactInfo.EmailAddress));

            UIModels.SessionInformation sessionInfo = new UIModels.SessionInformation();
            try
            {
                using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    ServiceModels.TripInformation tripInformation = new TripInformation()
                    {
                        ContactInformation = new ServiceModels.ContactInformation(),
                        PaymentInformation = new TripPaymentInformation()
                    };

                    tripInformation = ConvertUIModelToServiceModel(model);

                    ServiceModels.TripSessionInformation tripSessionInfo = serviceManager.ReserveATrip(tripInformation);

                    //Update sessionId in session, this is important in retreving other basic information.
                    sessionInfo.TripId = tripSessionInfo.TripId;
                    Session["TripId"] = tripSessionInfo.TripId;
                    sessionInfo.TripUUId = tripSessionInfo.TripUUID;
                    Session["UUId"] = tripSessionInfo.TripUUID;
                    sessionInfo.SessionID = tripSessionInfo.SessionID;
                    Session["sessionID"] = tripSessionInfo.SessionID;
                    sessionInfo.IsValid = tripSessionInfo.IsValid;

                    if (!tripSessionInfo.IsValid)
                        tripSessionInfo.Error.CopyTo(sessionInfo.Error);
                    
                    //throw new Exception("testing");

                }

                LogMessage(sessionInfo.SessionID, "ReservationController: MakeReservation Success", "Log", string.Format("MakeReservation returned for EmailID = {0} with TripID = {1}.", model.ContactInfo.EmailAddress, sessionInfo.TripId));

                return sessionInfo;
            }
            catch (Exception ex)
            {
                LogMessage(model.TripUUId, "Class Library: MakeReservation", "Exception", FormatString("Exception occured saving reservation to DB: ", ex));
                sessionInfo.Error[0] = "Error:Saving information to DB";
                return sessionInfo;
            }

        }

        private UIModels.SessionInformation UpdateReservationAndTrip(Reservation model,string action)
        {
            UIModels.SessionInformation sessionInfo = new UIModels.SessionInformation();

            LogMessage(model.SessionId, "ReservationController: UpdateReservationAndTrip Request", "Log", string.Format("UpdateReservation called for TripUUID = {0}.", model.TripUUId));

            try
            {
                using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    ServiceModels.TripInformation tripInformation = new TripInformation() { ContactInformation = new ServiceModels.ContactInformation(), PaymentInformation = new TripPaymentInformation() };
                
                     tripInformation = ConvertUIModelToServiceModel(model);

                    ServiceModels.TripSessionInformation tripSessionInfo = serviceManager.UpdateATrip(tripInformation, (ServiceModels.TripAction) Enum.Parse(typeof(ServiceModels.TripAction), action));
                    //Update sessionId in session, this is important in retreving other basic information.
                    sessionInfo.TripId = tripSessionInfo.TripId;
                    Session["TripId"] = tripSessionInfo.TripId;
                    sessionInfo.TripUUId = tripSessionInfo.TripUUID;
                    Session["UUId"] = tripSessionInfo.TripUUID;
                    sessionInfo.SessionID = tripSessionInfo.SessionID;
                    Session["sessionID"] = tripSessionInfo.SessionID;
                    sessionInfo.IsValid = tripSessionInfo.IsValid;

                    if (!tripSessionInfo.IsValid)
                        tripSessionInfo.Error.CopyTo(sessionInfo.Error);

                }

                LogMessage(model.SessionId, "ReservationController: UpdateReservationAndTrip Success", "Log", string.Format("MakeReservation returned for EmailID = {0} with TripID = {1}.", model.ContactInfo.EmailAddress, sessionInfo.TripId));

                return sessionInfo;
            }
            catch (Exception ex)
            {
                LogMessage(model.TripUUId, "Class Library: UpdateReservationAndTrip", "Exception", FormatString("Exception occured updating reservation or trip to DB: ", ex));
                sessionInfo.Error[0] = "Error:Updating information to DB.";
                throw;
            }

        }

        public ServiceModels.TripInformation ConvertUIModelToServiceModel(Reservation model)
        {
            int number;
            ServiceModels.TripInformation tripInformation = new TripInformation() { ContactInformation = new ServiceModels.ContactInformation(), PaymentInformation = new ServiceModels.TripPaymentInformation() ,VehicleInformation = new ServiceModels.VehicleLicenseInfo()};
            try
            {
                if (model.Action == "End")
                {
                    tripInformation.Status = (ServiceModels.TripStatus)Enum.Parse(typeof(ServiceModels.TripStatus), model.TripCurrentStatus.ToString());
                    tripInformation.SessionID = Session["sessionId"] != null ? Session["sessionId"].ToString() : null;
                    tripInformation.TripID = Session["TripId"] != null ? Session["TripId"].ToString() : null;//model.TripId;
                    tripInformation.TripUUID = Session["UUId"] != null ? Session["UUId"].ToString() : null;//model.TripUUId;
                    tripInformation.AccountNumber = Session["accountNumber"] != null ? (int.TryParse(Session["accountNumber"].ToString(), out number) ? number : 0) : 0;
                    return tripInformation;
                }
                tripInformation.Status =(ServiceModels.TripStatus)Enum.Parse(typeof(ServiceModels.TripStatus), model.TripCurrentStatus.ToString());
                tripInformation.SessionID = Session["sessionId"] != null ? Session["sessionId"].ToString() : null;
                tripInformation.TripID = Session["TripId"] != null ? Session["TripId"].ToString() : null;//model.TripId;
                tripInformation.TripUUID = Session["UUId"] != null ? Session["UUId"].ToString() : null;//model.TripUUId;
                tripInformation.AccountNumber = Session["accountNumber"] != null? (int.TryParse(Session["accountNumber"].ToString(), out number) ? number : 0): 0;


                if ((model.TripCurrentStatus == UIModels.TripStatus.Other) ||
                    (model.TripCurrentStatus == UIModels.TripStatus.Reserved))
                {
                    tripInformation.ReservationSource = model.ReservationSource;
                    tripInformation.IsBusinessAccount = model.IsBusinessAccountType ? true : false;
                    tripInformation.ContactInformation.FirstName = model.ContactInfo.FirstName;
                    tripInformation.ContactInformation.LastName = model.ContactInfo.LastName;
                    tripInformation.ContactInformation.PhoneNumber = model.ContactInfo.PhoneNumber;
                    tripInformation.ContactInformation.AddressLine1 = model.ContactInfo.AddressLine1;
                    tripInformation.ContactInformation.AddressLine2 = model.ContactInfo.AddressLine2;
                    tripInformation.ContactInformation.City = model.ContactInfo.City;
                    tripInformation.ContactInformation.CountryCode = model.ContactInfo.AddressCountrySelected;
                    tripInformation.ContactInformation.AddressStateSelected = model.ContactInfo.AddressStateSelected;
                    tripInformation.ContactInformation.ZipCode = model.ContactInfo.ZipCode;
                    tripInformation.ArrivalDate =
                        model.ArrivalDate.AsDateTime().AddMinutes(int.Parse(model.ArrivalTimeSelected));

                }

                tripInformation.ContactInformation.EmailAddress = model.ContactInfo.EmailAddress;
                tripInformation.DirtyEdit = model.EmailUpdateFlag;
                tripInformation.DirtyEdit = false;
                if (model.TripCurrentStatus != UIModels.TripStatus.Other)
                {
                    if (!(model.ContactInfo.EmailAddress.ToUpper().Equals(model.ContactInfo.OldEmailAddress.ToUpper())))
                    {
                        model.EmailUpdateFlag = true;
                        tripInformation.DirtyEdit = model.EmailUpdateFlag;
                    }
                }
                tripInformation.ContactInformation.PhoneNumber = model.ContactInfo.PhoneNumber;
                //if ((model.TripCurrentStatus == UIModels.TripStatus.Other) ||(model.TripCurrentStatus == UIModels.TripStatus.Reserved))
                //{
                //    tripInformation.ContactInformation.PhoneNumber = model.ContactInfo.PhoneNumber;
                //    tripInformation.ContactInformation.AddressLine1 = model.ContactInfo.AddressLine1;
                //    tripInformation.ContactInformation.AddressLine2 = model.ContactInfo.AddressLine2;
                //    tripInformation.ContactInformation.City = model.ContactInfo.City;
                //    tripInformation.ContactInformation.CountryCode = model.ContactInfo.AddressCountrySelected;
                //    tripInformation.ContactInformation.AddressStateSelected = model.ContactInfo.AddressStateSelected;
                //    tripInformation.ContactInformation.ZipCode = model.ContactInfo.ZipCode;
                //}

                    model.PaymentInfo.CreditCardExpirationDate =
                        new DateTime(int.Parse(model.PaymentInfo.ExpirationYear.ToString()),
                            int.Parse(model.PaymentInfo.ExpirationMonth.ToString()),
                            DateTime.DaysInMonth(int.Parse(model.PaymentInfo.ExpirationYear.ToString()),
                                int.Parse(model.PaymentInfo.ExpirationMonth.ToString())));
                    tripInformation.PaymentInformation.NameOnCard = model.PaymentInfo.NameOnCard;
                    tripInformation.PaymentInformation.CreditCardNumber = model.PaymentInfo.CreditCardNumber;
                    tripInformation.PaymentInformation.CreditCardType = model.PaymentInfo.CreditCardType;
                    tripInformation.PaymentInformation.CCExpirationDate = model.PaymentInfo.CreditCardExpirationDate;
                    tripInformation.PaymentInformation.UseCardOnFile = model.PaymentInfo.UseCardOnFile;
                


                if (model.TripCurrentStatus == UIModels.TripStatus.Started)
                {
                    tripInformation.VehicleInformation.LicensePlate = model.VehicleInfo.LicensePlate;
                    tripInformation.VehicleInformation.LicenseState = model.VehicleInfo.LicenseStateSelected;
                }

                //if ((model.TripCurrentStatus == UIModels.TripStatus.Other) ||(model.TripCurrentStatus == UIModels.TripStatus.Reserved))
                //{
                //    tripInformation.ArrivalDate =model.ArrivalDate.AsDateTime().AddMinutes(int.Parse(model.ArrivalTimeSelected));
                //}
                if ((model.TripCurrentStatus == UIModels.TripStatus.Other) ||
                    (model.TripCurrentStatus == UIModels.TripStatus.Reserved) ||
                    (model.TripCurrentStatus == UIModels.TripStatus.Started))
                {
                    tripInformation.DepartureDate =
                        model.DepartureDate.AsDateTime().AddMinutes(int.Parse(model.DepartureTimeSelected));
                }


                RequiredParameters reqParams = base.GetRequiredParameters();
                tripInformation.SessionID = reqParams.SessionId.ToString();
                if ((model.TripCurrentStatus == UIModels.TripStatus.Other) ||
                    (model.TripCurrentStatus == UIModels.TripStatus.Reserved))
                {
                    tripInformation.RentalAgency = model.RentalAgencySelectedText;
                    tripInformation.RentalAgencyCode = model.RentalAgencySelected;
                }
                return tripInformation;
            }
            catch (Exception ex)
            {
                LogMessage(model.TripUUId, "Class Library: ConvertUIModelToServiceModel", "Exception", FormatString("Exception occured converting UI model to service model: ", ex));
                var error = ex.Message;
                throw;
            }
        }

        
        private Reservation GetBaseReservationModel(string defaultState, string defaultCountry = "USA")
        {
            Reservation model = new Reservation();
            try
            {
                model.ContactInfo = new UIModels.ContactInformation();
                model.ContactInfo.AddressStateList = base.GetStateListItems(defaultState);
                if (model.ContactInfo.AddressStateList == null || (model.ContactInfo.AddressStateList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving state information from db.");
                }
                
                model.ContactInfo.AddressCountryList = base.GetCountryListItems(defaultCountry);
                if (model.ContactInfo.AddressCountryList == null || (model.ContactInfo.AddressCountryList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving country information from db.");
                }

                model.VehicleInfo = new UIModels.VehicleInformation();
                
                model.VehicleInfo.LicenseStateList = base.GetStateListItems(defaultState);
                if (model.VehicleInfo.LicenseStateList == null || (model.VehicleInfo.LicenseStateList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving state information from db.");
                }

                model.ArrivalTimeList = base.GetTimeIntervals(BaseController.DefaultArrivalTime);
                model.DepartureTimeList = base.GetTimeIntervals(BaseController.DefaultDepartureTime);

                
                model.RentalAgencyList = base.GetRentalAgencyListItems();
                if (model.RentalAgencyList == null || (model.RentalAgencyList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving rental agency from db.");
                }

                model.PaymentInfo = new UIModels.PaymentInformation();
                model.PaymentInfo.ExpirationMonthList = base.GetExpirationMonthListItems();
                model.ArrivalDate = DateTime.Today.ToShortDateString();
                model.DepartureDate = DateTime.Today.ToShortDateString();


                return model;
            }
            
            catch (Exception ex)
            {
                LogMessage(Session["UUId"], "Class Library: GetBaseReservationModel", "Exception", FormatString("Exception occured getting base reservation model from db: ", ex));
                model.ErrorMessage = "Error:retrieving information from DB";
                throw;
                //return model;
            }
        
           
        }

        private Reservation RepopulateDropDownBoxes(Reservation model)
        {
            try
            {
                
                model.ContactInfo.AddressStateList = base.GetStateListItems(model.ContactInfo.AddressStateSelected);
                if (model.ContactInfo.AddressStateList == null || (model.ContactInfo.AddressStateList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving state information from db.");
                }
                
                model.ContactInfo.AddressCountryList = base.GetCountryListItems(model.ContactInfo.AddressCountrySelected);
                if (model.ContactInfo.AddressCountryList == null || (model.ContactInfo.AddressCountryList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving country information from db.");
                }
                if (model.VehicleInfo == null)
                {
                    model.VehicleInfo = new UIModels.VehicleInformation();
                }
                
                var defaultlicensestate = string.IsNullOrEmpty(model.VehicleInfo.LicenseStateSelected)? "FL": model.VehicleInfo.LicenseStateSelected;
                model.VehicleInfo.LicenseStateList = base.GetStateListItems(model.VehicleInfo.LicenseStateSelected);
                if (model.VehicleInfo.LicenseStateList == null || (model.VehicleInfo.LicenseStateList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving state information from db.");
                }
                int number;
                model.ArrivalTimeList = base.GetTimeIntervals(int.TryParse(model.ArrivalTimeSelected, out number) ? number : BaseController.DefaultArrivalTime);
                model.DepartureTimeList = base.GetTimeIntervals(int.TryParse(model.DepartureTimeSelected, out number) ? number : BaseController.DefaultDepartureTime);

                
                model.RentalAgencyList = base.GetRentalAgencyListItems(model.RentalAgencySelected);
                if (model.RentalAgencyList == null || (model.RentalAgencyList.Count == 0))
                {
                    throw new NullReferenceException("Error retrieving rental agency from db.");
                }

                model.PaymentInfo.ExpirationMonthList = base.GetExpirationMonthListItems(model.PaymentInfo.ExpirationMonth);
                
                if(model.TripCurrentStatus != UIModels.TripStatus.Reserved && model.TripCurrentStatus != UIModels.TripStatus.Reserved)
                {
                    Reservation existingReservation = this.GetExistingReservation(model.TripUUId);
                    model.TollTransactions = existingReservation.TollTransactions;
                    model.FinancialTransactions = existingReservation.FinancialTransactions;
                }

                return model;
            }
            catch (Exception ex)
            {
                LogMessage(Session["UUId"], "Class Library: RepopulateDropDownBoxes", "Exception", FormatString("Exception occured repopulating dropdown boxes: ", ex));
                model.ErrorMessage = "Error:retrieving information from DB";
                throw;
            }
        }

        public UIModels.SessionInformation CallTheRightServiceFunction(Reservation model)
        {
            UIModels.SessionInformation sessionInfo = new UIModels.SessionInformation();
            try
            {
                if (model.TripCurrentStatus == UIModels.TripStatus.Other)
                {
                    sessionInfo = MakeReservation(model);
                }
                else
                {
                    sessionInfo = UpdateReservationAndTrip(model, model.Action);
                }
                return sessionInfo;
            }
            catch (Exception ex)
            {
                LogMessage(Session["UUId"], "Class Library: CallTheRightServiceFunction", "Exception", FormatString("Exception occured calling save or update reservation: ", ex));
                sessionInfo.Error[0] = "Error:Saving information to DB";
                sessionInfo.IsValid = false;
                return sessionInfo;
            }


}

       
        [AllowAnonymous]
        public ActionResult Product(string id)
        {
            TempData["Source"] = string.IsNullOrEmpty(id) ? "Direct" : id;
            return RedirectToAction("Index", "Reservation");
        }

        // GET: Reservation
        public ActionResult Index(string id)
        {
            string source = "Direct";

            source = TempData.ContainsKey("Source") && TempData["Source"] != null ? TempData["Source"].ToString() : source;

            bool response =false;
            try
            {
                response = base.EnrollmentLogin();
                if (!response)
                {
                    ModelState.AddModelError("Error",
                        "Having Problem opening the site try again by refreshing the page.");
                    return View(new Reservation());
                }

                Reservation model = this.GetBaseReservationModel(null);

                if (model.ErrorMessage != null)
                {
                    ModelState.AddModelError("Error",
                        "Having Problem opening the site try again by refreshing the page.");
                    return View(new Reservation());
                }

                model.ReservationSource = source;
                model.TripCurrentStatus = UIModels.TripStatus.Other;
                model.IsBusinessAccountType = false;
                return View(model);
            }
            catch (Exception ex)
            {
                LogMessage(Session["UUId"], "Class Library: Index [HTTPGet]", "Exception", FormatString("Exception occured in Index page: ", ex));
                ModelState.AddModelError("Error","Problem opening the site try again by refreshing the page.");
                return View(new Reservation());
            }
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(Reservation model)
        {
            bool result= true;
            List<string> errors = new List<string>();
            bool isvalid = false;
            model.ValidationError = false;
            UIModels.SessionInformation sessionInfo = new UIModels.SessionInformation();
            try
            {
                if (model.Action == "End")
                {
                    //if (model.PaymentInfo.UseCardOnFile)
                    //{
                    //    model.PaymentInfo.NameOnCard = model.PaymentInfo.OldNameOnCard;
                    //    model.PaymentInfo.CreditCardNumber = model.PaymentInfo.OldCreditCardNumber;
                    //    model.PaymentInfo.CreditCardType = model.PaymentInfo.OldCreditCardType;
                    //    model.PaymentInfo.ExpirationMonth = model.PaymentInfo.OldExpirationMonth;
                    //    model.PaymentInfo.ExpirationYear = model.PaymentInfo.OldExpirationYear;
                    //}
                    sessionInfo = UpdateReservationAndTrip(model, model.Action);
                    if (sessionInfo.IsValid)
                    {
                        //model.TripId = sessionInfo.TripId;
                        Confirmation confirmation = new Confirmation();
                        confirmation.TripId = sessionInfo.TripId;
                        confirmation.AccountNumber = string.IsNullOrEmpty(model.AccountNumber) ? 0 : int.Parse(model.AccountNumber);
                        confirmation.TripUUId = sessionInfo.TripUUId;
                        confirmation.EmailAddress = model.ContactInfo.EmailAddress;
                        confirmation.Status = model.TripCurrentStatus;
                        confirmation.Stage = model.TripStage;

                        TempData["ConfirmationModel"] = confirmation;
                        return RedirectToAction("Confirmation", "Reservation");
                    }
                    else
                    {
                        model = RepopulateDropDownBoxes(model);
                        model.ValidationError = true;
                        ModelState.AddModelError("Error", "errors in reservation");

                        return View(model);
                    }
                }
                else
                {

                    if (model.PaymentInfo.UseCardOnFile)
                    {
                        model.PaymentInfo.NameOnCard = model.PaymentInfo.OldNameOnCard;
                        model.PaymentInfo.CreditCardNumber = model.PaymentInfo.OldCreditCardNumber;
                        model.PaymentInfo.CreditCardType = model.PaymentInfo.OldCreditCardType;
                        model.PaymentInfo.ExpirationMonth = model.PaymentInfo.OldExpirationMonth;
                        model.PaymentInfo.ExpirationYear = model.PaymentInfo.OldExpirationYear;
                    }
                    //model = GetCorrectPayment(model);
                    result = ReservationValidator.ValidateNewResesrvationInfo(model, out errors);

                    if (errors == null || errors.Count <= 0)
                    {
                        sessionInfo = CallTheRightServiceFunction(model);

                        if (sessionInfo.IsValid)
                        {
                            model.TripId = sessionInfo.TripId;
                            Confirmation confirmation = new Confirmation();
                            confirmation.TripId = sessionInfo.TripId;
                            confirmation.AccountNumber = string.IsNullOrEmpty(model.AccountNumber) ? 0 : int.Parse(model.AccountNumber);
                            confirmation.TripUUId = sessionInfo.TripUUId;
                            confirmation.EmailAddress = model.ContactInfo.EmailAddress;
                            confirmation.Status = model.TripCurrentStatus;
                            confirmation.Stage = model.TripStage;

                            TempData["ConfirmationModel"] = confirmation;
                            return RedirectToAction("Confirmation", "Reservation");
                        }
                        else
                        {
                            model = RepopulateDropDownBoxes(model);
                            model.ValidationError = true;
                            ModelState.AddModelError("Error", "errors in your trip retrieval");
                            return View(model);
                        }

                    }
                    else
                    {
                        model = RepopulateDropDownBoxes(model);
                        model.ValidationError = true;
                        ModelState.AddModelError("Error", errors[0].ToString());
                        return View(model);
                        //return Json(new { success = false, dataFormat = "text", message = "Validation Errors:"+errors }, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(Session["UUId"], "Class Library: Index [HttpPost]", "Exception", FormatString("Exception occured in Index page: ", ex));
                model = RepopulateDropDownBoxes(model);
                model.ValidationError = true;
                ModelState.AddModelError("Error", "An error occurred while saving your changes, please try again. If the problem persists, please contact 1800-353-7277 or (407) 823-7277.");
                return View(model);
            }

        }
       
        public ActionResult Confirmation(Reservation model)
        {

            if(TempData["ConfirmationModel"] == null)
            {
                ModelState.AddModelError("Error Loading Confirmation", "There is an error retrieving your session or session has expired.");
                return View(new Confirmation());
            }
            Confirmation confirmation = (Confirmation)TempData["ConfirmationModel"];
            return View(confirmation);
        }


        //[HttpPost]
        [AllowAnonymous]
        public JsonResult ResendEmail(string TripIdOrAccountNo, string Status, bool Transactions)
        {
            LogMessage(TripIdOrAccountNo, "ReservationController: ResendEmail", "Log", string.Format("ResendEmail called for {0}.", TripIdOrAccountNo));

            RequiredParameters requiredParams = base.GetRequiredParameters();
            bool serviceResponse = false;
            if (!string.IsNullOrEmpty(TripIdOrAccountNo))
            {
                using (TRAILSWEBIServiceClient serviceManager = new TRAILSWEBIServiceClient())
                {
                    serviceResponse = serviceManager.ReSendEmail(requiredParams.SessionId.ToString(), int.Parse(TripIdOrAccountNo),
                                            (ServiceModels.TripStatus)Enum.Parse(typeof(ServiceModels.TripStatus), Status),string.Empty, Transactions);
                }
            }

            return Json(new { EmailResend = serviceResponse });
        }

    }
}