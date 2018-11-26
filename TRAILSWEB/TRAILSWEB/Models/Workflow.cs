using System.Collections.Generic;
using System.ComponentModel;

namespace TRAILSWEB.Models
{
    public class Step
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Sequence { get; set; }
    }

    public class Workflow
    {
        public string EntryPointActivity { get; set; }
        public string RedirectToAction { get; set; }
        public List<Step> Steps { get; set; }
        public string EndPointActivity { get; set; }
        public string ProcessedActivity { get; set; }
    }

    public static class ActivityTypes
    {
        /// <summary>
        /// New Account Activity.
        /// </summary>
        [Description("New Account Activity.")]
        public const string NewAccount = "NewAccount";

        /// <summary>
        /// Create User Account.
        /// </summary>
        [Description("Create User Account.")]
        public const string CreateUser = "CreateUser";

        /// <summary>
        /// Existing Account Activity.
        /// </summary>
        [Description("Existing Account Activity.")]
        public const string ExistingAccount = "ExistingAccount";

        /// <summary>
        /// Login Activity
        /// </summary>
        [Description("Login Activity")]
        public const string Login = "Login";

        /// <summary>
        /// Logout Activity
        /// </summary>
        [Description("Logout Activity")]
        public const string Logout = "Logout";

        /// <summary>
        /// Reatil Sales Activation Activity
        /// </summary>
        [Description("Retail Sales Activation Activity")]
        public const string Activate = "Activate";

        /// <summary>
        /// Indicates that the Login Activity for Activate was Successful
        /// </summary>
        [Description("Activate Login Suucess Activity")]
        public const string ActivateLoginSuccess = "ActivateLoginSuccess";


        /// <summary>
        /// Register In Lane Purchase Activity
        /// </summary>
        [Description("Register In Lane Purchase Activity")]
        public const string Register = "Register";

        /// <summary>
        /// Manage Account Activity
        /// </summary>
        [Description("Manage Account Activity")]
        public const string Manage = "Manage";

        /// <summary>
        /// Manage Account Activity
        /// </summary>
        [Description("Manage Login Success Activity")]
        public const string ManageLoginSuccess = "ManageLoginSuccess";

        /// <summary>
        /// Initiate a Non-User Session Activity
        /// </summary>
        [Description("Initiate non-user Session Activity")]
        public const string InitiateSession = "InitiateSession";

        /// <summary>
        /// E-PASS Purchase and Replace Activity
        /// </summary>
        [Description("Get E-PASS Activity")]
        public const string GetEPASS = "GetEPASS";

        /// <summary>
        /// Indicates that the Login Activity for Get E-PASS was Successful
        /// </summary>
        [Description("Get E-PASS Login Success Activity")]
        public const string GetEPASSLoginSuccess = "GetEPASSLoginSuccess";


        /// <summary>
        /// Indicates that the Login Activity for Get E-PASS was Successful
        /// </summary>
        [Description("Pocess cart clicked and submitted.")]
        public const string ProcessCart = "ProcessCart";

        /// <summary>
        /// Indicates that the Login Activity for In Lane Registration was Successful
        /// </summary>
        [Description("Register Login Suucess Activity")]
        public const string RegisterLoginSuccess = "RegisterLoginSuccess";

        /// <summary>
        /// Replenish Existing Account with additional funds.
        /// </summary>
        [Description("Replenish Existing Account with additional funds.")]
        public const string Replenish = "Replenish";

        /// <summary>
        /// Make Account Payment
        /// </summary>
        [Description("Make Account Payment")]
        public const string MakePayment = "MakePayment";

        /// <summary>
        /// Vehicle Information Activity
        /// </summary>
        [Description("Vehicle Information Activity")]
        public const string VehicleInfo = "VehicleInfo";

        /// <summary>
        /// Save Customer Information Activity
        /// </summary>
        [Description("Save Customer Information Activity")]
        public const string SaveCustomer = "SaveCustomer";

        /// <summary>
        /// Save Vehicle Information Activity
        /// </summary>
        [Description("Save Vehicle Information Activity")]
        public const string SaveVehicle = "SaveVehicle";


        /// <summary>
        /// Indicates that the Login Activity for Replenish was Successful
        /// </summary>
        [Description("Replenish Login Suucess Activity")]
        public const string ReplenishLoginSuccess = "ReplenishLoginSuccess";


        /// <summary>
        /// Processed GetEPASS after GetEpass Login Success
        /// </summary>
        [Description("Processed GetEPASS after GetEpass Login Success")]
        public const string ProcessedGetEPASS = "ProcessedGetEPASS";

        /// <summary>
        /// Processed GetEPASS after GetEpass Login Success
        /// </summary>
        [Description("Processed Activate after GetEpass Login Success")]
        public const string ProcessedActivate = "ProcessedActivate";


        ///// <summary>
        ///// Indicates that the Login Activity for Manage Account was Successful
        ///// </summary>
        //[Description("Manage Account Login Success Activity")]
        //public const string ManageLoginSuccess = "ManageLoginSuccess";

        /// <summary>
        /// E-PASS Purchase and Replace Activity
        /// </summary>
        [Description("Forgot UserName Password Activity")]
        public const string ForgotUsernamePassword = "ForgotUsernamePassword";

        public static string UnauthenticatedActivities()
        {
            return ActivityTypes.NewAccount + "," + ActivityTypes.CreateUser;
        }
    }
}