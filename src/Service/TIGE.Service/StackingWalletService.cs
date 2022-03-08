using Elect.Core.StringUtils;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IStackingWalletService))]
    public class StackingWalletService : Base.Service, IStackingWalletService
    {
        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKeyPrefix = "User_";

        private readonly TimeSpan _cacheSliding = TimeSpan.FromHours(4);

        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        private readonly ICommonService _commonService;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<UserWalletEntity> _walletRepository;
        private readonly IRepository<UserWithDrawRequestEntity> _userWithdrawRepository;
        private readonly IRepository<StackingWalletEntity> _stackingWalletRepository;
        private readonly IRepository<HistoryDepositEntity> _stackingDepositRepository;
        private readonly IRepository<HistoryWithdrawTokenEntity> _stackingWithdrawRepository;
        private readonly IRepository<StackingConfigEntity> _stackingConfigRepository;
        private readonly IRepository<StackHistoryEntity> _stackHistoryRepository;
        private readonly IRepository<HistoryRefundEntity> _refundRepository;
        private readonly IRepository<ConfigurationEntity> _configRepo;
        private readonly IRepository<UserTokensEntity> _userTokensRepo;
        private readonly IRepository<HistoryWithdrawUSDEntity> _stackingWithdrawUSDRepository;

        public StackingWalletService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMemoryCache memoryCache, IEmailService emailService, ICommonService commonService, ITokenService tokenService) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _emailService = emailService;
            _memoryCache = memoryCache;
            _tokenService = tokenService;

            _commonService = commonService;
            _userRepository = unitOfWork.GetRepository<UserEntity>();
            _walletRepository = unitOfWork.GetRepository<UserWalletEntity>();
            _userWithdrawRepository = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
            _stackingWalletRepository = unitOfWork.GetRepository<StackingWalletEntity>();
            _stackingDepositRepository = unitOfWork.GetRepository<HistoryDepositEntity>();
            _stackingWithdrawRepository = unitOfWork.GetRepository<HistoryWithdrawTokenEntity>();
            _stackingConfigRepository = unitOfWork.GetRepository<StackingConfigEntity>();
            _refundRepository = unitOfWork.GetRepository<HistoryRefundEntity>();
            _configRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userTokensRepo = unitOfWork.GetRepository<UserTokensEntity>();
            _stackHistoryRepository = unitOfWork.GetRepository<StackHistoryEntity>();
            _stackingWithdrawUSDRepository = unitOfWork.GetRepository<HistoryWithdrawUSDEntity>();
        }

        public Task CheckConfirmDepositWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _stackingDepositRepository.Get(x => x.ConfirmToken == token).FirstOrDefault();

            if (request == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (request.ExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            request.ConfirmToken = null;
            request.ExpireTime = null;
            request.ConfirmedTime = CoreHelper.SystemTimeNow;
            request.Status = Enums.StackDeposit.Confirming;
            _stackingDepositRepository.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status, x => x.ExpireTime);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public StackDepositModel GetByLatestDepositAsync(string userId)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == userId).FirstOrDefault();

            var depositRequestEntity = _stackingDepositRepository.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<StackDepositModel>().FirstOrDefault();

            return depositRequestEntity;
        }

        public StackWithdrawModel GetByLatestWithDrawAsync(string userId)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == userId).FirstOrDefault();

            if (wallet == null)
            {
                return null;
            }
            var withdrawRequestEntity = _stackingWithdrawRepository.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<StackWithdrawModel>().FirstOrDefault();

            return withdrawRequestEntity;
        }

        public async Task<StackingWalletEntity> GetMyWalletAsync(string id, CancellationToken cancellationToken = default)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == id).FirstOrDefault();

            if (wallet != null)
            {
                await _commonService.UpdateStackWalletBalance(wallet.Id);
            }

            wallet = _stackingWalletRepository.Get(x => x.UserId == id).FirstOrDefault();

            if (wallet != null)
            {
                var history = _stackingDepositRepository.Get(x => x.WalletId == wallet.Id)
                    .OrderByDescending(x => x.CreatedTime).Take(30)
                    .ToList();

                wallet.Deposits = history;
            }

            return await Task.FromResult(wallet);
        }

        public Task<List<StackWithdrawModel>> GetMyWithdrawAsync(string currentId, CancellationToken cancellationToken = default)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == currentId).FirstOrDefault();
            if(wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            var history = _stackingWithdrawRepository.Get(x => x.WalletId == wallet.Id)
                .QueryTo<StackWithdrawModel>().ToList();

            return Task.FromResult(history);
        }

        public async Task ResendConfirmDeposit(string data, CancellationToken cancellationToken = default)
        {
            var history = _stackingDepositRepository.Get(x => x.Id == data).FirstOrDefault();
            if (history == null)
            {
                return;
            }

            if (history.ExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }


            history.ConfirmToken = StringHelper.Generate(6, false, false, true, false);
            history.ConfirmedTime = null;
            history.ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            _stackingDepositRepository.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();

            await _emailService.SendVerifyDepositStackAsync(data, cancellationToken).ConfigureAwait(true);
            return;
        }

        public async Task<string> SubmitDepositAsync(CreateDepositStackingRequestModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(model.TxHash))
            {
                throw new CoreException(ErrorCode.BadRequest, "Please input Txhash");
            }

            //if (model.Amount <= 0)
            //{
            //    throw new CoreException(ErrorCode.BadRequest, "Invalid deposit amount");
            //}

            var walletid = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault().Id;

            var deposit = new HistoryDepositEntity()
            {
                AmountTige = model.Amount,
                Status = Enums.StackDeposit.New,
                TxHash = model.TxHash,
                //Rate = SystemHelper.GetETHRate(),
                Rate = 1,
                WalletId = walletid,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _stackingDepositRepository.Add(deposit);
            UnitOfWork.SaveChanges();

            await _commonService.UpdateStackWalletBalance(walletid);
            //send email
            await _emailService.SendVerifyDepositStackAsync(deposit.Id, cancellationToken).ConfigureAwait(true);

            return deposit.Id;
        }

        public async Task<string> SubmitWithdrawAsync(CreateWithdrawStackRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _stackingConfigRepository.Get().FirstOrDefault();
            //if (model.TxHash == null)
            //{
            //    throw new CoreException(ErrorCode.BadRequest, $"Please input TxHash");
            //}

            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input Wallet Address");
            }

            if (config != null && config.MinWithDraw > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.MinWithDraw} TIGE");
            }
            var wallet = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();

            if(wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"404 error");
            }

            if (model.Amount > wallet.Balance)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
            }

            var withdraw = new HistoryWithdrawTokenEntity()
            {
                AmountTige = model.Amount,
                WalletId = wallet.Id,
                ToWalletAddress = model.WalletAddress,
                Status = Enums.StackWithdraw.New,
                Rate = SystemHelper.GetETHRate(),
                TxHash = model.TxHash,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _stackingWithdrawRepository.Add(withdraw);            
            UnitOfWork.SaveChanges();

            await _commonService.UpdateStackWalletBalance(wallet.Id);

            //send email
            await _emailService.SendVerifyWithdrawStackAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            return withdraw.Id;
        }

        public async Task ResendConfirmWithdraw(string data, CancellationToken cancellationToken = default)
        {
            var history = _stackingWithdrawRepository.Get(x => x.Id == data).FirstOrDefault();
            if (history == null)
            {
                return;
            }

            if (history.ExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }

            history.ConfirmToken = StringHelper.Generate(6, false, false, true, false);
            history.ConfirmedTime = null;
            history.ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            _stackingWithdrawRepository.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();


            //send email
            await _emailService.SendVerifyWithdrawStackAsync(data, cancellationToken).ConfigureAwait(true);
        }

        public Task CheckConfirmWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _stackingWithdrawRepository.Get(x => x.ConfirmToken == token).FirstOrDefault();
            if (request == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (request.ExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            request.ConfirmToken = null;
            request.ConfirmedTime = CoreHelper.SystemTimeNow;
            request.Status = Enums.StackWithdraw.Confirming;
            _stackingWithdrawRepository.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task DeleteWithdrawAsync(string id, CancellationToken cancellationToken = default)
        {
            var history = _stackingWithdrawRepository.Get(x => x.Id == id).Include(x => x.StackingWallet).Select(x => x.StackingWallet).FirstOrDefault();

            _stackingWithdrawRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            if (history != null)
            {
                await _commonService.UpdateStackWalletBalance(history.Id, cancellationToken);
            }
        }

        public Task<List<WithdrawRequestModel>> GetTranferRequest(string userId, CancellationToken cancellationToken = default)
        {
            var wallet = _walletRepository.Get(x => x.UserId == userId).FirstOrDefault();
            var history = _userWithdrawRepository.Get(x => x.WalletId == wallet.Id)
                .Where(x => x.Status == Enums.WithdrawStatus.NewTransfer
                || x.Status == Enums.WithdrawStatus.ConfirmingTransfer
                || x.Status == Enums.WithdrawStatus.ApprovedTransfer
                || x.Status == Enums.WithdrawStatus.RejectTransfer)
                .QueryTo<WithdrawRequestModel>().ToList();

            return Task.FromResult(history);
        }

        public async Task<string> SubmitTransferAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepo.Get().FirstOrDefault();
            var config2 = _stackingConfigRepository.Get().FirstOrDefault();
            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }

            if (config != null && config2.MinTransfer > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config2.MinTransfer} TIGE");
            }

            var totaltoken = await _tokenService.GetTotalToken(LoggedInUser.Current.Id);

            if (model.Amount > totaltoken)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
            }

            var withdraw = new UserWithDrawRequestEntity()
            {
                AmountUSD = model.Amount,
                WalletId = LoggedInUser.Current.WalletId,
                FromWalletId = model.WalletAddress,
                Status = Enums.WithdrawStatus.NewTransfer,
                Rate = SystemHelper.GetETHRate(),
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _userWithdrawRepository.Add(withdraw);
            UnitOfWork.SaveChanges();

            var wallet = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();

            await _commonService.UpdateStackWalletBalance(wallet.Id, cancellationToken);

            //send email
            await _emailService.SendVerifyTransferTigeAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            return withdraw.Id;
        }
        public async Task<StackDepositModel> GetByDepositByIdAsync(string id, CancellationToken cancellationToken)
        {
            var history = _stackingDepositRepository.Get(x => x.Id == id).QueryTo<StackDepositModel>().FirstOrDefault();
            return history;
        }

        public Task<DataTableResponseModel<StackDepositModel>> GetDepositDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _stackingDepositRepository.Get();
            var listData = query.QueryTo<StackDepositModel>();
            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (StackDepositModel)x);

            foreach(var item in data)
            {
                var wallet = _stackingWalletRepository.Get(x => x.Id == item.WalletId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == wallet.UserId).FirstOrDefault();
                item.UserId = user.Code;
                item.Email = user.Email;
            }

            return Task.FromResult(result);
        }
        public Task<DataTableResponseModel<StackWithdrawModel>> GetWithdrawDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var listData = _stackingWithdrawRepository.Get().QueryTo<StackWithdrawModel>();
            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (StackWithdrawModel)x);

            foreach (var item in data)
            {
                var wallet = _stackingWalletRepository.Get(x => x.Id == item.WalletId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == wallet.UserId).FirstOrDefault();
                item.UserId = user.Code;
                item.Email = user.Email;
            }

            return Task.FromResult(result);
        }
        public Task<DataTableResponseModel<TransferRequestModel>> GetTransferDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userWithdrawRepository.Get(x => x.Status == Enums.WithdrawStatus.NewTransfer
            || x.Status == Enums.WithdrawStatus.ConfirmingTransfer
            || x.Status == Enums.WithdrawStatus.ApprovedTransfer
            || x.Status == Enums.WithdrawStatus.RejectTransfer);
            var listData = query.QueryTo<TransferRequestModel>();
            var result = listData.GetDataTableResponse(model);

            //var data = result.Data.Select(x => (TransferRequestModel)x);

            //foreach (var item in data)
            //{
            //    var user = _userRepository.Get(x => x.Id == item.UserId).FirstOrDefault();
            //    item.UserEmail = user.Email;
            //}

            return Task.FromResult(result);
        }

        public async Task UpdateDepositAsync(StackDepositModel model, CancellationToken cancellationToken = default)
        {
            var deposit = _stackingDepositRepository.Get(x => x.Id == model.Id).FirstOrDefault();
            if (deposit != null)
            {
                deposit.AmountTige = model.AmountTige;
                deposit.Rate = model.Rate;
                deposit.WalletId = model.WalletId;
                deposit.Status = model.Status;
                deposit.TxHash = model.TxHash;
                if (model.Status == Enums.StackDeposit.Approved)
                {
                    deposit.ApproveBy = LoggedInUser.Current.Id;
                    _stackingDepositRepository.Update(deposit,
                        x => x.AmountTige,
                        x => x.WalletId,
                        x => x.Rate,
                        x => x.Status,
                        x => x.TxHash,
                        x => x.ApproveBy);
                }
                else
                {
                    _stackingDepositRepository.Update(deposit,
                        x => x.AmountTige,
                        x => x.WalletId,
                        x => x.Rate,
                        x => x.TxHash,
                        x => x.Status);
                }

                UnitOfWork.SaveChanges();
            }

            // load balance
            if (deposit != null)
            {
                var wallet = _stackingWalletRepository.Get(x => x.Id == deposit.WalletId).FirstOrDefault();
                if (wallet != null) await _commonService.UpdateStackWalletBalance(wallet.Id, cancellationToken);
            }
        }

        public async Task UpdateTransferAsync(WithdrawRequestModel model, CancellationToken cancellationToken)
        {
            var withDraw = _userWithdrawRepository.Get(x => x.Id == model.Id).FirstOrDefault();
            if (withDraw != null)
            {
                withDraw.AmountETH = model.AmountETH;
                withDraw.Rate = model.Rate;
                withDraw.TxHash = model.TxHash;
                withDraw.FeeUSD = model.FeeUSD;
                //withDraw.AmountUSD = model.AmountETH * model.Rate;
                withDraw.AmountUSD = model.AmountUSD;
                withDraw.Status = model.Status;
                withDraw.FromWalletId = model.FromWalletId;
                if (model.Status == Enums.WithdrawStatus.Approved || model.Status == Enums.WithdrawStatus.TokenApproved)
                {
                    withDraw.ApproveById = LoggedInUser.Current.Id;
                    _userWithdrawRepository.Update(withDraw,
                        x => x.AmountUSD,
                        x => x.AmountETH,
                        x => x.Rate,
                        x => x.FeeUSD,
                        x => x.TxHash,
                        x => x.Status,
                        x => x.ApproveById,
                        x => x.FromWalletId
                        );
                }
                else
                {
                    _userWithdrawRepository.Update(withDraw,
                        x => x.AmountUSD,
                        x => x.AmountETH,
                        x => x.Rate,
                        x => x.TxHash,
                        x => x.Status,
                        x => x.FromWalletId
                        );
                }

                UnitOfWork.SaveChanges();
            }

            // load balance
            if (withDraw != null) await _commonService.UpdateWalletBalance(LoggedInUser.Current.Id, cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var history = _stackingDepositRepository.Get(x => x.Id == id).Include(x => x.StackingWallet).Select(x => x.StackingWallet).FirstOrDefault();

            _stackingDepositRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            if (history != null) await _commonService.UpdateStackWalletBalance(history.Id, cancellationToken);
        }

        public async Task DeleteTransferAsync(string id, CancellationToken cancellationToken)
        {
            var history = _userWithdrawRepository.Get(x => x.Id == id).Include(x => x.Wallet).Select(x => x.Wallet).FirstOrDefault();

            _userWithdrawRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            var wallet = _stackingWalletRepository.Get(x=>x.UserId==history.UserId).FirstOrDefault();
            if (history != null) await _commonService.UpdateWalletBalance(wallet.UserId, cancellationToken);
        }

        public Task<StackWithdrawModel> GetByWithdrawByIdAsync(string id, CancellationToken cancellationToken)
        {
            var history = _stackingWithdrawRepository.Get(x => x.Id == id).QueryTo<StackWithdrawModel>().FirstOrDefault();
            return Task.FromResult(history);
        }

        public Task<WithdrawRequestModel> GetByTransferByIdAsync(string id, CancellationToken cancellationToken)
        {
            var history = _userWithdrawRepository.Get(x => x.Id == id).QueryTo<WithdrawRequestModel>().FirstOrDefault();
            return Task.FromResult(history);
        }

        public async Task UpdateWithdrawAsync(StackWithdrawModel model, CancellationToken cancellationToken)
        {
            var withdraw = _stackingWithdrawRepository.Get(x => x.Id == model.Id).FirstOrDefault();
            if (withdraw != null)
            {
                withdraw.AmountTige = model.AmountTige;
                withdraw.TxHash = model.TxHash;
                withdraw.FeeTige = model.FeeTige;
                withdraw.Status = model.Status;
                withdraw.Rate = model.Rate;
                if (model.Status == Enums.StackWithdraw.Approved)
                {
                    withdraw.ApproveBy = LoggedInUser.Current.Id;
                    _stackingWithdrawRepository.Update(withdraw,
                        x => x.AmountTige,
                        x => x.TxHash,
                        x => x.FeeTige,
                        x => x.Rate,
                        x => x.Status,
                        x => x.ApproveBy);
                }
                else
                {
                    _stackingWithdrawRepository.Update(withdraw,
                        x => x.AmountTige,
                        x => x.TxHash,
                        x => x.FeeTige,
                        x => x.Rate,
                        x => x.Status);
                }
                UnitOfWork.SaveChanges();

                await _commonService.UpdateStackWalletBalance(withdraw.WalletId, cancellationToken);
            }
        }

        public Task<StackDashboardModel> GetDashBoardModel(CancellationToken cancellationToken)
        {
            var cacheKey = "StackDashboard_" + LoggedInUser.Current.Id;

            //// Check Cache
            _memoryCache.TryGetValue(cacheKey, out var data);
            if (data is StackDashboardModel result)
            {
                return Task.FromResult(result);
            }

            var wallet = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();
            if(wallet != null)
            {
                //var totalreward = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && x.Status == Enums.StackState.Finished).Sum(x => x.TotalReward);
                var totalreward = _refundRepository.Get(x => x.WalletId == wallet.Id).Sum(x => x.Amount);
                var dailyreward = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && x.Status == Enums.StackState.Staking).Sum(x => x.DailyReward);
                var totalstack = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id && (x.Status == Enums.StackState.Staking || x.Status == Enums.StackState.New)).Sum(x=>x.StackAmount);

                result = new StackDashboardModel()
                {
                    WalletAddress = wallet.WalletAddress,
                    Balance = wallet.Balance,
                    DailyReward = dailyreward,
                    TotalReward = totalreward,
                    TotalStack = totalstack,
                };

                // Set Cache
                _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = _cacheSliding
                });

                return Task.FromResult(result);
            }

            result = new StackDashboardModel();

            //// Set Cache
            //_memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
            //{
            //    SlidingExpiration = _cacheSliding
            //});

            return Task.FromResult(result);
        }

        public Task CreateWalletAsync(string id, CancellationToken cancellationToken = default)
        {
            var wallet = _walletRepository.Get(x=>x.UserId == id).FirstOrDefault();
            if(wallet == null)
            {
                //throw new CoreException(ErrorCode.BadRequest, $"Please update your profile");
                return Task.CompletedTask;
            }

            var newwallet = _stackingWalletRepository.Add(new StackingWalletEntity()
            {
                UserId = id,
                WalletAddress = wallet.AddressWallet,
                Balance = 0,
                DailyReward = 0,
                TotalReward = 0,
            });

            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<string> SubmitWithdrawUSDAsync(CreateWithdrawStackRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _stackingConfigRepository.Get().FirstOrDefault();
            //if (model.TxHash == null)
            //{
            //    throw new CoreException(ErrorCode.BadRequest, $"Please input TxHash");
            //}

            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input Wallet Address");
            }

            if (config != null && config.MinWithDrawUSD > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.MinWithDrawUSD} TIGE");
            }
            var wallet = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();

            if (wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"404 error");
            }

            if (model.Amount > wallet.Balance)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
            }

            var amount = await _commonService.CalculatingTokenPrice(model.Amount);

            var withdraw = new HistoryWithdrawUSDEntity()
            {
                AmountTige = amount.Quantity,
                TigePrice = amount.UnitPrice,
                ConvertFeeUSD = amount.FeeAmount,
                AmountUSD = amount.TotalAmount,
                WalletId = wallet.Id,
                ToWalletAddress = model.WalletAddress,
                Status = Enums.StackWithdrawUSD.New,
                Rate = SystemHelper.GetETHRate(),
                //TxHash = model.TxHash,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _stackingWithdrawUSDRepository.Add(withdraw);
            UnitOfWork.SaveChanges();

            await _commonService.UpdateStackWalletBalance(wallet.Id);

            //send email
            await _emailService.SendVerifyWithdrawUSDStackAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            return withdraw.Id;
        }

        public Task CheckConfirmUSDWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _stackingWithdrawUSDRepository.Get(x => x.ConfirmToken == token).FirstOrDefault();
            if (request == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (request.ExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            request.ConfirmToken = null;
            request.ConfirmedTime = CoreHelper.SystemTimeNow;
            request.Status = Enums.StackWithdrawUSD.Confirming;
            _stackingWithdrawUSDRepository.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task ResendConfirmWithdrawUSD(string data, CancellationToken cancellationToken = default)
        {
            var history = _stackingWithdrawUSDRepository.Get(x => x.Id == data).FirstOrDefault();
            if (history == null)
            {
                return;
            }

            if (history.ExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }

            history.ConfirmToken = StringHelper.Generate(6, false, false, true, false);
            history.ConfirmedTime = null;
            history.ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            _stackingWithdrawUSDRepository.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();


            //send email
            await _emailService.SendVerifyWithdrawUSDStackAsync(data, cancellationToken).ConfigureAwait(true);
        }

        public Task<List<StackWithdrawUSDModel>> GetMyWithdrawUSDAsync(string currentId, CancellationToken cancellationToken = default)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == currentId).FirstOrDefault();
            if (wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            var history = _stackingWithdrawUSDRepository.Get(x => x.WalletId == wallet.Id)
                .QueryTo<StackWithdrawUSDModel>().ToList();

            return Task.FromResult(history);
        }

        public StackWithdrawUSDModel GetByLatestWithDrawUSDAsync(string userId)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == userId).FirstOrDefault();

            if (wallet == null)
            {
                return null;
            }
            var withdrawRequestEntity = _stackingWithdrawUSDRepository.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<StackWithdrawUSDModel>().FirstOrDefault();

            return withdrawRequestEntity;
        }

        public async Task DeleteWithdrawUSDAsync(string id, CancellationToken cancellationToken)
        {
            var history = _stackingWithdrawUSDRepository.Get(x => x.Id == id).Include(x => x.StackingWallet).Select(x => x.StackingWallet).FirstOrDefault();

            _stackingWithdrawUSDRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            if (history != null) await _commonService.UpdateStackWalletBalance(history.Id, cancellationToken);
        }

        public Task<DataTableResponseModel<StackWithdrawUSDModel>> GetWithdrawUSDDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var listData = _stackingWithdrawUSDRepository.Get().QueryTo<StackWithdrawUSDModel>();
            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (StackWithdrawUSDModel)x);

            foreach (var item in data)
            {
                var wallet = _stackingWalletRepository.Get(x => x.Id == item.WalletId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == wallet.UserId).FirstOrDefault();
                item.UserId = user.Code;
                item.Email = user.Email;
            }

            return Task.FromResult(result);
        }

        public Task<StackWithdrawUSDModel> GetConvertByIdAsync(string id, CancellationToken cancellationToken)
        {
            var history = _stackingWithdrawUSDRepository.Get(x => x.Id == id).QueryTo<StackWithdrawUSDModel>().FirstOrDefault();
            return Task.FromResult(history);
        }

        public async Task UpdateWithdrawUSDAsync(StackWithdrawUSDModel model, CancellationToken cancellationToken)
        {
            var withdraw = _stackingWithdrawUSDRepository.Get(x => x.Id == model.Id).FirstOrDefault();
            if (withdraw != null)
            {
                withdraw.AmountTige = model.AmountTige;
                withdraw.TxHash = model.TxHash;
                withdraw.FeeUSD = model.FeeUSD;
                withdraw.TigePrice = model.TigePrice;
                withdraw.ConvertFeeUSD = model.ConvertFeeUSD;
                withdraw.AmountUSD = model.AmountUSD;
                withdraw.Status = model.Status;
                withdraw.Rate = model.Rate;
                if (model.Status == Enums.StackWithdrawUSD.Approved)
                {
                    withdraw.ApproveBy = LoggedInUser.Current.Id;
                    _stackingWithdrawUSDRepository.Update(withdraw,
                        x => x.AmountTige,
                        x => x.AmountUSD,
                        x => x.TxHash,
                        x => x.FeeUSD,
                        x => x.ConvertFeeUSD,
                        x => x.TigePrice,
                        x => x.Rate,
                        x => x.Status,
                        x => x.ApproveBy);
                }
                else
                {
                    _stackingWithdrawUSDRepository.Update(withdraw,
                        x => x.AmountTige,
                        x => x.AmountUSD,
                        x => x.TxHash,
                        x => x.FeeUSD,
                        x => x.ConvertFeeUSD,
                        x => x.TigePrice,
                        x => x.Rate,
                        x => x.Status);
                }
                UnitOfWork.SaveChanges();

                await _commonService.UpdateStackWalletBalance(withdraw.WalletId, cancellationToken);
            }
        }
    }
}
