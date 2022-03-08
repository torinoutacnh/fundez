#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> AuthenticationService.cs </Name>
//         <Created> 21/04/2018 5:45:54 PM </Created>
//         <Key> abe00b25-6581-48fa-bbfc-b4ca602b9b9a </Key>
//     </File>
//     <Summary>
//         AuthenticationService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.Application;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Models.Authentication;
using Elect.Core.DateTimeUtils;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.HttpDetection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Authentication;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using SystemHelper = TIGE.Core.Utils.SystemHelper;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IAuthenticationService))]
    public class AuthenticationService : Base.Service, IAuthenticationService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IMemoryCache _memoryCache;

        private readonly IRepository<UserEntity> _userRepo;

        private readonly IRepository<RefreshTokenEntity> _refreshTokenRepo;

        private readonly IDataProtector _protector;

        public AuthenticationService(IUnitOfWork unitOfWork,
            IDataProtectionProvider protectionProvider,
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;

            _memoryCache = memoryCache;

            _userRepo = unitOfWork.GetRepository<UserEntity>();

            _refreshTokenRepo = unitOfWork.GetRepository<RefreshTokenEntity>();

            _protector = protectionProvider.CreateProtector(SystemHelper.Setting.EncryptKey.ToString("N"));
        }

        // Set Password

        public async Task<string> SendSetPasswordAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = _userRepo.Get(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.User.DoesNotExist);
            }

            if (user.SetPasswordTokenExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }

            var setPasswordTokenExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            string setPasswordToken = StringHelper.Generate(6, false, false, true, false);

            _userRepo.Update(new UserEntity
            {
                Id = id,
                SetPasswordToken = setPasswordToken,
                SetPasswordTokenExpireTime = setPasswordTokenExpireTime
            }, x => x.SetPasswordToken, x => x.SetPasswordTokenExpireTime);

            UnitOfWork.SaveChanges();

            var emailService = _serviceProvider.GetService<IEmailService>();

            await emailService.SendAsync(id, EmailTemplate.ChangePassword, cancellationToken).ConfigureAwait(true);

            return setPasswordToken;
        }

        public void CheckSetPasswordToken(string token, CancellationToken cancellationToken = default)
        {
            var user = _userRepo.Get(x => x.SetPasswordToken == token).FirstOrDefault();
            if (user == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (user.SetPasswordTokenExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }
        }

        public Task SetPasswordAsync(SetPasswordModel model, CancellationToken cancellationToken = default)
        {
            var systemTimeNow = CoreHelper.SystemTimeNow;

            var userEntity = _userRepo.Get(x => x.SetPasswordToken == model.Token).Single();

            userEntity.SetPasswordToken = null;

            userEntity.SetPasswordTokenExpireTime = null;

            userEntity.PasswordLastUpdatedTime = systemTimeNow;

            userEntity.PasswordHash = HashPassword(model.Password, userEntity.PasswordLastUpdatedTime.Value);

            _userRepo.Update(userEntity,
                x => x.SetPasswordToken,
                x => x.SetPasswordTokenExpireTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        // Confirm Email

        public async Task<string> SendConfirmEmailAsync(string id, CancellationToken cancellationToken = default)
        {
            var confirmEmailTokenExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            var confirmEmailToken = StringHelper.Generate(6, false, false, true, false);

            var user = _userRepo.Get(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return confirmEmailToken;
            }

            if (user.ConfirmEmailTokenExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, "Please wait 5 minutes to receive new verify code!");
            }

            _userRepo.Update(new UserEntity
            {
                Id = id,
                ConfirmEmailToken = confirmEmailToken,
                ConfirmEmailTokenExpireTime = confirmEmailTokenExpireTime
            }, x => x.ConfirmEmailToken, x => x.ConfirmEmailTokenExpireTime);

            UnitOfWork.SaveChanges();

            var emailService = _serviceProvider.GetService<IEmailService>();

            await emailService.SendAsync(id, EmailTemplate.VerifyEmail, cancellationToken).ConfigureAwait(true);

            return confirmEmailToken;
        }

        public void CheckConfirmEmailToken(string token, CancellationToken cancellationToken = default)
        {
            var checkTime = CoreHelper.SystemTimeNow;

            var confirmEmailTokenExpireTime = _userRepo.Get(x => x.ConfirmEmailToken == token)
                .Select(x => x.ConfirmEmailTokenExpireTime).FirstOrDefault();

            if (confirmEmailTokenExpireTime == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (confirmEmailTokenExpireTime < checkTime)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            var systemTimeNow = CoreHelper.SystemTimeNow;
            var userEntity = _userRepo.Get(x => x.ConfirmEmailToken == token).Single();

            userEntity.ConfirmEmailToken = null;
            userEntity.ConfirmEmailTokenExpireTime = null;
            userEntity.EmailConfirmedTime = userEntity.PasswordLastUpdatedTime = systemTimeNow;

            _userRepo.Update(userEntity,
                x => x.ConfirmEmailToken,
                x => x.ConfirmEmailTokenExpireTime,
                x => x.EmailConfirmedTime);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

        }

        public Task ConfirmEmailAsync(ConfirmEmailModel model, CancellationToken cancellationToken = default)
        {
            var systemTimeNow = CoreHelper.SystemTimeNow;

            var userEntity = _userRepo.Get(x => x.ConfirmEmailToken == model.Token).Single();

            userEntity.ConfirmEmailToken = null;

            userEntity.ConfirmEmailTokenExpireTime = null;

            userEntity.Email = model.Email;

            userEntity.EmailConfirmedTime = userEntity.PasswordLastUpdatedTime = systemTimeNow;

            userEntity.PasswordHash = HashPassword(model.Password, userEntity.PasswordLastUpdatedTime.Value);

            _userRepo.Update(userEntity,
                x => x.ConfirmEmailToken,
                x => x.ConfirmEmailTokenExpireTime,
                x => x.Email,
                x => x.EmailConfirmedTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<UserModel> SignInAsync(string email, string password,
            CancellationToken cancellationToken = default)
        {
            CheckValidSignIn(email, password);

            var userModel = GetUserByEmail(email);

            return Task.FromResult(userModel);
        }

        // Authentication

        public async Task<TokenModel> GetTokenAsync(RequestTokenModel model,
            CancellationToken cancellationToken = default)
        {
            var systemNow = CoreHelper.SystemTimeNow;
            var JwtExpirationSecond = 600;

            var tokenModel = new TokenModel
            {
                ExpireIn = systemNow.AddSeconds(JwtExpirationSecond).ToTimestamp(),
                State = model.State,
                TokenType = TokenType.AuthTokenType
            };

            switch (model.GrantType)
            {
                case GrantType.AuthorizationCode:
                case GrantType.AuthorizationCodePKCE:
                    {
                        AuthorizeCodeStorageModel authorizeCodeStorageModel = CheckAndGetValidCode(model);

                        var userBasicInfo = GetUserByEmail(authorizeCodeStorageModel.UserName);

                        tokenModel.AccessToken = JwtHelper.Generate(userBasicInfo, JwtExpirationSecond);

                        tokenModel.RefreshToken = GenerateRefreshToken(userBasicInfo.Id);

                        break;
                    }
                case GrantType.Implicit:
                    {
                        var userBasicInfo = GetUserByEmail(model.Email);

                        tokenModel.AccessToken = JwtHelper.Generate(userBasicInfo, JwtExpirationSecond);

                        break;
                    }
                case GrantType.ResourceOwner:
                    {
                        CheckValidSignIn(model.Email, model.Password);

                        var userBasicInfo = GetUserByEmail(model.Email);

                        tokenModel.AccessToken = JwtHelper.Generate(userBasicInfo, JwtExpirationSecond);

                        tokenModel.RefreshToken = GenerateRefreshToken(userBasicInfo.Id);

                        break;
                    }
                case GrantType.RefreshToken:
                    {
                        CheckValidRefreshToken(model.RefreshToken);

                        tokenModel.RefreshToken = model.RefreshToken;

                        var userBasicInfo = GetUserByRefreshToken(model.RefreshToken);

                        tokenModel.AccessToken = JwtHelper.Generate(userBasicInfo, JwtExpirationSecond);

                        break;
                    }
                case GrantType.ClientCredential:
                    {
                        // Check valid Client Id and Client Secret

                        // Client Id as User Name. Client Secret as Password

                        break;
                    }
            }

            return tokenModel;
        }

        public Task ExpireRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var entities = _refreshTokenRepo.Get(x => x.RefreshToken == refreshToken).ToList();

            if (entities.Any() != true)
            {
                return Task.CompletedTask;
            }

            var systemTimePast = CoreHelper.SystemTimeNow.AddSeconds(-1);

            foreach (var entity in entities)
            {
                entity.ExpireOn = systemTimePast;

                _refreshTokenRepo.Update(entity, x => x.ExpireOn);
            }

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task ExpireAllRefreshTokenAsync(string id, CancellationToken cancellationToken = default)
        {
            var listRefreshToken =
                _refreshTokenRepo.Get(x => x.UserId == id).Select(x =>
                        new RefreshTokenEntity
                        {
                            Id = x.Id
                        })
                    .ToList();

            var systemTimePast = CoreHelper.SystemTimeNow.AddSeconds(-1);

            foreach (var refreshTokenEntity in listRefreshToken)
            {
                refreshTokenEntity.ExpireOn = systemTimePast;

                _refreshTokenRepo.Update(refreshTokenEntity, x => x.ExpireOn);
            }

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        // Code

        public Task<string> GetCodeAsync(RequestCodeModel model, CancellationToken cancellationToken = default)
        {
            AuthorizeCodeStorageModel authorizeCodeStorageModel = model.MapTo<AuthorizeCodeStorageModel>();

            // Generate Code
            authorizeCodeStorageModel.Code = SecurityHelper.GenerateSalt();

            // Save to cache
            var expireIn = CoreHelper.SystemTimeNow.AddMinutes(SystemHelper.Setting.AuthorizeCodeStorageSeconds);

            _memoryCache.Set(authorizeCodeStorageModel.Code, authorizeCodeStorageModel, expireIn);

            // Encode to Response
            authorizeCodeStorageModel.Code = Elect.Web.StringUtils.StringHelper.EncodeBase64Url(authorizeCodeStorageModel.Code);

            return Task.FromResult(authorizeCodeStorageModel.Code);
        }

        public async Task VerifyAuthy(string modelCode, string userId, CancellationToken cancellationToken = default)
        {
            var user = _userRepo.Get(x => x.Id == userId).FirstOrDefault();
            if (user == null || user.AuthyId == 0)
            {
                throw new CoreException(nameof(ErrorCode.NotFound), ErrorCode.NotFound);
            }

            try
            {
                var url = $"https://api.authy.com/protected/json/verify/{modelCode}/{user.AuthyId}";
                FlurlClient requestClient = new FlurlClient(url);
                requestClient.Headers["X-Authy-API-Key"] = SystemHelper.Setting.AuthyAppSecretKey;
                var response = await requestClient.Request().GetJsonAsync<VerifyAuthyResponseModel>();
            }
            catch (Exception e)
            {
                throw new CoreException(nameof(ErrorCode.BadRequest), ErrorCode.AuthCodeInValid);
            }
        }

        #region Helper

        public static string HashPassword(string password, DateTimeOffset hashTime)
        {
            var passwordSalt = hashTime.ToString("O") + SystemHelper.Setting.EncryptKey;

            var passwordHash = SecurityHelper.HashPassword(password, passwordSalt);

            return passwordHash;
        }

        private string GenerateToken()
        {
            //var token = _protector.Protect(SecurityHelper.GenerateSalt());
            var token = Guid.NewGuid().ToString("N");

            return token;
        }

        private bool IsValidToken(string token)
        {
            try
            {
                var salt = _protector.Protect(token);

                return !string.IsNullOrWhiteSpace(salt);
            }
            catch
            {
                return false;
            }
        }

        private string GenerateRefreshToken(string id)
        {
            var refreshToken = Guid.NewGuid().ToString();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = refreshToken,
                UserId = id,
                TotalUsage = 1,
                ExpireOn = null
            };

            var deviceInfo = CoreHelper.CurrentHttpContext?.Request.GetDeviceInformation();

            deviceInfo?.MapTo(refreshTokenEntity);

            _refreshTokenRepo.Add(refreshTokenEntity);

            UnitOfWork.SaveChanges();

            return refreshToken;
        }

        private void CheckValidSignIn(string email, string password, bool isApp = false)
        {
            var query = _userRepo.Get(x =>x.Email.ToLower() == email.ToLower());

            var user = query.Select(x => new
            {
                x.PasswordHash,
                x.PasswordLastUpdatedTime,
                x.BannedTime,
                x.BannedRemark,
                x.ConfirmEmailToken,
                x.EmailConfirmedTime
            })
                .FirstOrDefault();

            // Check Exist
            if (user == null)
            {
                throw new CoreException(nameof(ErrorCode.UserPasswordWrong), ErrorCode.UserPasswordWrong);
            }

            // Check activated
            if (!string.IsNullOrWhiteSpace(user.ConfirmEmailToken))
            {
                throw new CoreException(nameof(ErrorCode.BadRequest), "User not activated");
            }

            // Check Password
            if (user.PasswordLastUpdatedTime == null)
            {
                throw new CoreException(nameof(ErrorCode.UserPasswordWrong), ErrorCode.UserPasswordWrong);
            }

            password = HashPassword(password, user.PasswordLastUpdatedTime.Value);

            if (password != user.PasswordHash)
            {
                throw new CoreException(nameof(ErrorCode.UserPasswordWrong), ErrorCode.UserPasswordWrong);
            }

            // Check Banned

            if (user.BannedTime != null)
            {
                throw new CoreException(nameof(ErrorCode.UserBanned), user.BannedRemark);
            }
        }

        private void CheckValidRefreshToken(string refreshToken)
        {
            var refreshTokenInfo = _refreshTokenRepo.Get(x => x.RefreshToken == refreshToken).FirstOrDefault();

            if (refreshTokenInfo == null)
            {
                throw new CoreException(nameof(ErrorCode.NotFound), "The refresh token is not found.");
            }

            if (refreshTokenInfo.ExpireOn.HasValue && refreshTokenInfo.ExpireOn.Value < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(nameof(ErrorCode.TokenExpired), "The refresh token is expired.");
            }
        }

        private UserModel GetUserByEmail(string email)  
        {
            var user = _userRepo.Get(x => x.Email.ToLower() == email.ToLower())
                .QueryTo<UserModel>()
                .FirstOrDefault();

            if (user != null)
            {
                var permisson = user.Permissions.Split(',').Select(x => (Permission) int.Parse(x)).ToList();
                user.ListPermission = permisson;
            }

            return user;
        }

        private UserModel GetUserByRefreshToken(string refreshToken)
        {
            var user = _refreshTokenRepo.Get(x => x.RefreshToken == refreshToken).Select(x => x.User)
                .QueryTo<UserModel>().Single();

            if (user != null && !string.IsNullOrWhiteSpace(user.Permissions))
            {
                user.ListPermission = user.Permissions.Split(',').Select(x => (Permission)int.Parse(x)).ToList();
            }

            return user;
        }

        private AuthorizeCodeStorageModel CheckAndGetValidCode(RequestTokenModel model)
        {
            // Decode to get Data in Cache
            model.Code = Elect.Web.StringUtils.StringHelper.DecodeBase64Url(model.Code);

            _memoryCache.TryGetValue(model.Code, out var data);

            var authorizeCodeStorageModel = data as AuthorizeCodeStorageModel;

            if (authorizeCodeStorageModel == null)
            {
                throw new CoreException(nameof(ErrorCode.NotFound), "The code is not found.");
            }

            if (model.RedirectUri != authorizeCodeStorageModel.RedirectUri)
            {
                throw new CoreException(nameof(ErrorCode.AuthCodeInValid), ErrorCode.AuthCodeInValid);
            }

            var codeHash = model.CodeVerifier;

            if (authorizeCodeStorageModel.CodeChallengeMethod == CodeChallengeMethod.S256)
            {
                codeHash = SecurityHelper.EncryptSha256(model.CodeVerifier);
            }

            if (authorizeCodeStorageModel.CodeChallenge != codeHash)
            {
                throw new CoreException(nameof(ErrorCode.AuthCodeInValid), ErrorCode.AuthCodeInValid);
            }

            _memoryCache.Remove(model.Code);

            return authorizeCodeStorageModel;
        }

        #endregion
    }
}