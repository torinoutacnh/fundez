#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserService.cs </Name>
//         <Created> 20/04/2018 6:50:54 PM </Created>
//         <Key> c9211419-02f0-4cd3-87ac-064059a194e6 </Key>
//     </File>
//     <Summary>
//         UserService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;
using SystemHelper = TIGE.Core.Utils.SystemHelper;
using QRCoder;
using ZXing;
using TIGE.Contract.Repository.Models.Stacking;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IUserService))]
    public class UserService : Base.Service, IUserService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<UserTempProfileEntity> _temProfileRepo;
        private readonly IRepository<UserWalletEntity> _userWalletRepo;
        private readonly IRepository<UserSlotsEntity> _userSlotRepo;
        private readonly IRepository<UserSellTokenEntity> _userSellTokenRepo;
        private readonly IRepository<UserTokensEntity> _userTokenRepo;
        private readonly IRepository<UserBusinessEntity> _userBusinessRepo;
        private readonly IRepository<UserDepositRequestEntity> _userDepositRepo;
        private readonly IRepository<UserWithDrawRequestEntity> _userWithdrawRepo;
        private readonly IRepository<StackingWalletEntity> _stackWalletRepository;
        private readonly IRepository<HistoryDepositEntity> _historydepositRepository;
        private readonly IRepository<HistoryWithdrawTokenEntity> _stackWithdrawRepository;
        private readonly IRepository<HistoryRefundEntity> _refundRepository;
        private readonly IRepository<StackHistoryEntity> _stackHistoryRepository;
        private readonly IRepository<TransferTokenEntity> _transferRepository;

        // Cache

        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKeyPrefix = "User_";

        private readonly TimeSpan _cacheSliding = TimeSpan.FromHours(4);

        public UserService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMemoryCache memoryCache, IEmailService emailService, IRepository<UserTokensEntity> userTokenRepo,ITokenService tokenService) :
            base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _tokenService = tokenService;
            _userTokenRepo = userTokenRepo;
            _userRepo = unitOfWork.GetRepository<UserEntity>();
            _userWalletRepo = unitOfWork.GetRepository<UserWalletEntity>();
            _userSlotRepo = unitOfWork.GetRepository<UserSlotsEntity>();
            _temProfileRepo = unitOfWork.GetRepository<UserTempProfileEntity>();
            _userBusinessRepo = unitOfWork.GetRepository<UserBusinessEntity>();
            _userWithdrawRepo = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
            _userDepositRepo = unitOfWork.GetRepository<UserDepositRequestEntity>();
            _userSellTokenRepo = unitOfWork.GetRepository<UserSellTokenEntity>();
            _stackWalletRepository = unitOfWork.GetRepository<StackingWalletEntity>();
            _historydepositRepository = unitOfWork.GetRepository<HistoryDepositEntity>();
            _stackWithdrawRepository = unitOfWork.GetRepository<HistoryWithdrawTokenEntity>();
            _refundRepository = unitOfWork.GetRepository<HistoryRefundEntity>();
            _stackHistoryRepository = unitOfWork.GetRepository<StackHistoryEntity>();
            _transferRepository = unitOfWork.GetRepository<TransferTokenEntity>();
        }

        // Get

        public Task<PagedResponseModel<UserModel>> GetPagedAsync(PagedRequestModel model,
            CancellationToken cancellationToken = default)
        {
            var query = _userRepo.Get();

            var total = query.Count();

            var items = query.QueryTo<UserModel>().OrderByDescending(x => x.CreatedTime).Skip(model.Skip)
                .Take(model.Take).ToList();

            var pagedResponse = new PagedResponseModel<UserModel> { Total = total, Items = items };

            return Task.FromResult(pagedResponse);
        }

        public Task<DataTableResponseModel<UserModel>> GetDataTableAsync(DataTableRequestModel model,
            CancellationToken cancellationToken = default)
        {
            var query = _userRepo.Get();

            var listData = query.QueryTo<UserModel>();

            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (UserModel)x);
            var userIds = data.Select(x => x.Id).ToList();
            var wallets = _userWalletRepo.Get(x => userIds.Contains(x.UserId)).ToList();
            var walletIds = wallets.Select(x => x.Id);
            var deposit = _userDepositRepo.Get(x => walletIds.Contains(x.WalletId) && x.Status == Enums.WalletDepositStatus.Approved).ToList();
            var withDraw = _userWithdrawRepo.Get(x => walletIds.Contains(x.WalletId) 
            && x.Status!= Enums.WithdrawStatus.Reject 
            && x.Status != Enums.WithdrawStatus.TokenReject
            && x.Status != Enums.WithdrawStatus.NewTransfer
            && x.Status != Enums.WithdrawStatus.ConfirmingTransfer
            && x.Status != Enums.WithdrawStatus.ApprovedTransfer
            && x.Status != Enums.WithdrawStatus.RejectTransfer
            ).ToList();
            var slots = _userSlotRepo.Get(x => userIds.Contains(x.UserId)).ToList();
            var sells = _userSellTokenRepo.Get(x => userIds.Contains(x.UserId)).ToList();
            var business = _userBusinessRepo.Get(x => userIds.Contains(x.ToUserId)).ToList();

            foreach (var userModel in data)
            {
                var wallet = wallets.FirstOrDefault(x => x.UserId == userModel.Id);
                if (wallet == null)
                {
                    continue;
                }

                userModel.TotalDeposit = deposit.Where(x => x.WalletId == wallet.Id).Sum(x => x.AmountUSD).ToString("#,###,###,###.##");

                var withdrawRequestUSD = withDraw.Where(
                    x => x.WalletId == wallet.Id && (x.Status == Enums.WithdrawStatus.New
                || x.Status == Enums.WithdrawStatus.Confirming
                || x.Status == Enums.WithdrawStatus.Approved));
                var withdrawUSD = withdrawRequestUSD.Sum(x => x.AmountUSD);
                var feeUSD = withdrawRequestUSD.Sum(x => x.FeeUSD);
                userModel.TotalWithdraw = withdrawUSD.ToString("#,###,###,###.##");
                userModel.TotalFee = feeUSD.ToString("#,###,###,###.##");

                var withdrawRequestToken = withDraw.Where(
                    x => x.WalletId == wallet.Id
                && (x.Status == Enums.WithdrawStatus.TokenNew
                || x.Status == Enums.WithdrawStatus.TokenConfirming
                || x.Status == Enums.WithdrawStatus.TokenApproved));
                var withdrawToken = withdrawRequestToken.Sum(x => x.AmountUSD);
                var feeTige = withdrawRequestToken.Sum(x => x.FeeUSD);
                userModel.TotalWithdrawTige = withdrawToken.ToString("#,###,###,###.##");
                userModel.TotalFeeTige = feeTige.ToString("#,###,###,###.##");
                userModel.TotalSlot = slots.Where(x => x.UserId == userModel.Id).Sum(x => x.Quantity).ToString("#,###,###,###");

                var totalToken = slots.Where(x => x.UserId == userModel.Id).Sum(x => x.TokenQuantity);

                var sell = sells.Where(x => x.UserId == userModel.Id).Sum(x => x.TokenQuantity);

                userModel.TotalToken = (totalToken - sell - withdrawToken - feeTige).ToString("#,###,###,###");



                userModel.TotalCommision = business.Where(x => x.ToUserId == userModel.Id).Sum(x => x.Commission).ToString("#,###,###,###.##");
                userModel.Balance = wallet.AmountUSD.ToString("#,###,###,###.##");
            }

            return Task.FromResult(result);
        }

        public Task<UserModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var cacheKey = _cacheKeyPrefix + id;

            //// Check Cache
            _memoryCache.TryGetValue(cacheKey, out var data);
            if (data is UserModel user)
            {
                return Task.FromResult(user);
            }

            // Get Data
            user = _userRepo.Get(x => x.Id == id).QueryTo<UserModel>().SingleOrDefault();

            if (user != null && !string.IsNullOrWhiteSpace(user.Permissions))
            {
                user.ListPermission = user.Permissions.Split(',').Select(x => (Permission)int.Parse(x)).ToList();
            }

            if (user != null)
            {
                var wallet = _userWalletRepo.Get(x => x.UserId == user.Id).FirstOrDefault();
                user.Address = wallet?.AddressWallet;
                user.WalletId = wallet?.Id;
                if (wallet != null)
                {
                    user.WalletBalance = wallet.AmountUSD > 0 ? wallet.AmountUSD : 0;
                }
                else
                {
                    user.WalletBalance = 0d;
                }

                if (wallet != null)
                {
                    var deposit = _userDepositRepo.Get(x => x.WalletId == wallet.Id && x.Status == Enums.WalletDepositStatus.Approved).ToList();
                    var withDraw = _userWithdrawRepo.Get(x => x.WalletId == wallet.Id && x.Status != Enums.WithdrawStatus.Reject).ToList();
                    var slots = _userSlotRepo.Get(x => x.UserId == id).ToList();
                    var business = _userBusinessRepo.Get(x => x.ToUserId == id).ToList();

                    user.TotalDeposit = deposit.Sum(x => x.AmountUSD).ToString("#,###,###,###.##");
                    user.TotalWithdraw = withDraw.Sum(x => x.AmountUSD).ToString("#,###,###,###.##");
                    user.TotalSlot = slots.Sum(x => x.Quantity).ToString("#,###,###,###");
                    user.TotalToken = slots.Sum(x => x.TokenQuantity).ToString("#,###,###,###");
                    user.TotalCommision = business.Sum(x => x.AmountUSD).ToString("#,###,###,###.##");
                    user.Balance = wallet?.AmountUSD.ToString("#,###,###,###.##");
                }

            }

            // Set Cache
            _memoryCache.Set(cacheKey, user, new MemoryCacheEntryOptions
            {
                SlidingExpiration = _cacheSliding
            });

            return Task.FromResult(user);
        }

        public Task<UserModel> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = _userRepo.Get(x => x.Email == email).QueryTo<UserModel>().SingleOrDefault();

            if (user != null && !string.IsNullOrWhiteSpace(user.Permissions))
            {
                user.ListPermission = user.Permissions?.Split(',').Select(x => (Permission)int.Parse(x)).ToList();
            }

            return Task.FromResult(user);
        }

        // Create

        public async Task<string> CreateAsync(CreateUserModel model, CancellationToken cancellationToken = default)
        {
            CheckUniqueEmail(model.Email);

            var userEntity = model.MapTo<UserEntity>();

            // User Name following the Email for new account
            userEntity.Email = model.Email;

            _userRepo.Add(userEntity);

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            if (string.IsNullOrWhiteSpace(userEntity.Email))
            {
                return userEntity.Id;
            }

            // Send Confirm Email

            var authService = _serviceProvider.GetService<IAuthenticationService>();

            await authService.SendConfirmEmailAsync(userEntity.Id, cancellationToken).ConfigureAwait(true);

            return userEntity.Id;
        }

        // Update

        public async Task UpdateAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            CheckExist(model.Id);

            CheckUniqueEmail(model.Email, model.Id);

            CheckUniquePhone(model.Phone, model.Id);

            // User

            var userEntity = _userRepo.Get(x => x.Id == model.Id).Single();

            // Update Data

            userEntity.Email = model.Email;
            userEntity.Enable2FA = model.Enable2FA;
            userEntity.FullName = model.FullName;

            if (!string.Equals(userEntity.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                userEntity.Email = model.Email;

                userEntity.EmailConfirmedTime = null;

                var authService = _serviceProvider.GetService<IAuthenticationService>();

                await authService.SendConfirmEmailAsync(userEntity.Id, cancellationToken).ConfigureAwait(true);
            }

            if (!string.Equals(userEntity.Phone, model.Phone, StringComparison.OrdinalIgnoreCase))
            {
                userEntity.Phone = model.Phone;
                userEntity.PhoneConfirmedTime = null;
            }

            if (model.IsBanned)
            {
                if (userEntity.BannedTime == null)
                {
                    userEntity.BannedTime = CoreHelper.SystemTimeNow;
                    userEntity.BannedRemark = model.BannedRemark;
                }
            }
            else
            {
                userEntity.BannedTime = null;
                userEntity.BannedRemark = null;
            }

            userEntity.Permission = model.ListPermission?.Any() == true
                ? string.Join(",", model.ListPermission.Select(x => (int)x))
                : null;

            _userRepo.Update(userEntity,
                x => x.Email,
                x => x.Phone,
                x => x.Enable2FA,
                x => x.BannedRemark,
                x => x.EmailConfirmedTime,
                x => x.PhoneConfirmedTime,
                x => x.BannedTime,
                x => x.Permission,
                x => x.FullName
            );

            // Save
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + model.Id;
            _memoryCache.Remove(cacheKey);
        }

        // Delete

        public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id == LoggedInUser.Current.Id)
            {
                throw new CoreException(nameof(ErrorCode.UnAuthorized), "Cannot delete yourself.");
            }

            CheckExist(id);

            _userRepo.Delete(new UserEntity { Id = id });

            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + id;

            _memoryCache.Remove(cacheKey);

            return Task.CompletedTask;
        }

        public async Task UpdateProfileAsync(ProfileModel model, CancellationToken cancellationToken = default)
        {
            var userEntity = model.MapTo<UserEntity>();

            _userRepo.Update(userEntity, x => x.FullName, x => x.Email);

            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + model.Id;

            _memoryCache.Remove(cacheKey);
        }

        public void CheckExist(string id)
        {
            var isExist = _userRepo.Get(x => x.Id == id).Any();

            if (!isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotFound), "The User is not found.");
            }
        }

        public void CheckUniqueEmail(string email, string excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var query = _userRepo.Get(x => x.Email == email);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Email {email} is already exist, please try another.");
            }
        }

        public void CheckUniquePhone(string phone, string excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return;
            }

            var query = _userRepo.Get(x => x.Phone == phone);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Phone {phone} is already exist, please try another.");
            }
        }

        public void CheckUniqueAddress(string address, string excludeId)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return;
            }

            var query = _userWalletRepo.Get(x => x.AddressWallet == address);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query = query.Where(x => x.UserId != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Address {address} is already exist, please try another.");
            }
        }

        public void CheckUniqueTxHash(string txHash, string excludeId)
        {
            if (string.IsNullOrWhiteSpace(txHash))
            {
                return;
            }

            var query = _userDepositRepo.Get(x => x.TxHash == txHash);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Your Tx Hash is already exist, please try another.");
            }

            var query2 = _userWithdrawRepo.Get(x => x.TxHash == txHash);

            if (!string.IsNullOrWhiteSpace(excludeId))
            {
                query2 = query2.Where(x => x.Id != excludeId);
            }

            if (query2.Any())
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Your Tx Hash is already exist, please try another.");
            }

        }


        public async Task<string> RegisterAsync(RegisterUserModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Confirm Password is wrong, please try another.");
            }

            var refer = await GetByCodeAsync(model.Code, cancellationToken);
            if (refer == null)
            {
                throw new CoreException(nameof(ErrorCode.NotUnique),
                    $"Invalid invited reference user, please try another.");
            }

            CheckUniqueEmail(model.Email);

            var userEntity = model.MapTo<UserEntity>();

            // User Name following the Email for new account
            userEntity.Email = model.Email;
            userEntity.ReferenceId = refer.Id;
            userEntity.Code = StringHelper.Generate(8, false, false, true, false);
            var systemTimeNow = CoreHelper.SystemTimeNow;
            userEntity.PasswordLastUpdatedTime = systemTimeNow;
            userEntity.PasswordHash = HashPassword(model.Password, userEntity.PasswordLastUpdatedTime.Value);
            userEntity.Permission = ((int)Permission.Member).ToString();
            _userRepo.Add(userEntity);
            cancellationToken.ThrowIfCancellationRequested();

            UnitOfWork.SaveChanges();

            if (string.IsNullOrWhiteSpace(userEntity.Email))
            {
                return userEntity.Id;
            }

            // Send Confirm Email
            var authService = _serviceProvider.GetService<IAuthenticationService>();
            await authService.SendConfirmEmailAsync(userEntity.Id, cancellationToken).ConfigureAwait(true);
            return userEntity.Id;
        }

        public Task<UserModel> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            var user = _userRepo.Get(x => x.Code.ToLower() == code.ToLower()).QueryTo<UserModel>().FirstOrDefault();
            return Task.FromResult(user);
        }

        public async Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            var now = CoreHelper.SystemTimeNow;

            _temProfileRepo.DeleteWhere(x => x.UserId == model.Id, true);
            UnitOfWork.SaveChanges();

            var user = new UserTempProfileEntity()
            {
                FullName = model.FullName,
                AboutMe = model.AboutMe,
                Gender = model.Gender,
                UserId = LoggedInUser.Current.Id,
                WalletAddress = model.Address,
                Password = model.Password,
                Enable2FA = model.Enable2FA,
                ConfirmEmailToken = StringHelper.Generate(6, false, false, true, false),
                ConfirmEmailTokenExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _temProfileRepo.Add(user);
            UnitOfWork.SaveChanges();

            // send email notify update
            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.UpdateProfile, cancellationToken);
            return;
        }

        public async Task UpdateStackProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            var now = CoreHelper.SystemTimeNow;

            _temProfileRepo.DeleteWhere(x => x.UserId == model.Id, true);
            UnitOfWork.SaveChanges();

            var user = new UserTempProfileEntity()
            {
                FullName = model.FullName,
                AboutMe = model.AboutMe,
                Gender = model.Gender,
                UserId = LoggedInUser.Current.Id,
                WalletAddress = model.Address,
                Password = model.Password,
                Enable2FA = model.Enable2FA,
                ConfirmEmailToken = StringHelper.Generate(6, false, false, true, false),
                ConfirmEmailTokenExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _temProfileRepo.Add(user);
            UnitOfWork.SaveChanges();

            // send email notify update
            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.UpdateProfile, cancellationToken);
            return;
        }

        public Task ConfirmUpdateProfileAsync(string token, CancellationToken cancellationToken = default)
        {
            var temp = _temProfileRepo.Get(x => x.ConfirmEmailToken == token).FirstOrDefault();
            if (temp == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (temp.ConfirmEmailTokenExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            var user = _userRepo.Get(x => x.Id == temp.UserId).FirstOrDefault();
            if (user == null)
            {
                return Task.CompletedTask;
            }

            user.FullName = temp.FullName;
            user.Enable2FA = temp.Enable2FA;
            user.AboutMe = temp.AboutMe;
            user.Gender = temp.Gender;
            _userRepo.Update(user, x => x.AboutMe, x => x.FullName, x => x.Gender, x => x.Enable2FA);
            UnitOfWork.SaveChanges();

            var now = CoreHelper.SystemTimeNow;
            if (!string.IsNullOrWhiteSpace(temp.Password))
            {
                user.PasswordLastUpdatedTime = now;
                user.PasswordHash = HashPassword(temp.Password, now);
                _userRepo.Update(user, x => x.PasswordHash, x => x.PasswordLastUpdatedTime);
            }
            UnitOfWork.SaveChanges();

            var wallet = _userWalletRepo.Get(x => x.UserId == temp.UserId).FirstOrDefault();
            var stackwallet = _stackWalletRepository.Get(x => x.UserId == temp.UserId).FirstOrDefault();
            if(stackwallet != null)
            {
                stackwallet.WalletAddress = temp.WalletAddress;
                _stackWalletRepository.Update(stackwallet, x => x.WalletAddress);
            }
            else
            {
                _stackWalletRepository.Add(new StackingWalletEntity()
                {
                    WalletAddress = temp.WalletAddress,
                    UserId = temp.UserId,
                    Balance = 0,
                    TotalReward = 0,
                    DailyReward = 0,
                });
            }
            if (wallet != null)
            {
                wallet.AddressWallet = temp.WalletAddress;
                _userWalletRepo.Update(wallet, x => x.AddressWallet);
            }
            else
            {
                _userWalletRepo.Add(new UserWalletEntity()
                {
                    AddressWallet = temp.WalletAddress,
                    UserId = temp.UserId,
                    AmountUSD = 0,
                });
            }
            UnitOfWork.SaveChanges();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + temp.UserId;
            _memoryCache.Remove(cacheKey);


            // remove temp
            _temProfileRepo.DeleteWhere(x => x.ConfirmEmailToken == token);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<List<DetailSlotRequestModel>> GetSlot(List<string> userIdList)
        {
            var result = _userSlotRepo.Get(x => userIdList.Contains(x.UserId)).QueryTo<DetailSlotRequestModel>()
                .ToList();
            return result;
        }

        public Task<DashboardModel> GetDashBoardAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = "Dashboard_" + LoggedInUser.Current.Id;

            //// Check Cache
            _memoryCache.TryGetValue(cacheKey, out var data);
            if (data is DashboardModel result)
            {
                return Task.FromResult(result);
            }

            result = new DashboardModel();

            result.TotalSlots = _userSlotRepo.Get(x => x.UserId == LoggedInUser.Current.Id).Sum(x => x.Quantity);
            result.TotalToken = _userTokenRepo.Get(x => x.UserId == LoggedInUser.Current.Id).Sum(x => x.Quantity);
            //var a = _userTokenRepo.Get(p=>p.UserId.Equals("938a1fcc75f7415db875f991caea8e1d"));

            var sellToken = _userSellTokenRepo.Get(x => x.UserId == LoggedInUser.Current.Id).ToList();
            var sell = sellToken.Sum(x => x.TokenQuantity);

            var withdrawToken = _userWithdrawRepo.Get(x => (x.WalletId == LoggedInUser.Current.WalletId) && (x.Status != Enums.WithdrawStatus.New
            && x.Status != Enums.WithdrawStatus.Approved
            && x.Status != Enums.WithdrawStatus.Confirming
            && x.Status != Enums.WithdrawStatus.Reject
            && x.Status != Enums.WithdrawStatus.TokenReject));
            var sumWithdraw = withdrawToken.Sum(x => x.AmountUSD) + withdrawToken.Sum(x => x.FeeUSD);
            //result.TotalToken = result.TotalToken - sell;
            result.TotalToken = result.TotalToken - sumWithdraw - sell;
            result.TotalCommission = _userBusinessRepo.Get(x => x.ToUserId == LoggedInUser.Current.Id).Sum(x => x.Commission);

            // Set Cache
            _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                SlidingExpiration = _cacheSliding
            });

            return Task.FromResult(result);
        }

        public string GenerateAuthyToken(string currentId)
        {
            var user = _userRepo.Get(x => x.Id == currentId).FirstOrDefault();
            if (user != null && user.AuthyId > 0)
            {
                return "images/QR/ThankAuthy.jpg";
            }

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SystemHelper.Setting.AuthyAppSecretKey));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256");

            var header = new JwtHeader(credentials);

            var start = EpochTime.GetIntDate(DateTime.UtcNow);
            var end = EpochTime.GetIntDate(DateTime.UtcNow.AddMinutes(10));

            var payload = new JwtPayload
            {
                { "iss", SystemHelper.Setting.AuthyAppName},
                { "iat", start},
                { "exp", end},
                { "context", new
                    {
                        custom_user_id = currentId,
                        authy_app_id = SystemHelper.Setting.AuthyAppId
                    }
                },
            };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(secToken);
            var link = $"authy://account?token={token}";

            return GenerateQRCode(link);
        }

        public async Task ConfirmAuthy(string userId, CancellationToken cancellationToken = default)
        {
            var request = new FlurlRequest($"https://api.authy.com/protected/json/registrations/status?custom_user_id={userId}");
            request.Headers["X-Authy-API-Key"] = SystemHelper.Setting.AuthyAppSecretKey;

            try
            {
                var response = await request.GetJsonAsync<AuthyRegistrationResponseModel>();
                if (response.success)
                {
                    var user = _userRepo.Get(x => x.Id == userId).FirstOrDefault();
                    if (user != null)
                    {
                        user.AuthyId = response.registration.authy_id;
                        _userRepo.Update(user, x => x.AuthyId);
                        UnitOfWork.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public UpdateProfileModel GetProfileTemp(string userId)
        {
            var profile = _temProfileRepo.Get(x => x.UserId == userId).FirstOrDefault();

            if (profile == null)
            {
                return new UpdateProfileModel();
            }

            var token = profile?.Id;
            var temp = new UpdateProfileModel()
            {
                Id = profile?.Id,
                FullName = profile?.FullName,
                Address = profile?.WalletAddress,
                AboutMe = profile?.AboutMe,
                Enable2FA = profile?.Enable2FA ?? false,
                Password = profile?.Password,
                Gender = profile?.Gender ?? Enums.Gender.Other,
            };

            return temp;
        }

        public async Task ResendConfirmRegisterAsync(string id, CancellationToken cancellationToken = default)
        {
            // Send Confirm Email
            var authService = _serviceProvider.GetService<IAuthenticationService>();
            await authService.SendConfirmEmailAsync(id, cancellationToken).ConfigureAwait(true);
            return;
        }

        public async Task ResendUpdateProfileAsync(CancellationToken cancellationToken = default)
        {
            var user = _temProfileRepo.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            if (user.ConfirmEmailTokenExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }


            user.ConfirmEmailToken = StringHelper.Generate(6, false, false, true, false);
            user.ConfirmEmailTokenExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            user.EmailConfirmedTime = null;

            _temProfileRepo.Update(user, x => x.ConfirmEmailToken, x => x.ConfirmEmailTokenExpireTime, x => x.EmailConfirmedTime);
            UnitOfWork.SaveChanges();

            // send email notify update
            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.UpdateProfile, cancellationToken);
            return;
        }

        private string GenerateQRCode(string qrcodeText)
        {
            string imagePath = $"wwwroot/images/QR";
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var id = Guid.NewGuid().ToString("N");
            string barcodePath = imagePath + $"/QrCode_{id}.jpg";

            if (File.Exists(barcodePath))
            {
                File.Delete(barcodePath);
            }


            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrcodeText, QRCodeGenerator.ECCLevel.H, true, true, QRCodeGenerator.EciMode.Utf8);
            var qrCode = new QRCode(qrCodeData);

            var mitmap = qrCode.GetGraphic(10);
            mitmap.Save(barcodePath, ImageFormat.Jpeg);

            return $"images/QR/QrCode_{id}.jpg"; ;
        }

        public static string HashPassword(string password, DateTimeOffset hashTime)
        {
            var passwordSalt = hashTime.ToString("O") + SystemHelper.Setting.EncryptKey;
            var passwordHash = SecurityHelper.HashPassword(password, passwordSalt);
            return passwordHash;
        }

        public Task ConfirmUpdateStackProfileAsync(string token, CancellationToken cancellationToken = default)
        {
            var temp = _temProfileRepo.Get(x => x.ConfirmEmailToken == token).FirstOrDefault();
            if (temp == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (temp.ConfirmEmailTokenExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            var user = _userRepo.Get(x => x.Id == temp.UserId).FirstOrDefault();
            if (user == null)
            {
                return Task.CompletedTask;
            }

            user.FullName = temp.FullName;
            user.Enable2FA = temp.Enable2FA;
            user.AboutMe = temp.AboutMe;
            user.Gender = temp.Gender;
            _userRepo.Update(user, x => x.AboutMe, x => x.FullName, x => x.Gender, x => x.Enable2FA);
            UnitOfWork.SaveChanges();

            var now = CoreHelper.SystemTimeNow;
            if (!string.IsNullOrWhiteSpace(temp.Password))
            {
                user.PasswordLastUpdatedTime = now;
                user.PasswordHash = HashPassword(temp.Password, now);
                _userRepo.Update(user, x => x.PasswordHash, x => x.PasswordLastUpdatedTime);
            }
            UnitOfWork.SaveChanges();

            var wallet = _stackWalletRepository.Get(x => x.UserId == temp.UserId).FirstOrDefault();
            var otherwallet = _userWalletRepo.Get(x => x.UserId == temp.UserId).FirstOrDefault();
            if (wallet != null)
            {
                wallet.WalletAddress = temp.WalletAddress;
                _stackWalletRepository.Update(wallet, x => x.WalletAddress);
            }
            else
            {
                _stackWalletRepository.Add(new StackingWalletEntity()
                {
                    WalletAddress = temp.WalletAddress,
                    UserId = temp.UserId,
                    Balance = 0,
                    DailyReward = 0,
                    TotalReward = 0,
                });
            }


            if (otherwallet == null)
            {
                _userWalletRepo.Add(new UserWalletEntity()
                {
                    AddressWallet = temp.WalletAddress,
                    UserId = temp.UserId,
                    AmountUSD = 0,
                });
            }

            UnitOfWork.SaveChanges();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + temp.UserId;
            _memoryCache.Remove(cacheKey);


            // remove temp
            _temProfileRepo.DeleteWhere(x => x.ConfirmEmailToken == token);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<DataTableResponseModel<StackUserModel>> GetStackDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userRepo.Get();

            var listData = query.QueryTo<StackUserModel>();

            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (StackUserModel)x);
            var userIds = data.Select(x => x.Id).ToList();
            var wallets = _stackWalletRepository.Get(x => userIds.Contains(x.UserId)).ToList();
            var walletIds = wallets.Select(x => x.Id);
            var deposits = _historydepositRepository.Get(x => walletIds.Contains(x.WalletId) && x.Status == Enums.StackDeposit.Approved).ToList();
            var withDraws = _stackWithdrawRepository.Get(x => walletIds.Contains(x.WalletId) && x.Status != Enums.StackWithdraw.Reject).ToList();

            foreach (var userModel in data)
            {
                var wallet = wallets.FirstOrDefault(x => x.UserId == userModel.Id);
                if (wallet == null)
                {
                    continue;
                }

                userModel.Id = wallet.UserId;

                userModel.TotalDeposit = deposits.Where(x => x.WalletId == wallet.Id).Sum(x => x.AmountTige).ToString("#,###,###,###.##");

                var withdrawRequest = withDraws.Where(
                    x => x.WalletId == wallet.Id
                && x.Status != Enums.StackWithdraw.Reject);
                var withdraw = withdrawRequest.Sum(x => x.AmountTige);
                var fee = withdrawRequest.Sum(x => x.FeeTige);
                userModel.TotalWithdraw = withdraw.ToString("#,###,###,###.##");
                userModel.TotalFee = fee.ToString("#,###,###,###.##");

                userModel.Balance = wallet.Balance.ToString("#,###,###,###");
                userModel.OldBalance = (await _tokenService.GetTotalToken(userModel.Id)).ToString("#,###,###,###");

                userModel.WalletAddress = wallet.WalletAddress;


                userModel.TotalStack = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id).Sum(x=>x.StackAmount).ToString("#,###,###,###");

                var oldwallet = _userWalletRepo.Get(x => x.UserId == userModel.Id).FirstOrDefault();
                userModel.TotalTransfer = _userWithdrawRepo.Get(x => x.WalletId == oldwallet.Id).Where(x => x.Status==Enums.WithdrawStatus.ConfirmingTransfer || x.Status==Enums.WithdrawStatus.ApprovedTransfer).Sum(x => x.AmountUSD).ToString("#,###,###,###.##");
                //userModel.TotalReward = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && x.Status == Enums.StackState.Finished).Sum(x => x.TotalReward).ToString("#,###,###,###.##");
                userModel.TotalReward = _refundRepository.Get(x=>x.WalletId==wallet.Id).Sum(x=>x.Amount).ToString("#,###,###,###.##");
                userModel.TotalDailyReward = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && x.Status == Enums.StackState.Staking).Sum(x => x.DailyReward).ToString("#,###,###,###.##");
            }

            return result;
        }


        public async Task UpdateStackUserAsync(StackUserModel model, CancellationToken cancellationToken = default)
        {
            CheckExist(model.Id);

            CheckUniqueEmail(model.Email, model.Id);

            CheckUniquePhone(model.Phone, model.Id);

            // User

            var userEntity = _userRepo.Get(x => x.Id == model.Id).Single();

            // Update Data

            userEntity.Email = model.Email;
            userEntity.FullName = model.FullName;

            if (!string.Equals(userEntity.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                userEntity.Email = model.Email;

                userEntity.EmailConfirmedTime = null;

                var authService = _serviceProvider.GetService<IAuthenticationService>();

                await authService.SendConfirmEmailAsync(userEntity.Id, cancellationToken).ConfigureAwait(true);
            }

            if (!string.Equals(userEntity.Phone, model.Phone, StringComparison.OrdinalIgnoreCase))
            {
                userEntity.Phone = model.Phone;
                userEntity.PhoneConfirmedTime = null;
            }

            if (model.IsBanned)
            {
                if (userEntity.BannedTime == null)
                {
                    userEntity.BannedTime = CoreHelper.SystemTimeNow;
                    userEntity.BannedRemark = model.BannedRemark;
                }
            }
            else
            {
                userEntity.BannedTime = null;
                userEntity.BannedRemark = null;
            }

            userEntity.Permission = model.ListPermission?.Any() == true
                ? string.Join(",", model.ListPermission.Select(x => (int)x))
                : null;

            _userRepo.Update(userEntity,
                x => x.Email,
                x => x.Phone,
                x => x.Enable2FA,
                x => x.BannedRemark,
                x => x.EmailConfirmedTime,
                x => x.PhoneConfirmedTime,
                x => x.BannedTime,
                x => x.Permission,
                x => x.FullName
            );

            // Save
            await UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + model.Id;
            _memoryCache.Remove(cacheKey);
        }

        public Task<StackUserModel> GetStackUserByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var cacheKey = _cacheKeyPrefix + id;

            // Get Data
            var user = _userRepo.Get(x => x.Id == id).QueryTo<StackUserModel>().SingleOrDefault();

            if (user != null && !string.IsNullOrWhiteSpace(user.Permissions))
            {
                user.ListPermission = user.Permissions.Split(',').Select(x => (Permission)int.Parse(x)).ToList();
            }

            if (user != null)
            {
                var wallet = _stackWalletRepository.Get(x => x.UserId == user.Id).FirstOrDefault();
                user.Address = wallet?.WalletAddress;
                if (wallet != null)
                {
                    user.Balance = wallet.Balance > 0 ? wallet.Balance.ToString() : "0";
                }
                else
                {
                    user.Balance = "0";
                }

                if (wallet != null)
                {
                    var deposit = _historydepositRepository.Get(x => x.WalletId == wallet.Id && x.Status == Enums.StackDeposit.Approved).ToList();
                    var withDraw = _stackWithdrawRepository.Get(x => x.WalletId == wallet.Id && x.Status != Enums.StackWithdraw.Reject).ToList();
                    var transfer = _transferRepository.Get(x => x.WalletId == wallet.Id && x.Status != Enums.StackTransfer.Reject).ToList();
                    var stack = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && x.Status != Enums.StackState.New).ToList();

                    user.TotalDeposit = deposit.Sum(x => x.AmountTige).ToString("#,###,###,###.##");
                    user.TotalWithdraw = withDraw.Sum(x => x.AmountTige).ToString("#,###,###,###.##");
                    user.TotalFee = withDraw.Sum(x => x.FeeTige).ToString("#,###,###,###.##");
                    user.TotalTransfer = transfer.Sum(x => x.AmountTige).ToString("#,###,###,###");
                    user.TotalReward = stack.Sum(x => x.TotalReward).ToString("#,###,###,###");
                    user.TotalDailyReward = stack.Sum(x => x.DailyReward).ToString("#,###,###,###");
                }
            }
            return Task.FromResult(user);
        }
    }
}