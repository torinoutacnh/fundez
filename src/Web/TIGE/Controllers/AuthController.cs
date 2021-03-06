using System;
using System.Linq;
using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.ITempDataDictionaryUtils;
using Elect.Web.IUrlHelperUtils;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models.Authentication;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models.Authentication;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using CookieHelper = TIGE.Core.Utils.CookieHelper;

namespace TIGE.Controllers
{
    [AllowAnonymous]
    [Route(LandingEndpoint.Auth.IndexEndpoint)]
    public class AuthController : BaseController
    {

        public const string Endpoint = AreaName + "/auth";

        public const string OopsEndpoint = "oops";

        public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";


        private readonly IAuthenticationService _authenticationService;
        
        private readonly IUserService _userService;

        private readonly IDataProtectionProvider _dataProtectionProvider;

      
        public AuthController(IAuthenticationService authenticationService,
            IUserService userService,
            IDataProtectionProvider dataProtectionProvider)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _dataProtectionProvider = dataProtectionProvider;
        }

        #region Sign In

        /// <summary>
        ///     Sign In 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     Support GrantType: Implicit, Authorization Code (PKCE), Resource Owner Password.
        /// </remarks>
        [HttpGet]
        [Route(LandingEndpoint.Auth.SignInEndpoint)]
        public IActionResult SignIn()
        {
            AuthTrackingModel authTracking = CookieHelper.GetAuthTracking(HttpContext, _dataProtectionProvider);
            ViewBag.AuthTracking = authTracking;

            return View(new AuthorizeModel());
        }

        [HttpGet]
        [Route(LandingEndpoint.Auth.SignUpEndpoint)]
        public IActionResult SignUp([FromQuery] string code = null)
        {
            var model = new RegisterUserModel()
            {
                Code =  code
            };
            return View(model);
        }

        [HttpPost]
        [Route(LandingEndpoint.Auth.SignUpEndpoint)]
        public async Task<IActionResult> SubmitAdd([FromForm] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("SignUp", model);
            }

            try
            {
                var userId = await _userService.RegisterAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

                this.SetNotification(string.Format(Messages.User.AddedFormat, model.Email),
                    NotificationStatus.Success);

                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.Register, data = userId });
                //return RedirectToAction("SignIn");
            }
            catch (Exception e)
            {
                this.SetNotification( e.Message, NotificationStatus.Error);
                return View("SignUp", model);
            }
        }

        /// <summary>
        ///     Submit Sign In 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Support GrantType: Implicit, Authorization Code (PKCE), Resource Owner Password.
        /// </remarks>
        [HttpPost]
        [Route(LandingEndpoint.Auth.SignInEndpoint)]
        public async Task<IActionResult> SubmitSignIn([FromForm] AuthorizeModel model)
        {
            UserSignInTrackingModel userSignInTracking;

            AuthTrackingModel authTracking = CookieHelper.GetAuthTracking(HttpContext, _dataProtectionProvider);

            if (!ModelState.IsValid && !authTracking.Users.Any(x => x.IsLoggedIn && x.Id == authTracking.CurrentUserId))
            {
                return View("SignIn", model);
            }

            var @continue = model.Continue;

            if (string.IsNullOrWhiteSpace(@continue))
            {
                @continue = Url.AbsoluteAction("Index", "Home");
            }
            //if (string.IsNullOrWhiteSpace(@continue))
            //{
            //    @continue = Url.AbsoluteAction("Index", "Home", new { Area = "Stack"});
            //}

            // 1. Sign In
            if (authTracking.Users.Any(x => x.IsLoggedIn))
            {
                userSignInTracking = authTracking.Users.First(x => x.IsLoggedIn && x.Id == authTracking.CurrentUserId);
                userSignInTracking.LastUpdatedTime = CoreHelper.SystemTimeNow;
            }
            else
            {
                try
                {
                    var userModel = await _authenticationService
                        .SignInAsync(model.Email, model.Password, this.GetRequestCancellationToken())
                        .ConfigureAwait(true);
                    
                    userSignInTracking = userModel.MapTo<UserSignInTrackingModel>();
                }
                catch (CoreException e)
                {
                    if (e.Message == "User not activated")
                    {
                        var user = await _userService.GetByEmailAsync(model.Email);
                        return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.Register, data = user.Id });
                    }
                    else
                    {
                        Console.WriteLine(e);

                        this.SetNotification(e.Message, NotificationStatus.Error);

                        return View("SignIn", model);
                    }
                }
            }

            // login success
            if (userSignInTracking.Enable2FA && userSignInTracking.AuthyId != 0)
            {
                AuthorizeWithCodeModel verifyModel = model.MapTo<AuthorizeWithCodeModel>();
                return View("VeryfySignIn", verifyModel);
            }

            // Set UserName from signin tracking to model
            model.Email = userSignInTracking.Email;

            // 2. Set Cookie
            CookieHelper.SetAuthTracking(userSignInTracking, HttpContext, _dataProtectionProvider);

            // 3. Get Code or Access Token
            RequestTokenModel requestTokenModel = new RequestTokenModel
            {
                GrantType = model.GrantType,
                Email = model.Email,
                Password = model.Password,
                State = model.State,
                RedirectUri = @continue
            };

            var tokenModel = await _authenticationService
                .GetTokenAsync(requestTokenModel, this.GetRequestCancellationToken()).ConfigureAwait(true);

            if (userSignInTracking.Permissions.Contains(Permission.Admin))
            {
                @continue = Url.AbsoluteAction("Index", "Admin", new { Area = "Portal" });
            }
            //if (userSignInTracking.Permissions.Contains(Permission.Admin))
            //{
            //    @continue = Url.AbsoluteAction("Index", "User", new { Area = "StackAdmin" });
            //}

            @continue = @continue
                .SetQueryParam(nameof(tokenModel.AccessToken).ToLowerInvariant(), tokenModel.AccessToken)
                .SetQueryParam(nameof(tokenModel.TokenType).ToLowerInvariant(), tokenModel.TokenType)
                .SetQueryParam(nameof(tokenModel.ExpireIn).ToLowerInvariant(), tokenModel.ExpireIn)
                .SetQueryParam(nameof(tokenModel.State).ToLowerInvariant(), tokenModel.State);

            // 4. Redirect to Authorized to set a cookie to current site


            TempData.Set(Models.Constants.TempDataKey.RedirectUri, @continue);

            return RedirectToAction("Authorized", "Auth");
        }


        /// <summary>
        ///     Submit Sign In with Verify
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        ///     Support GrantType: Implicit, Authorization Code (PKCE), Resource Owner Password.
        /// </remarks>
        [HttpPost]
        [Route(LandingEndpoint.Auth.SignInWithCodeEndpoint)]
        public async Task<IActionResult> SubmitVerifySignIn([FromForm] AuthorizeWithCodeModel model)
        {
            UserSignInTrackingModel userSignInTracking;

            AuthTrackingModel authTracking = CookieHelper.GetAuthTracking(HttpContext, _dataProtectionProvider);

            if (!ModelState.IsValid && !authTracking.Users.Any(x => x.IsLoggedIn && x.Id == authTracking.CurrentUserId))
            {
                return View("SignIn", model);
            }

            var @continue = model.Continue;

            if (string.IsNullOrWhiteSpace(@continue))
            {
                @continue = Url.AbsoluteAction("Index", "Home");
            }


           


            // 1. Sign In
            if (authTracking.Users.Any(x => x.IsLoggedIn))
            {
                userSignInTracking = authTracking.Users.First(x => x.IsLoggedIn && x.Id == authTracking.CurrentUserId);
                userSignInTracking.LastUpdatedTime = CoreHelper.SystemTimeNow;
            }
            else
            {
                try
                {
                    var userModel = await _authenticationService
                        .SignInAsync(model.Email, model.Password, this.GetRequestCancellationToken())
                        .ConfigureAwait(true);

                    userSignInTracking = userModel.MapTo<UserSignInTrackingModel>();
                }
                catch (CoreException e)
                {
                    Console.WriteLine(e);

                    this.SetNotification(e.Message, NotificationStatus.Error);

                    return View("SignIn", model);
                }
            }

            // verify code
            try
            {
                await _authenticationService.VerifyAuthy(model.Code, userSignInTracking.Id);
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return View("VeryfySignIn", model);
            }
          

            // Set UserName from signin tracking to model
            model.Email = userSignInTracking.Email;

            // 2. Set Cookie
            CookieHelper.SetAuthTracking(userSignInTracking, HttpContext, _dataProtectionProvider);

            // 3. Get Code or Access Token
            RequestTokenModel requestTokenModel = new RequestTokenModel
            {
                GrantType = model.GrantType,
                Email = model.Email,
                Password = model.Password,
                State = model.State,
                RedirectUri = @continue
            };

            var tokenModel = await _authenticationService
                .GetTokenAsync(requestTokenModel, this.GetRequestCancellationToken()).ConfigureAwait(true);

            if (userSignInTracking.Permissions.Contains(Permission.Admin))
            {
                @continue = Url.AbsoluteAction("Index", "Admin", new { Area = "Portal" });
            }

            @continue = @continue
                .SetQueryParam(nameof(tokenModel.AccessToken).ToLowerInvariant(), tokenModel.AccessToken)
                .SetQueryParam(nameof(tokenModel.TokenType).ToLowerInvariant(), tokenModel.TokenType)
                .SetQueryParam(nameof(tokenModel.ExpireIn).ToLowerInvariant(), tokenModel.ExpireIn)
                .SetQueryParam(nameof(tokenModel.State).ToLowerInvariant(), tokenModel.State);

            // 4. Redirect to Authorized to set a cookie to current site


            TempData.Set(Models.Constants.TempDataKey.RedirectUri, @continue);

            return RedirectToAction("Authorized", "Auth");
        }

        /// <summary>
        ///     Authorized 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.AuthorizedEndpoint)]

        public IActionResult Authorized()
        {
            var redirectUri = TempData.Get<string>(Models.Constants.TempDataKey.RedirectUri);

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.AbsoluteAction("Index", "Home");
            }

            ViewBag.RedirectUri = redirectUri;

            return View();
        }

        #endregion Sign In

        #region Sign Out

        /// <summary>
        ///     Sign Out 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.SignOutEndpoint)]
        public IActionResult SignOut(string @continue)
        {
            // Redirect to Un-Authorize to set a cookie to current site
            TempData.Set(Models.Constants.TempDataKey.RedirectUri, @continue);

            // Cookie Handle

            var authTracking = CookieHelper.GetAuthTracking(HttpContext, _dataProtectionProvider);

            if (authTracking.Users.Any(x => x.IsLoggedIn))
            {
                // 1. Sign In Info
                var userSignInTracking = authTracking.Users.First(x => x.IsLoggedIn && x.Id == authTracking.CurrentUserId);

                userSignInTracking.IsLoggedIn = false;

                userSignInTracking.LastUpdatedTime = CoreHelper.SystemTimeNow;

                // 2. Set Cookie
                CookieHelper.SetAuthTracking(userSignInTracking, HttpContext, _dataProtectionProvider);
            }

            return RedirectToAction("UnAuthorize");
        }

        /// <summary>
        ///     Un Authorize 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.UnAuthorizeEndpoint)]
        public IActionResult UnAuthorize()
        {
            // Redirect

            var redirectUri = TempData.Get<string>(Models.Constants.TempDataKey.RedirectUri);

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.AbsoluteAction("SignIn", "Auth");
            }

            ViewBag.RedirectUri = redirectUri;

            return View();
        }

        #endregion Sign Out

        #region Forget Password

        /// <summary>
        ///     Forget Password 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.ForgetPasswordEndpoint)]
        public IActionResult ForgetPassword(ForgetPasswordModel model)
        {
            return View(model);
        }

        /// <summary>
        ///     Submit Forget Password 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LandingEndpoint.Auth.ForgetPasswordEndpoint)]
        public async Task<IActionResult> SubmitForgetPassword([FromForm] ForgetPasswordModel model)
        {
            var user = await _userService.GetByEmailAsync(model.Email, this.GetRequestCancellationToken());

            if (user == null)
            {
                this.SetNotification( string.Format(Messages.User.DoesNotExist), NotificationStatus.Error);

                return View("ForgetPassword", model);
            }

            if (user.EmailConfirmedTime.HasValue)
            {
                await _authenticationService.SendSetPasswordAsync(user.Id, this.GetRequestCancellationToken());

                this.SetNotification( Messages.Auth._001, NotificationStatus.Success);

                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.ForgetPassword.ToString(), data = user.Id });
            }
            else
            {
                await _authenticationService.SendConfirmEmailAsync(user.Id, this.GetRequestCancellationToken());
                
                this.SetNotification( Messages.Auth._002, NotificationStatus.Success);

                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.Register.ToString(), data = user.Id });
            }
        }

        #endregion Forget Password

        #region Confirm Account
        
        /// <summary>
        ///     Confirm Email
        /// </summary>
        /// <param name="token">Confirm Email Token via Email</param>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.ConfirmEmailEndpoint)]
        public IActionResult ConfirmEmail(string token)
        {
            try
            {
                _authenticationService.CheckConfirmEmailToken(token);
            }
            catch (CoreException ex)
            {
                this.SetNotification( ex.Message, NotificationStatus.Error);

                return RedirectToAction("SignIn");
            }

            var confirmEmailModel = new ConfirmEmailModel
            {
                Token = token
            };

            return View();
        }
        
        #endregion
        
        #region Set Password
        
        /// <summary>
        ///     Set Password
        /// </summary>
        /// <param name="token">Set Password Token via Email</param>
        /// <returns></returns>
        [HttpGet]
        [Route(LandingEndpoint.Auth.ChangePasswordEndpoint)]
        public IActionResult SetPassword(string token)
        {
            try
            {
                _authenticationService.CheckSetPasswordToken(token);
            }
            catch (CoreException ex)
            {
                this.SetNotification( ex.Message, NotificationStatus.Error);

                return RedirectToAction("ForgetPassword");
            }

            var setPasswordModel = new SetPasswordModel
            {
                Token = token
            };

            return View(setPasswordModel);
        }
        
        [HttpPost]
        [Route(LandingEndpoint.Auth.ChangePasswordEndpoint)]
        public async Task<IActionResult> SubmitSetPassword([FromForm] SetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("SetPassword", model);
            }
            try
            {
                await _authenticationService.SetPasswordAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

                this.SetNotification( Messages.Auth._003, NotificationStatus.Success);

                return RedirectToAction("SignIn");
            }
            catch (CoreException ex)
            {
                this.SetNotification( ex.Message, NotificationStatus.Error);
                
                return RedirectToAction("ForgetPassword");
            }
        }

        #endregion
        
        #region Validation

        [Route(LandingEndpoint.Auth.CheckUniqueEmailEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueEmail(string email, string id = null)
        {
            try
            {
                _userService.CheckUniqueEmail(email, id);
                return Json(true);
            }
            catch (CoreException ex)
            {
                if (ex.Code == nameof(ErrorCode.NotUnique))
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(LandingEndpoint.Auth.CheckUniquePhoneEndpoint)]
        [HttpPost]
        public JsonResult CheckUniquePhone(string phone, string id = null)
        {
            try
            {
                _userService.CheckUniquePhone(phone, id);
                return Json(true);
            }
            catch (CoreException ex)
            {
                if (ex.Code == nameof(ErrorCode.NotUnique))
                {
                    return Json(false);
                }

                throw;
            }
        }


        [Route(LandingEndpoint.Auth.CheckUniqueAddressEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueAddress(string address, string id = null)
        {
            try
            {
                _userService.CheckUniqueAddress(address, id);
                return Json(true);
            }
            catch (CoreException ex)
            {
                if (ex.Code == nameof(ErrorCode.NotUnique))
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(LandingEndpoint.Auth.CheckUniqueUserNameEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueUserName(string username, string id = null)
        {
            try
            {
                return Json(true);
            }
            catch (CoreException ex)
            {
                if (ex.Code == nameof(ErrorCode.NotUnique))
                {
                    return Json(false);
                }

                throw;
            }
        }

        [Route(LandingEndpoint.Auth.CheckUniqueTxHashEndpoint)]
        [HttpPost]
        public JsonResult CheckUniqueTxHash(string txHash, string id = null)
        {
            try
            {
                _userService.CheckUniqueTxHash(txHash, id);
                return Json(true);
            }
            catch (CoreException ex)
            {
                if (ex.Code == nameof(ErrorCode.NotUnique))
                {
                    return Json(false);
                }

                throw;
            }
        }

        #endregion

        #region Helper

        private bool IsValid(AuthorizeModel model)
        {
            if (model.GrantType == GrantType.AuthorizationCodePKCE || model.GrantType == GrantType.AuthorizationCode ||
                model.GrantType == GrantType.Implicit)
            {
                if (string.IsNullOrWhiteSpace(model.Continue))
                {
                    return false;
                }
            }

            if (model.GrantType == GrantType.AuthorizationCodePKCE)
            {
                if (string.IsNullOrWhiteSpace(model.CodeChallenge))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Helper
    }
}