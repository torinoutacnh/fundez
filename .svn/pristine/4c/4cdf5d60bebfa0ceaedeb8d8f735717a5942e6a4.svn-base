#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> EmailService.cs </Name>
//         <Created> 21/04/2018 7:30:52 PM </Created>
//         <Key> f9843199-cfd1-4f7a-8ac5-cd90bd94f728 </Key>
//     </File>
//     <Summary>
//         EmailService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.EmailProvider;
using TIGE.Core.Models.Email;
using TIGE.Core.Utils;
using Elect.DI.Attributes;
using Flurl;
using Flurl.Http;
using Hangfire;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Contract.Repository.Models.Stacking;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IEmailService))]
    public class EmailService : Base.Service, IEmailService
    {
        private readonly IEmailProvider _emailProvider;
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<UserSlotsEntity> _userSlotRepo;
        private readonly IRepository<UserSellTokenEntity> _userSellTokenRepo;
        private readonly IRepository<UserWithDrawRequestEntity> _withdrawRequestRepository;
        private readonly IRepository<HistoryWithdrawTokenEntity> _withdrawStackRepository;
        private readonly IRepository<UserDepositRequestEntity> _depositRequestRepository;
        private readonly IRepository<HistoryDepositEntity> _historyDepositRepository;
        private readonly IRepository<StackHistoryEntity> _stackHistoryRepository;
        private readonly IRepository<HistoryWithdrawUSDEntity> _withdrawStackUSDRepository;

        public EmailService(IUnitOfWork unitOfWork, IEmailProvider emailProvider) : base(unitOfWork)
        {
            _emailProvider = emailProvider;
            _userRepo = unitOfWork.GetRepository<UserEntity>();
            _withdrawRequestRepository = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
            _depositRequestRepository = unitOfWork.GetRepository<UserDepositRequestEntity>();
            _userSlotRepo = unitOfWork.GetRepository<UserSlotsEntity>();
            _userSellTokenRepo = unitOfWork.GetRepository<UserSellTokenEntity>();
            _historyDepositRepository = unitOfWork.GetRepository<HistoryDepositEntity>();
            _withdrawStackRepository = unitOfWork.GetRepository<HistoryWithdrawTokenEntity>();
            _stackHistoryRepository = unitOfWork.GetRepository<StackHistoryEntity>();
            _withdrawStackUSDRepository = unitOfWork.GetRepository<HistoryWithdrawUSDEntity>();
        }

        public Task SendAsync(string userId, EmailTemplate template, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (template)
            {
                case EmailTemplate.VerifyEmail:
                    {
                        BackgroundJob.Enqueue(() => SendVerifyEmail(userId));
                        break;
                    }
                case EmailTemplate.Welcome:
                    {
                        BackgroundJob.Enqueue(() => SendWelcome(userId));
                        break;
                    }
                case EmailTemplate.ChangePassword:
                    {
                        BackgroundJob.Enqueue(() => SendChangePassword(userId));
                        break;
                    }
                case EmailTemplate.UpdateProfile:
                    {
                        BackgroundJob.Enqueue(() => SendUpdateProfile(userId));
                        break;
                    }
                case EmailTemplate.BuySlot:
                    {
                        BackgroundJob.Enqueue(() => SendBuySlot(userId));
                        break;
                    }
                case EmailTemplate.SellToken:
                    {
                        BackgroundJob.Enqueue(() => SendSellToken(userId));
                        break;
                    }
            }
            
            return Task.CompletedTask;
        }

        public void SendBuySlot(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Include(x => x.TempProfiles).FirstOrDefault();

            var token = _userSlotRepo.Get(x => x.UserId == userId).OrderByDescending(x => x.CreatedTime)
                .FirstOrDefault();

            //private const string BaseEndpoint = "~/" + AreaName + "/wallets";

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                UserId = userId,
                Template = EmailTemplate.BuySlot,
                Url = SystemHelper.GetAbsoluteEndpoint("~/slots/confirm").SetQueryParam("token", token.Id),
                Code = token.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"{SystemHelper.Setting.ApplicationName} - Verify Buy Token Request.", template).Wait();
        }


        public void SendBuySlotById(string slotId)
        {

            var token = _userSlotRepo.Get(x => x.Id == slotId).FirstOrDefault();
            var userInfo = _userRepo.Get(x => x.Id == token.UserId).Include(x => x.TempProfiles).FirstOrDefault();

            //private const string BaseEndpoint = "~/" + AreaName + "/wallets";

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                UserId = token?.UserId,
                Template = EmailTemplate.BuySlot,
                Url = SystemHelper.GetAbsoluteEndpoint("~/slots/confirm").SetQueryParam("token", token.Id),
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"{SystemHelper.Setting.ApplicationName} - Verify Buy Token Request.", template).Wait();
        }

        public void SendSellToken(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Include(x => x.TempProfiles).FirstOrDefault();

            var token = _userSellTokenRepo.Get(x => x.UserId == userId && x.ConfirmToken != null).OrderByDescending(x => x.CreatedTime).FirstOrDefault();

            //private const string BaseEndpoint = "~/" + AreaName + "/wallets";

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                UserId = userId,
                Template = EmailTemplate.SellToken,
                Url = SystemHelper.GetAbsoluteEndpoint("~/tokens/confirm").SetQueryParam("token", token?.Id),
                Code = token?.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"{SystemHelper.Setting.ApplicationName} - Verify Sell Token Request.", template).Wait();
        }

        public void SendUpdateProfile(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Include(x => x.TempProfiles).FirstOrDefault();

            var profile = userInfo.TempProfiles.FirstOrDefault();
            var token = profile?.ConfirmEmailToken;

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                UserId = userId,
                Template = EmailTemplate.UpdateProfile,
                Url = SystemHelper.GetAbsoluteEndpoint("~/profile/confirm").SetQueryParam("token", token),
                Code = token,
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"{SystemHelper.Setting.ApplicationName} - Your profile have changed.", template).Wait();
        }

        public Task SendVerifyWithdrawAsync(string withDrawId, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _withdrawRequestRepository.Get(x => x.Id == withDrawId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(LandingEndpoint.Wallet.ConfirmWithdrawEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.WithDraw,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Withdraw Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        //fix
        public Task SendVerifyWithdrawTigeAsync(string withDrawId, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _withdrawRequestRepository.Get(x => x.Id == withDrawId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(LandingEndpoint.Wallet.ConfirmWithdrawTigeEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.WithDrawTige,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Withdraw Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        public Task SendVerifyTransferTigeAsync(string withDrawId, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _withdrawRequestRepository.Get(x => x.Id == withDrawId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(LandingEndpoint.Wallet.ConfirmWithdrawTigeEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.TransferTige,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Transfer Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        public Task SendVerifyDepositAsync(string depositId, in CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _depositRequestRepository.Get(x => x.Id == depositId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(LandingEndpoint.Wallet.ConfirmDepositEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.Deposit,
                Code = withRawInfo?.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Deposit Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;



        }

        public Task SendVerifyDepositStackAsync(string depositId, in CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _historyDepositRepository.Get(x => x.Id == depositId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(StackEndPoint.StackingWallet.ConfirmDepositEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.DepositStack,
                Code = withRawInfo?.ConfirmToken,
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Deposit Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        public void SendVerifyEmail(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();


            if (userInfo == null)
            {
                return;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(LandingEndpoint.Auth.ConfirmEmailEndpoint).SetQueryParam("token", userInfo?.ConfirmEmailToken),
                ExpireTime = userInfo.ConfirmEmailTokenExpireTime,
                UserId = userId,
                Template = EmailTemplate.VerifyEmail,
                Code = userInfo?.ConfirmEmailToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Confirm Your Registration", template).Wait();
        }

        public void SendWelcome(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Select(x => new
            {
                x.Email
            }).Single();

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                UserId = userId,
                Template = EmailTemplate.Welcome
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"Welcome to {SystemHelper.Setting.ApplicationName}", template).Wait();
        }

        public void SendChangePassword(string userId)
        {
            var userInfo = _userRepo.Get(x => x.Id == userId).Select(x => new
            {
                x.Email,
                x.SetPasswordToken,
                x.SetPasswordTokenExpireTime
            }).Single();

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Auth.ChangePasswordEndpoint).SetQueryParam("token", userInfo.SetPasswordToken),
                ExpireTime = userInfo.SetPasswordTokenExpireTime,
                UserId = userId,
                Template = EmailTemplate.ChangePassword,
                Code = userInfo.SetPasswordToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();

            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Reset Password", template).Wait();
        }

        public Task SendVerifyWithdrawStackAsync(string withDrawId, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _withdrawStackRepository.Get(x => x.Id == withDrawId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(StackEndPoint.StackingWallet.ConfirmWithdrawEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.WithdrawStack,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Withdraw Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        public Task SendVerifyStackAsync(string stackid, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _stackHistoryRepository.Get(x => x.Id == stackid).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(StackEndPoint.StackingWallet.ConfirmWithdrawEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.Stack,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify start staking Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }

        public Task SendVerifyWithdrawUSDStackAsync(string withDrawId, CancellationToken cancellationToken = default)
        {
            var userInfo = _userRepo.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.Email,
                x.ConfirmEmailToken,
                x.ConfirmEmailTokenExpireTime
            }).FirstOrDefault();

            var withRawInfo = _withdrawStackUSDRepository.Get(x => x.Id == withDrawId).FirstOrDefault();

            if (userInfo == null || withRawInfo == null)
            {
                return Task.CompletedTask;
            }

            EmailModel model = new EmailModel
            {
                ApplicationName = SystemHelper.Setting.ApplicationName,
                Url = SystemHelper.GetAbsoluteEndpoint(StackEndPoint.StackingWallet.ConfirmWithdrawUSDEndpoint).SetQueryParam("token", withRawInfo?.ConfirmToken),
                ExpireTime = withRawInfo.ExpireTime,
                UserId = LoggedInUser.Current.Id,
                Template = EmailTemplate.WithdrawUSDStack,
                Code = withRawInfo.ConfirmToken
            };

            var task = SystemHelper.GetAbsoluteEndpoint(PortalEndpoint.Email.EmailTemplateEndpoint).SetQueryParams(model).GetStringAsync();
            task.Wait();

            var template = task.Result;

            _emailProvider.SendAsync(userInfo.Email, $"[{SystemHelper.Setting.ApplicationName}] Verify Withdraw Request", template, SystemHelper.Setting.NotifyEmail).Wait();
            return Task.CompletedTask;
        }
    }
}