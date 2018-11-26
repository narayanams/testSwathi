using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

public static class ExtensionMethods
{
    static readonly string _PasswordHash = "()()<3@#";
    static readonly string _SaltKey = "|<3/43@!";
    static readonly string _VIKey = "@1B2c3D4e5F6g7H8";
    static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

    #region Numeric Data Helper Extensions
    public static bool IsNumeric(this object Expression)
    {
        if (Expression == null || Expression is DateTime)
        {
            return false;
        }

        if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
        {
            return true;
        }

        string value = Expression.ToString();

        double test;
        return double.TryParse(value, out test);
    }

    public static bool IsInteger(this string source)
    {
        int test;
        return int.TryParse(source, out test);
    }

    public static bool IsDouble(this string source)
    {
        double test;
        return double.TryParse(source, out test);
    }

    public static bool IsOdd(this int value)
    {
        return value % 2 != 0;
    }

    public static bool IsEven(this int value)
    {
        return value % 2 == 0;
    }
    #endregion

    #region Data Type Conversions

    /// <summary>
    /// Converts a given Integer to a GUID
    /// <remarks>
    /// <para>Useful in conjunction with ToInt for validating an Integer to a given GUID</para>
    /// <example>
    /// <para>GUID Example: 714107b3-d951-451c-b7cb-bdbe5dc71c32</para>
    /// <para>INT Derived from GUID: 1900087219</para>
    /// <para>GUID Derived from INT: 714107b3-0000-0000-0000-000000000000</para>
    /// <para>Notice that the first segment of the converted GUID from the INT only gives us the first segment of the original GUID.</para>
    /// <para>This may be useful in scenarios where one wants to track a Session with a GUID and transfer that GUID to the client but not send 
    /// the entire GUID but Validate Server-Side.</para>
    /// </example>
    /// </remarks>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid ToGuid(this int value)
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }

    /// <summary>
    /// Converts a  given String to a GUID
    /// <remarks>Useful for converting strings that are actually GUIDs to actualy GUID Types</remarks>
    /// </summary>
    /// <param name="value"><see cref="string"/></param>
    /// <returns><see cref="Guid"/></returns>
    public static Guid ToGuid(this string value)
    {
        Guid guidValue = new Guid();

        Guid.TryParse(value, out guidValue);

        return guidValue;
    }

    public static bool ToBoolean(this string value)
    {
        bool returnValue;

        bool.TryParse(value, out returnValue);

        return returnValue;
    }

    /// <summary>
    /// Converts a GUID to an Integer
    /// <remarks>
    /// <para>NOTE: For Validation only. Only the first segment gets converted.</para>
    /// </remarks>
    /// </summary>
    /// <param name="value">Input GUID Value</param>
    /// <returns><see cref="int"/></returns>
    public static int ToInt(this Guid value)
    {
        byte[] b = value.ToByteArray();
        int bint = BitConverter.ToInt32(b, 0);
        return bint;
    }

    public static int ToInt(this string value)
    {
        // Verify value is not NULL
        value = String.IsNullOrEmpty(value) ? "" : value;

        // If validation fails, Default return value to 0
        int returnValue = 0;

        // Test if Integer, with or without comma separator
        Regex regex = new Regex(@"^(\d|,)*\d*$");
        Match match = regex.Match(value);

        // If Valid Integer, Parse and Return Number
        if (match.Success)
        {
            // Strip out non-numeric characters
            string cleanValue = Regex.Replace(value, "[^0-9]+", "");

            // Safely Parse to desired Data Type
            int.TryParse(cleanValue, out returnValue);
        }

        return returnValue;
    }

    public static short ToShort(this string value)
    {
        // Verify value is not NULL
        value = String.IsNullOrEmpty(value) ? "" : value;

        // If validation fails, Default return value to 0
        short returnValue = 0;

        // Test if Integer, with or without comma separator
        Regex regex = new Regex(@"^(\d|,)*\d*$");
        Match match = regex.Match(value);

        // If Valid Integer, Parse and Return Number
        if (match.Success)
        {
            // Strip out non-numeric characters
            string cleanValue = Regex.Replace(value, "[^0-9]+", "");

            // Safely Parse to desired Data Type
            short.TryParse(cleanValue, out returnValue);
        }

        return returnValue;
    }

    public static double ToDouble(this string value, int? roundDigits = null)
    {
        // Verify value is not NULL
        value = String.IsNullOrEmpty(value) ? "" : value;

        double returnValue = 0.0D;

        // Test if Numeric with possible Currency, Thousands Separator and Decimals
        // RegEx Source: http://stackoverflow.com/questions/354044/what-is-the-best-u-s-currency-regex#answer-354276
        Regex regex = new Regex(@"^\$?\-?([1-9]{1}[0-9]{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\-?\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\(\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))\)$");
        Match match = regex.Match(value);

        if (match.Success)
        {
            // Strip out non-numeric characters
            string cleanValue = Regex.Replace(value, @"[^0-9\.\-]+", "");

            // Safely Parse to desired Data Type
            double.TryParse(cleanValue, out returnValue);

            // Round the value if requested
            if (roundDigits.HasValue)
            {
                returnValue = Math.Round(returnValue, (int)roundDigits);
            }
        }

        return returnValue;
    }

    public static decimal ToDecimal(this string value, int? roundDigits = null)
    {
        // Verify value is not NULL
        value = String.IsNullOrEmpty(value) ? "" : value;

        decimal returnValue = 0.0M;

        // Test if Numeric with possible Currency, Thousands Separator and Decimals
        // RegEx Source: http://stackoverflow.com/questions/354044/what-is-the-best-u-s-currency-regex#answer-354276
        Regex regex = new Regex(@"^\$?\-?([1-9]{1}[0-9]{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\-?\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))$|^\(\$?([1-9]{1}\d{0,2}(\,\d{3})*(\.\d{0,2})?|[1-9]{1}\d{0,}(\.\d{0,2})?|0(\.\d{0,2})?|(\.\d{1,2}))\)$");
        Match match = regex.Match(value);

        if (match.Success)
        {
            // Strip out non-numeric characters
            string cleanValue = Regex.Replace(value, @"[^0-9\.\-]+", "");

            // Safely Parse to desired Data Type
            decimal.TryParse(cleanValue, out returnValue);

            // Round the value if requested
            if (roundDigits.HasValue)
            {
                returnValue = Math.Round(returnValue, (int)roundDigits);
            }
        }

        return returnValue;
    }

    public static decimal ToDecimal(this double value)
    {
        decimal number;

        Decimal.TryParse(value.ToString(), out number);

        return number;
    }

    #endregion

    #region Date and Time Extension Methods
    /// <summary>
    /// Verify if given data is a valid Month
    /// </summary>
    /// <param name="value">Month to Test</param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsValidMonth(this int value)
    {
        var validMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        return (validMonths.Contains(value));
    }

    /// <summary>
    /// Verify if given data is a valid Day
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Can optionally verify based on give Year and Month for more precision.</para>
    /// </remarks>
    /// </summary>
    /// <param name="value">Day to Test</param>
    /// <param name="month">Optional Month for more precise validation.</param>
    /// <param name="year">Optional Year for more precise validation.</param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsValidDay(this int value, int? month = null, int? year = null)
    {
        // Default Maximum Days to 31
        int maxDays = 31;

        if (month.HasValue && year.HasValue)
        {
            // Determine Valid Days in a given Month accounting for Leap Year
            maxDays = DateTime.DaysInMonth((int)year, (int)month);
        }

        // Validate given Value
        return (value >= 1 && value <= maxDays);
    }

    /// <summary>
    /// Convert a given Date to a Month Name
    /// </summary>
    /// <param name="dateTime">Date to Convert</param>
    /// <returns>Month Name String</returns>
    public static string ToMonthName(this DateTime dateTime)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
    }

    /// <summary>
    /// Convert a given Date to an abbreviated Month Name
    /// </summary>
    /// <param name="dateTime">Date to Convert</param>
    /// <returns>Abbreviated Month Name String</returns>
    public static string ToShortMonthName(this DateTime dateTime)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
    }

    /// <summary>
    /// Converts DateTime to Unix/JSON Date Format
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>JSON/Unix Date in the form of a <see cref="long"/></returns>
    public static long ToJsonDate(this DateTime dateTime)
    {
        return (dateTime - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond;
    }
    #endregion

    #region Credit Card Extension Methods
    /// <summary>
    /// Validate Credit Card Expiration Month
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Will determine if given Expiration Date is still valid based on today's Date</para>
    /// </remarks>
    /// </summary>
    /// <param name="month">Month to Test</param>
    /// <param name="year">Year to Test</param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsValidExpirationMonth(this int month, int year)
    {
        var validMonths = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        if (validMonths.Contains(month) && year >= DateTime.Now.Year)
        {
            DateTime expiration = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return expiration >= DateTime.Now.Date;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Security Related String Extension Methods
    /// <summary>
    /// Encrypt String using AES (Rijndael) Cipher
    /// <remarks>
    /// <para>Requires that the Method Variables: _PasswordHash, _SaltKey and _VIKey be set as appropriate.</para>
    /// </remarks>
    /// </summary>
    /// <param name="plainText">Plan Text Input String</param>
    /// <returns>Encrypted String</returns>
    public static string Encrypt(this string plainText)
    {
        // Check for Null String
        plainText = (string.IsNullOrWhiteSpace(plainText)) ? string.Empty : plainText;

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] keyBytes = new Rfc2898DeriveBytes(_PasswordHash, Encoding.ASCII.GetBytes(_SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(_VIKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
            }
        }
        return Convert.ToBase64String(cipherTextBytes);
    }


    /// <summary>
    /// Decrypt AES (Rijndael) Encrypted String
    /// <remarks>
    /// <para>Requires that the Method Variables: _PasswordHash, _SaltKey and _VIKey be set as appropriate.</para>
    /// </remarks>
    /// </summary>
    /// <param name="encryptedText">Encrypted Data</param>
    /// <returns>Plain Text</returns>
    public static string Decrypt(this string encryptedText)
    {
        // Check for Null String
        encryptedText = (string.IsNullOrWhiteSpace(encryptedText)) ? string.Empty : encryptedText;

        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(_PasswordHash, Encoding.ASCII.GetBytes(_SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(_VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }

    /// <summary>
    /// Add or Update Claims based on given Security Principal
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Usage: <code>User.AddUpdateClaim("key1", "value");</code></para>
    /// <para>If the Value given is NULL the code will silently ignore and add nothing.</para>
    /// </remarks>
    /// </summary>
    /// <param name="currentPrincipal"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    //public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
    //{
    //    var identity = currentPrincipal.Identity as ClaimsIdentity;
    //    if (identity == null)
    //        return;

    //    // Check for Existing Claim and Remove it
    //    var existingClaim = identity.FindFirst(key);
    //    if (existingClaim != null)
    //        identity.RemoveClaim(existingClaim);

    //    // Add New Claim
    //    if (value != null)
    //    {
    //        identity.AddClaim(new Claim(key, value));
    //        var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
    //        authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
    //    }
    //}

    /// <summary>
    /// Add or Update Errors into Claims based on given Security Principal
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Usage: <code>User.AddUpdateClaim("key1", sring array);</code></para>
    /// <para>If the Value given is NULL the code will silently ignore and add nothing.</para>
    /// </remarks>
    /// </summary>
    /// <param name="currentPrincipal"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    //public static void AddUpdateTransferErrorsClaim(this IPrincipal currentPrincipal, string key, string[] value)
    //{
    //    var identity = currentPrincipal.Identity as ClaimsIdentity;
    //    if (identity == null)
    //        return;

    //    // Add New Claim
    //    if (value != null && value.GetLength(0) > 0)
    //    {
    //        List<Claim> claims = new List<Claim>();

    //        foreach(string singleValue in value)
    //        {
    //            claims.Add(new Claim(key, singleValue));
    //        }
    //        identity.AddClaims(claims);

    //        var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
    //        authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
    //    }
    //}


    /// <summary>
    /// Get a list of error from a given key
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Usage: <code>var key1 = User.GetClaim("key1");</code></para>
    /// </remarks>
    /// </summary>
    /// <param name="currentPrincipal"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    //public static string[] GetTransferErrors(this IPrincipal currentPrincipal, string key)
    //{
    //    var identity = currentPrincipal.Identity as ClaimsIdentity;
    //    if (identity == null)
    //        return null;

    //    // Attempt to Retrieve Claim Value
    //    string[] claimValue = identity.Claims.Where(claim => claim.Type == key)
    //        .Select(claim => claim.Value).ToArray<string>();

    //    return claimValue;
    //}
    //public static void RemoveClaim(this IPrincipal currentPrincipal, string key)
    //{
    //    var identity = currentPrincipal.Identity as ClaimsIdentity;
    //    if (identity == null)
    //        return;

    //    // Check for Existing Claim and Remove it
    //    var existingClaim = identity.FindFirst(key);
    //    if (existingClaim != null)
    //        identity.RemoveClaim(existingClaim);
    //}

    ///// <summary>
    ///// Get a given Claim Value from a given Security Principal
    ///// <remarks>
    ///// <para>&#160;</para>
    ///// <para>Usage: <code>var key1 = User.GetClaim("key1");</code></para>
    ///// </remarks>
    ///// </summary>
    ///// <param name="currentPrincipal"></param>
    ///// <param name="key"></param>
    ///// <returns></returns>
    //public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
    //{
    //    var identity = currentPrincipal.Identity as ClaimsIdentity;
    //    if (identity == null)
    //        return null;

    //    // Attempt to Retrieve Claim Value
    //    string claimValue = identity.Claims.Where(claim => claim.Type == key)
    //        .Select(claim => claim.Value).SingleOrDefault();

    //    return claimValue;
    //}

    /// <summary>
    /// Creates an MD5 Hash Value from a given String
    /// <remarks>
    /// <para>This is a one-way function and only creates a Hash but does not "Encrypt"</para>
    /// <para>NOTE: As of 2012 MD5 was considered "cryptographically broken and unsuitable for further use". 
    /// See <a href="https://en.wikipedia.org/wiki/MD5">More information here</a></para>
    /// </remarks>
    /// </summary>
    /// <param name="value"><see cref="string"/> to Hash</param>
    /// <returns>128-bit (16-byte) Hash Value (32-digit Hexadecimal Number)</returns>
    public static string ToMD5Hash(this string value)
    {
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));

        // Create a new Stringbuilder to collect the bytes and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("X2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    /// <summary>
    /// Returns an Insecure String from the SecureString Source Data
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Source: http://stackoverflow.com/questions/9887996/how-to-convert-a-string-to-securestring-explicitly#answer-31491863</para>
    /// </remarks>
    /// </summary>
    /// <param name="secureString"><see cref="SecureString"/> Data</param>
    /// <returns>Insecure <see cref="String"/></returns>
    public static string ToUnsecureString(this SecureString secureString)
    {
        if (secureString == null)
        {
            return null;
        }
        else
        {
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }

    /// <summary>
    /// Returns a Secure String from the Source String
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Source: http://stackoverflow.com/questions/9887996/how-to-convert-a-string-to-securestring-explicitly#answer-31491863</para>
    /// </remarks>
    /// </summary>
    /// <param name="unsecureString">Non-Secure String</param>
    /// <returns><see cref="SecureString"/></returns>
    public static SecureString ToSecureString(this string unsecureString)
    {
        if (string.IsNullOrWhiteSpace(unsecureString))
        {
            return null;
        }
        else
        {
            return unsecureString.Aggregate(new SecureString(), AppendChar, MakeReadOnly);
        }
    }

    /// <summary>
    /// Helper Method to Make a SecureString object Read Only
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Source: http://stackoverflow.com/questions/9887996/how-to-convert-a-string-to-securestring-explicitly#answer-31491863</para>
    /// </remarks>
    /// </summary>
    /// <param name="secureString"><see cref="SecureString"/> Source Object</param>
    /// <returns>Read Only <see cref="SecureString"/></returns>
    private static SecureString MakeReadOnly(SecureString secureString)
    {
        secureString.MakeReadOnly();
        return secureString;
    }

    /// <summary>
    /// Helper Method used when Creating a SecureString from an Insecure String
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Source: http://stackoverflow.com/questions/9887996/how-to-convert-a-string-to-securestring-explicitly#answer-31491863</para>
    /// </remarks>
    /// </summary>
    /// <param name="secureChar"><see cref="SecureString"/></param>
    /// <param name="c">Character Value</param>
    /// <returns><see cref="SecureString"/> version of supplied Character</returns>
    private static SecureString AppendChar(SecureString secureChar, char c)
    {
        secureChar.AppendChar(c);
        return secureChar;
    }
    #endregion

    #region List and Collection Extension Methods
    /// <summary>
    /// Convert Enumeration to SelectList
    /// </summary>
    /// <typeparam name="TEnum">Enumeration Type</typeparam>
    /// <param name="enumObj">Enumeration</param>
    /// <returns><see cref="SelectList"/> Collection</returns>
    public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        where TEnum : struct, IComparable, IFormattable, IConvertible
    {
        var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                     select new { Id = e, Name = e.ToString() };
        return new SelectList(values, "Id", "Name", enumObj);
    }

    /// <summary>
    /// Extension Metiod to verify that a Generic List is not NULL or Empty
    /// </summary>
    /// <typeparam name="T">List Type</typeparam>
    /// <param name="source"><see cref="List"/> Source</param>
    /// <returns><see cref="bool"/> (true/false)</returns>
    public static bool IsNullOrEmpty<T>(this List<T> source)
    {
        if (source != null && source.Any())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    /// <summary>
    /// Remove all elements of an Object Model from ModelState
    /// <remarks>
    /// <para>This is helpful for removing Optional Object Models from a ViewModel before processing.</para>
    /// <para>See <a href="http://stackoverflow.com/questions/6843171/is-it-correct-way-to-use-modelstate-remove-to-deal-with-modelstate">Stackoverflow Discussion here</a></para>
    /// </remarks>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="modelState"></param>
    /// <param name="expression"></param>
    public static void RemoveFor<TModel>(this ModelStateDictionary modelState,
                                             Expression<Func<TModel, object>> expression)
    {
        string expressionText = ExpressionHelper.GetExpressionText(expression);

        foreach (var ms in modelState.ToArray())
        {
            if (ms.Key.StartsWith(expressionText + ".") || ms.Key == expressionText)
            {
                modelState.Remove(ms);
            }
        }
    }

    /// <summary>
    /// Verify if a Nullable INT is NULL or has a given Value
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Useful for testing if NULL or 0</para>
    /// </remarks>
    /// </summary>
    /// <param name="value">Nullable <see cref="int"/> Source Object</param>
    /// <param name="valueToCheck">Value to Check</param>
    /// <returns><see cref="bool"/> True/False</returns>
    public static bool IsNullOrValue(this int? value, int valueToCheck)
    {
        return (value ?? valueToCheck) == valueToCheck;
    }

    /// <summary>
    /// Verify if Nullable Double is NULL or has a given Value
    /// <remarks>
    /// <para>&#160;</para>
    /// <para>Useful for testing if NULL or 0</para>
    /// </remarks>
    /// </summary>
    /// <param name="value">Nullable <see cref="double"/> Source Object</param>
    /// <param name="valueToCheck">Value to Check</param>
    /// <returns><see cref="bool"/> True/False</returns>
    public static bool IsNullOrValue(this double? value, double valueToCheck)
    {
        return (value ?? valueToCheck) == valueToCheck;
    }

    /// <summary>
    /// Verify if given string is a valid GUID
    /// </summary>
    /// <param name="value"><see cref="string"/> to Test</param>
    /// <returns><see cref="bool"/> result (true/false)</returns>
    public static bool IsGuid(this string value)
    {
        Guid test = Guid.Empty;

        return Guid.TryParse(value, out test);
    }

    #region Enumeration Extension Methods

    /// <summary>
    /// Retrieve the Description of a given Enum
    /// </summary>
    /// <param name="value"><see cref="Enum"/> Source Object</param>
    /// <returns><see cref="String"/> Description</returns>
    public static string GetDescription(this Enum value)
    {
        if (value == null)
        {
            throw new ArgumentNullException("value");
        }

        string desc = value.ToString();

        FieldInfo info = value.GetType().GetField(desc);
        var attrs = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attrs != null && attrs.Length > 0)
        {
            desc = attrs[0].Description;
        }

        return desc;
    }

    #endregion

    public static string RelativePath(this HttpServerUtilityBase srv, string path, HttpRequestBase context)
    {
        return path.Replace(context.ServerVariables["APPL_PHYSICAL_PATH"], string.Empty).Replace(@"\", "/");
    }

    public static bool IsHtml(this string source)
    {
        return Regex.IsMatch(source, @"/<[a-z][\s\S]*>/");
    }

    #region String Format Extension Methods

    /// <summary>
    /// Use the current thread's culture info for conversion
    /// </summary>
    public static string ToTitleCase(this string source)
    {
        var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
        return cultureInfo.TextInfo.ToTitleCase(source.ToLower());
    }

    /// <summary>
    /// Overload which uses the culture info with the specified name
    /// </summary>
    public static string ToTitleCase(this string source, string cultureInfoName)
    {
        var cultureInfo = new CultureInfo(cultureInfoName);
        return cultureInfo.TextInfo.ToTitleCase(source.ToLower());
    }

    /// <summary>
    /// Overload which uses the specified culture info
    /// </summary>
    public static string ToTitleCase(this string source, CultureInfo cultureInfo)
    {
        return cultureInfo.TextInfo.ToTitleCase(source.ToLower());
    }
    #endregion
}