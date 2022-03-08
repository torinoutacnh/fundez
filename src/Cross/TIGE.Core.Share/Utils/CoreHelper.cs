#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SystemHelper.cs </Name>
//         <Created> 21/04/2018 5:34:40 PM </Created>
//         <Key> 3bd06eed-ba10-4098-8ed5-aa26ce08dae1 </Key>
//     </File>
//     <Summary>
//         SystemHelper.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Elect.Core.DateTimeUtils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TIGE.Core.Share.Constants;
using HttpContext = Elect.Web.Middlewares.HttpContextMiddleware.HttpContext;

namespace TIGE.Core.Share.Utils
{
    public static class CoreHelper
    {
        public static Microsoft.AspNetCore.Http.HttpContext CurrentHttpContext => HttpContext.Current;

        #region Date Time Utils

        public static TimeZoneInfo SystemTimeZoneInfo => DateTimeHelper.GetTimeZoneInfo(Formattings.TimeZone);

        public static DateTimeOffset SystemTimeNow => DateTimeOffset.UtcNow.UtcToSystemTime();

        public static DateTime UtcToSystemTime(this DateTimeOffset dateTimeOffsetUtc)
        {
            return dateTimeOffsetUtc.UtcDateTime.UtcToSystemTime();
        }

        public static DateTime UtcToSystemTime(this DateTime dateTimeUtc)
        {
            var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, SystemTimeZoneInfo);

            return dateTimeWithTimeZone;
        }

        public static DateTimeOffset? ToSystemDateTime(this string dateTimeString)
        {
            DateTimeOffset result;

            if (DateTime.TryParseExact(dateTimeString, Formattings.DateTimeFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var dateTime))
            {
                result = dateTime;
            }
            else if (DateTime.TryParseExact(dateTimeString, Formattings.DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
            {
                result = date;
            }
            else
            {
                return null;
            }

            result = result.Date.WithTimeZone(Formattings.TimeZone);

            return result;
        }

        public static TimeSpan? ToSystemTimeSpan(this string timeSpanString)
        {
            TimeSpan result;

            if (DateTime.TryParseExact(timeSpanString, Formattings.TimeFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var dateTime))
            {
                result = dateTime.TimeOfDay;
            }
            else if (TimeSpan.TryParse(timeSpanString, CultureInfo.InvariantCulture, out var timeSpan))
            {
                result = timeSpan;
            }
            else
            {
                return null;
            }

            return result;
        }

        public static string ToSystemString(this TimeSpan timeSpan)
        {
            var result = DateTime.Today.Add(timeSpan).ToString(Formattings.TimeFormat);

            return result;
        }

        #endregion

        public static ClaimsIdentity GetClaimsIdentity<T>(T data)
        {
            var properties = typeof(T).GetProperties();

            // Object to Dictionary<string, object>
            var dictionaryObj = new Dictionary<string, object>();

            foreach (var propertyInfo in properties)
            {
                var key = propertyInfo.Name;

                var value = propertyInfo.GetValue(data)?.ToString();

                dictionaryObj.Add(key, value);
            }

            // Dictionary<string, object> to Json
            var json = JsonConvert.SerializeObject(dictionaryObj, Formattings.JsonSerializerSettings);

            // Json to Dictionary<string, string>
            var dictionaryStr = JsonConvert.DeserializeObject<Dictionary<string, string>>(json, Formattings.JsonSerializerSettings);

            // Build Claims
            List<Claim> claims = new List<Claim>();

            foreach (var key in dictionaryStr.Keys)
            {
                var value = dictionaryStr[key];

                if (value == null)
                {
                    continue;
                }

                claims.Add(new Claim(key, value));
            }

            var claimIdentity = new ClaimsIdentity(claims, TokenType.AuthTokenType);

            return claimIdentity;
        }

        public static byte[] StringEncode(this string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        public static string HashHMACBase64(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            var result = hash.ComputeHash(message);
            var resultText = Convert.ToBase64String(result);
            return resultText;
        }


        public static string CalculateNonce()
        {
            //Allocate a buffer
            var ByteArray = new byte[20];
            //Generate a cryptographically random set of bytes
            using (var Rnd = RandomNumberGenerator.Create())
            {
                Rnd.GetBytes(ByteArray);
            }
            //Base64 encode and then return

            int i = BitConverter.ToInt32(ByteArray, 0);
            //return Convert.ToBase64String(ByteArray);
            return i.ToString();
        }
    }
}