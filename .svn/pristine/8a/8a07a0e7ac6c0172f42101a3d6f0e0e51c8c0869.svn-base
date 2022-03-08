#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> CookieHelper.cs </Name>
//         <Created> 30/04/2018 11:00:01 AM </Created>
//         <Key> 78ca020e-3747-4d79-a9f8-e040ed91f162 </Key>
//     </File>
//     <Summary>
//         CookieHelper.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Utils
{
    public class CookieHelper
    {
        public static void Set<T>(string key, T data, HttpContext httpContext,
            IDataProtectionProvider protectionProvider, string encryptKey)
        {
            var protector = GetDataProtector(protectionProvider, encryptKey);

            var dataJson = JsonConvert.SerializeObject(data, Formattings.JsonSerializerSettings);

            var protectedDataJson = protector.Protect(dataJson);

            httpContext.Response.Cookies.Append(key, protectedDataJson, new CookieOptions
            {
                Expires = null,
                HttpOnly = true,
                Secure = false,
            });
        }

        public static T Get<T>(string key, HttpContext httpContext, IDataProtectionProvider protectionProvider, string encryptKey)
        {
            var protector = GetDataProtector(protectionProvider, encryptKey);

            if (!httpContext.Request.Cookies.TryGetValue(key, out var protectedDataJson))
            {
                return default;
            }

            try
            {
                var dataJson = protector.Unprotect(protectedDataJson);
                
                var data = JsonConvert.DeserializeObject<T>(dataJson, Formattings.JsonSerializerSettings);

                return data;
            }
            catch
            {
                return default;
            }
        }
        
        private static IDataProtector GetDataProtector(IDataProtectionProvider protectionProvider, string encryptKey)
        {
            return protectionProvider.CreateProtector(encryptKey);
        }
    }
}