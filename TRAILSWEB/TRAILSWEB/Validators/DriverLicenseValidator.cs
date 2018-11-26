using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json.Linq;

public static class DriverLicenseValidator
{
    //public static bool IsValid(string licenseNumber, string state)
    //{
    //    bool validPlate = false;

    //    if (!string.IsNullOrEmpty(licenseNumber) & !string.IsNullOrEmpty(state))
    //    {
    //        HttpServerUtility server = HttpContext.Current.Server;

    //        string validatorPath = server.MapPath(ConfigurationManager.AppSettings["ValidatorPath"]);

    //        // Load Driver License Validation Rules
    //        JObject rules = JObject.Parse(File.ReadAllText(validatorPath + @"\\driver-license-rules.json"));

    //        try
    //        {
    //            Regex licenseFormat = new Regex((string)rules[state]["rule"]);

    //            // Validate Driver License Number
    //            validPlate = licenseFormat.IsMatch(licenseNumber);
    //        }
    //        catch
    //        {
    //            // On Error report TRUE as there is nothing more we can do for the Validation
    //            validPlate = true;
    //        }
    //    }

    //    return validPlate;
    //}
}
