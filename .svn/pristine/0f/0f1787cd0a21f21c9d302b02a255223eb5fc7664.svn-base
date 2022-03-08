using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.DateTimeUtils;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Elect.Data.IO;
using Elect.Data.IO.FileUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Data.IO.ImageUtils.CompressUtils;
using Elect.Data.IO.ImageUtils.ResizeUtils;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IWalletService))]
    public class WalletService : Base.Service, IWalletService
    {
        private readonly IRepository<UserWalletEntity> _userWalletRepo;
        private readonly IRepository<UserSlotsEntity> _userSlotRepo;
        private readonly IRepository<ConfigurationEntity> _configRepo;
        private readonly IRepository<StackingConfigEntity> _stackConfigRepo;
        private readonly IRepository<UserDepositRequestEntity> _userDepositRepo;
        private readonly IRepository<UserWithDrawRequestEntity> _userWithdrawHistoryRepo;
        private readonly IRepository<UserSellTokenEntity> _userSellTokenRepo;
        private readonly IRepository<TigeHistoryEntity> _tigeHistory;
        private readonly IRepository<UserTokensEntity> _userTokensRepo;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;
        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKeyPrefix = "User_";

        public WalletService(IUnitOfWork unitOfWork, IEmailService emailService, IMemoryCache memoryCache, ICommonService commonService) : base(unitOfWork)
        {
            this._emailService = emailService;
            _memoryCache = memoryCache;
            _commonService = commonService;

            _stackConfigRepo = unitOfWork.GetRepository<StackingConfigEntity>();
            _userWalletRepo = unitOfWork.GetRepository<UserWalletEntity>();
            _configRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userDepositRepo = unitOfWork.GetRepository<UserDepositRequestEntity>();
            _userWithdrawHistoryRepo = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
            _userSlotRepo = unitOfWork.GetRepository<UserSlotsEntity>();
            _userSellTokenRepo = unitOfWork.GetRepository<UserSellTokenEntity>();
            _userTokensRepo = unitOfWork.GetRepository<UserTokensEntity>();
            _tigeHistory = unitOfWork.GetRepository<TigeHistoryEntity>();
        }


        public async Task<WalletModel> GetMyWalletAsync(string userid, CancellationToken cancellationToken = default)
        {
            var wallet = _userWalletRepo.Get(x => x.UserId == userid).QueryTo<WalletModel>().FirstOrDefault();

            if(wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please update your profile");
            }

            var history = _userDepositRepo.Get(x => x.WalletId == wallet.Id).QueryTo<WalletDepositModel>()
                    .OrderByDescending(x => x.CreatedTime).Take(30)
                    .ToList();

            wallet.Deposits = history;

            return wallet;
        }
        public async Task<string> SubmitWithdrawAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepo.Get().FirstOrDefault();
            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input wallet address");
            }

            if (config != null && config.MinWithdraw > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.MinWithdraw} USD");
            }


            if (model.Amount > LoggedInUser.Current.WalletBalance)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
            }

            var withdraw = new UserWithDrawRequestEntity()
            {                
                AmountUSD = model.Amount,
                WalletId = LoggedInUser.Current.WalletId,
                FromWalletId = model.WalletAddress,
                Status = Enums.WithdrawStatus.New,
                Rate = SystemHelper.GetETHRate(),
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _userWithdrawHistoryRepo.Add(withdraw);
            UnitOfWork.SaveChanges();

            //send email
            await _emailService.SendVerifyWithdrawAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            // update balance
            await _commonService.UpdateWalletBalance(LoggedInUser.Current.Id, cancellationToken);

            return withdraw.Id;
        }

        public async Task<string> SubmitTransferAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _stackConfigRepo.Get().FirstOrDefault();
            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"You dont have wallet address");
            }

            if (config != null && config.MinTransfer > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.MinTransfer} TIGE");
            }


            if (model.Amount > LoggedInUser.Current.WalletBalance)
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

            _userWithdrawHistoryRepo.Add(withdraw);
            UnitOfWork.SaveChanges();

            // update balance
            await _commonService.UpdateStackWalletBalance(withdraw.WalletId, cancellationToken);

            //send email
            await _emailService.SendVerifyTransferTigeAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            return withdraw.Id;
        }

        //fix
        public async Task<string> SubmitWithdrawTigeAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepo.Get().FirstOrDefault();
            if (model.WalletAddress == null)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input wallet address");
            }            

            if (config != null && Convert.ToDouble(config.SalePhone2) > model.Amount)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.SalePhone2} TIGE");
            }

            var allToken = _userTokensRepo.Get(x => x.UserId == LoggedInUser.Current.Id).Sum(x => x.Quantity);
            var allTokenRequest = _userWithdrawHistoryRepo.Get(x => x.WalletId == LoggedInUser.Current.WalletId)
                .Where(x => x.Status == Enums.WithdrawStatus.TokenNew
                || x.Status == Enums.WithdrawStatus.TokenConfirming
                || x.Status == Enums.WithdrawStatus.TokenApproved).Sum(x=>x.AmountUSD);
            var currentToken = allToken - allTokenRequest;

            if (model.Amount > currentToken)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
            }

            var withdraw = new UserWithDrawRequestEntity()
            {
                AmountUSD = model.Amount,
                WalletId = LoggedInUser.Current.WalletId,
                FromWalletId = model.WalletAddress,
                Status = Enums.WithdrawStatus.TokenNew,
                Rate = SystemHelper.GetETHRate(),
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _userWithdrawHistoryRepo.Add(withdraw);
            UnitOfWork.SaveChanges();

            //send email
            await _emailService.SendVerifyWithdrawTigeAsync(withdraw.Id, cancellationToken).ConfigureAwait(true);

            // update balance
            await _commonService.UpdateWalletBalance(LoggedInUser.Current.Id, cancellationToken);

            return withdraw.Id;
        }
                
        public async Task<string> SubmitDepositAsync(CreateDepositRequestModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(model.TxHash))
            {
                throw new CoreException(ErrorCode.BadRequest, "Please input Txhash");
            }

            var wallet = new UserDepositRequestEntity()
            {
                Status = Enums.WalletDepositStatus.Pending,
                TxHash = model.TxHash,
                Rate = SystemHelper.GetETHRate(),
                WalletId = LoggedInUser.Current.WalletId,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _userDepositRepo.Add(wallet);
            UnitOfWork.SaveChanges();


            //send email
            await _emailService.SendVerifyDepositAsync(wallet.Id, cancellationToken).ConfigureAwait(true);
            return wallet.Id;
        }

        public async Task<WalletDepositModel> GetByDepositByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var history = _userDepositRepo.Get(x => x.Id == id).QueryTo<WalletDepositModel>().FirstOrDefault();
            return history;
        }

        public async Task UpdateDepositAsync(WalletDepositModel model, CancellationToken cancellationToken = default)
        {
            var deposit = _userDepositRepo.Get(x => x.Id == model.Id).FirstOrDefault();
            if (deposit != null)
            {
                deposit.AmountETH = model.AmountETH;
                deposit.Rate = model.Rate;
                deposit.AmountUSD = model.AmountETH * model.Rate;
                deposit.Status = model.Status;
                if (model.Status == Enums.WalletDepositStatus.Approved)
                {
                    deposit.ApproveBy = LoggedInUser.Current.Id;
                    _userDepositRepo.Update(deposit,
                        x => x.AmountUSD,
                        x => x.AmountETH,
                        x => x.Rate,
                        x => x.Status,
                        x => x.ApproveBy);
                }
                else
                {
                    _userDepositRepo.Update(deposit,
                        x => x.AmountUSD,
                        x => x.AmountETH,
                        x => x.Rate,
                        x => x.Status);
                }

                UnitOfWork.SaveChanges();
            }

            // load balance
            if (deposit != null)
            {
                var wallet = _userWalletRepo.Get(x => x.Id == deposit.WalletId).FirstOrDefault();
                if (wallet != null) await _commonService.UpdateWalletBalance(wallet.UserId, cancellationToken);
            }
        }

        public Task<DataTableResponseModel<WalletDepositModel>> GetDepositDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userDepositRepo.Get();
            var listData = query.QueryTo<WalletDepositModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var history = _userDepositRepo.Get(x => x.Id == id).Include(x=>x.Wallet).Select(x=>x.Wallet).FirstOrDefault();

            _userDepositRepo.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            if (history != null) await _commonService.UpdateWalletBalance(history.UserId, cancellationToken);
        }

        public Task CheckConfirmWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _userWithdrawHistoryRepo.Get(x => x.ConfirmToken == token).FirstOrDefault();
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
            request.Status = Enums.WithdrawStatus.Confirming;
            _userWithdrawHistoryRepo.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        //fix
        public Task CheckConfirmTigeWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _userWithdrawHistoryRepo.Get(x => x.ConfirmToken == token).FirstOrDefault();
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
            request.Status = Enums.WithdrawStatus.TokenConfirming;
            _userWithdrawHistoryRepo.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task CheckConfirmTransferWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _userWithdrawHistoryRepo.Get(x => x.ConfirmToken == token).FirstOrDefault();
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
            request.Status = Enums.WithdrawStatus.ConfirmingTransfer;
            _userWithdrawHistoryRepo.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        //fix
        public Task<List<WithdrawRequestModel>> GetMyWithdrawAsync(string userId, CancellationToken cancellationToken = default)
        {
            var wallet = _userWalletRepo.Get(x => x.UserId == userId).FirstOrDefault();
            var history = _userWithdrawHistoryRepo.Get(x => x.WalletId == wallet.Id)
                //.Where(x=>x.Status!=Enums.WithdrawStatus.TokenApproved
                //&& x.Status != Enums.WithdrawStatus.TokenConfirming
                //&& x.Status != Enums.WithdrawStatus.TokenNew
                //&& x.Status != Enums.WithdrawStatus.TokenReject)
                .QueryTo<WithdrawRequestModel>().ToList();

            return Task.FromResult(history);
        }

        public async Task DeleteWithdrawAsync(string id, CancellationToken cancellationToken = default)
        {
            var history = _userWithdrawHistoryRepo.Get(x => x.Id == id).Include(x => x.Wallet).Select(x => x.Wallet).FirstOrDefault();

            _userWithdrawHistoryRepo.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();


            // load balance
            if (history != null) await _commonService.UpdateWalletBalance(history.UserId, cancellationToken);
        }

        public Task<DataTableResponseModel<WithdrawRequestModel>> GetWithdrawDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userWithdrawHistoryRepo.Get(x => x.Status == Enums.WithdrawStatus.New
            || x.Status == Enums.WithdrawStatus.Confirming
            || x.Status == Enums.WithdrawStatus.Approved
            || x.Status == Enums.WithdrawStatus.Reject);
            var listData = query.QueryTo<WithdrawRequestModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public Task<DataTableResponseModel<WithdrawRequestModel>> GetWithdrawTigeDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userWithdrawHistoryRepo.Get(x => x.Status == Enums.WithdrawStatus.TokenNew
            || x.Status == Enums.WithdrawStatus.TokenConfirming
            || x.Status == Enums.WithdrawStatus.TokenApproved
            || x.Status == Enums.WithdrawStatus.TokenReject);
            var listData = query.QueryTo<WithdrawRequestModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public Task<WithdrawRequestModel> GetWithdrawByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var withdraw = _userWithdrawHistoryRepo.Get(x => x.Id == id).QueryTo<WithdrawRequestModel>().FirstOrDefault();
            return Task.FromResult(withdraw);
        }

        //fix
        public async Task UpdateWithdrawAsync(WithdrawRequestModel model, CancellationToken cancellationToken = default)
        {
            var withDraw = _userWithdrawHistoryRepo.Get(x => x.Id == model.Id).FirstOrDefault();
            if (withDraw != null)
            {
                withDraw.AmountETH = model.AmountETH;
                withDraw.Rate = model.Rate;
                withDraw.TxHash = model.TxHash;
                withDraw.FeeUSD = model.FeeUSD;
                withDraw.AmountUSD = model.AmountETH * model.Rate;
                withDraw.Status = model.Status;
                withDraw.FromWalletId = model.FromWalletId;
                if (model.Status == Enums.WithdrawStatus.Approved || model.Status == Enums.WithdrawStatus.TokenApproved)
                {
                    withDraw.ApproveById = LoggedInUser.Current.Id;
                    _userWithdrawHistoryRepo.Update(withDraw,
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
                    _userWithdrawHistoryRepo.Update(withDraw,
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

        public Task CheckConfirmDepositWithToken(string token, CancellationToken cancellationToken = default)
        {
            var request = _userDepositRepo.Get(x => x.ConfirmToken == token).FirstOrDefault();
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
            request.Status = Enums.WalletDepositStatus.Confirming;
            _userDepositRepo.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status, x => x.ExpireTime);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public WalletDepositModel GetByLatestDepositAsync(string userId)
        {
            var wallet = _userWalletRepo.Get(x => x.UserId == userId).FirstOrDefault();

            var depositRequestEntity = _userDepositRepo.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<WalletDepositModel>().FirstOrDefault();

            return depositRequestEntity;
        }

        public WithdrawRequestModel GetByLatestWithDrawAsync(string userId)
        {
            var wallet = _userWalletRepo.Get(x => x.UserId == userId).FirstOrDefault();

            if (wallet ==null)
            {
                return null;
            }
            var depositRequestEntity = _userWithdrawHistoryRepo.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<WithdrawRequestModel>().FirstOrDefault();

            return depositRequestEntity;
        }

        public DetailSlotRequestModel GetByLatestSlotAsync(string userId)
        {
            var requestModel = _userSlotRepo.Get(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedTime).QueryTo<DetailSlotRequestModel>().FirstOrDefault();

            return requestModel;
        }

        public async Task ResendConfirmDeposit(string data, CancellationToken cancellationToken = default)
        {
            var history = _userDepositRepo.Get(x => x.Id == data).FirstOrDefault();
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
            _userDepositRepo.Update(history, x=>x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();

            await _emailService.SendVerifyDepositAsync(data, cancellationToken).ConfigureAwait(true);
            return;
        }

        public async Task ResendConfirmWithdraw(string data, CancellationToken cancellationToken = default)
        {
            var history = _userWithdrawHistoryRepo.Get(x => x.Id == data).FirstOrDefault();
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
            _userWithdrawHistoryRepo.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();


            //send email
            await _emailService.SendVerifyWithdrawAsync(data, cancellationToken).ConfigureAwait(true);
        }

        public async Task ResendConfirmTransfer(string data, CancellationToken cancellationToken = default)
        {
            var history = _userWithdrawHistoryRepo.Get(x => x.Id == data).FirstOrDefault();
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
            _userWithdrawHistoryRepo.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();


            //send email
            await _emailService.SendVerifyWithdrawAsync(data, cancellationToken).ConfigureAwait(true);
        }        
    }
}