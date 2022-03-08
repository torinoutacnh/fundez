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

using TIGE.Core.Models.Authentication;
using Elect.Core.LinqUtils;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TIGE.Core.Utils
{
    public class CookieHelper : Share.Utils.CookieHelper
    {
        private const string AuthKey = "Auth";


        public static AuthTrackingModel SetAuthTracking(UserSignInTrackingModel userSignInTracking,
            HttpContext httpContext, IDataProtectionProvider protectionProvider)
        {
            // Get Cookie

            var authTracking = Get<AuthTrackingModel>(AuthKey, httpContext, protectionProvider,
                                   SystemHelper.Setting.EncryptKey.ToString("N")) ?? new AuthTrackingModel();

            authTracking.Users = authTracking.Users.RemoveWhere(x => x.Id == userSignInTracking.Id).ToList();

            authTracking.Users.Add(userSignInTracking);

            authTracking.CurrentUserId = userSignInTracking.Id;

            // Set Cookie

            Set(AuthKey, authTracking, httpContext, protectionProvider,
                SystemHelper.Setting.EncryptKey.ToString("N"));

            return authTracking;
        }

        public static AuthTrackingModel GetAuthTracking(HttpContext httpContext,
            IDataProtectionProvider protectionProvider)
        {
            var authTracking = Get<AuthTrackingModel>(AuthKey, httpContext, protectionProvider,
                                   SystemHelper.Setting.EncryptKey.ToString("N")) ??
                               new AuthTrackingModel();

            return authTracking;
        }

        public static void EmptyAuthTracking(HttpContext httpContext, IDataProtectionProvider protectionProvider)
        {
            Set(AuthKey, new AuthTrackingModel(), httpContext, protectionProvider,
                SystemHelper.Setting.EncryptKey.ToString("N"));
        }
    }
}