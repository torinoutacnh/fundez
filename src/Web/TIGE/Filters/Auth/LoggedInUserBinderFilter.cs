using System.Linq;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using TIGE.Contract.Service;
using TIGE.Core.Models.Authentication;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;

namespace TIGE.Filters.Auth
{
    public class LoggedInUserBinderFilter : IAuthorizationFilter
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IUserService _userService;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            BindLoggedInUser(context.HttpContext);
        }

        public LoggedInUserBinderFilter(IDataProtectionProvider dataProtectionProvider, IUserService userService)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _userService = userService;
        }

        public void BindLoggedInUser(HttpContext httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    return;
                }

                // Get Sign In Tracking from Authorize Header or Cookie
                var userSignInTrackingModel = GetUserSignInTrackingModel(httpContext, out string accessToken);

                if (userSignInTrackingModel == null)
                {
                    return;
                }
                
                // Get Logged In User
                var userModelTask = _userService.GetByIdAsync(userSignInTrackingModel.Id);
                
                userModelTask.Wait();
                
                var userModel = userModelTask.Result;
                
                if (!string.IsNullOrWhiteSpace(userSignInTrackingModel.Id) && userModel == null)
                {
                    // In case user has signin but does not exist anymore => clear cookie
                    Core.Utils.CookieHelper.EmptyAuthTracking(httpContext, _dataProtectionProvider);
                }

                if (userModel != null)
                {
                    LoggedInUser.Current  = userModel.MapTo<LoggedInUserModel>();
                    LoggedInUser.Current.AccessToken = accessToken;
                }
            }
            catch (System.Exception e)
            {
                // Ignore
            }
        }

        public UserSignInTrackingModel GetUserSignInTrackingModel(HttpContext httpContext, out string token)
        {
            UserSignInTrackingModel userSignInTrackingModel = null;

            // Check User Basic Info in Token of Header
            token = httpContext.Request.Headers[HeaderKey.Authorization].ToString();

            if (!string.IsNullOrWhiteSpace(token))
            {
                token = token.Replace(TokenType.AuthTokenType, string.Empty)?.Trim();

                var isValid = JwtHelper.IsValid(token);

                var isExpire = JwtHelper.IsExpire(token);

                if (isValid && !isExpire)
                {
                    userSignInTrackingModel = JwtHelper.Get<UserSignInTrackingModel>(token);
                }
            }
            else
            {
                // Check Logged User in Cookie
                var authTracking = Core.Utils.CookieHelper.GetAuthTracking(httpContext, _dataProtectionProvider);

                userSignInTrackingModel = authTracking.Users.FirstOrDefault(x => x.IsLoggedIn && authTracking.CurrentUserId == x.Id);
            }

            return userSignInTrackingModel;
        }
    }
}