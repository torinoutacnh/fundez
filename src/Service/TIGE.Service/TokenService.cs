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
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(ITokenService))]
    public class TokenService : Base.Service, ITokenService
    {
        private readonly IRepository<UserSellTokenEntity> _userSellTokenRepo;
        private readonly IRepository<UserBusinessEntity> _userBusinessRepo;
        private readonly IRepository<UserWalletEntity> _userWalletRepo;
        private readonly IRepository<UserTokensEntity> _userTokenRepo;
        private readonly IRepository<ConfigurationEntity> _configRepo;
        private readonly IRepository<UserWithDrawRequestEntity> _userWithdrawRequestRepo;
        private readonly ICommonService _commonService;
        private readonly IEmailService _emailService;

        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKeyPrefix = "User_";



        public TokenService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ICommonService commonService, IEmailService emailService) : base(unitOfWork)
        {
            _memoryCache = memoryCache;
            _commonService = commonService;
            _emailService = emailService;
            _configRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userWalletRepo = unitOfWork.GetRepository<UserWalletEntity>();
            _userBusinessRepo = unitOfWork.GetRepository<UserBusinessEntity>();
            _userSellTokenRepo = unitOfWork.GetRepository<UserSellTokenEntity>();
            _userTokenRepo = unitOfWork.GetRepository<UserTokensEntity>();
            _userWithdrawRequestRepo = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
        }

        public async Task<string> SellToken(SubmitTokenModel model, CancellationToken cancellationToken = default)
        {
            // config 
            var config = _configRepo.Get().FirstOrDefault();

            if (config == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Invalid config");
            }
            if(model.Quantity < Convert.ToDouble(config.SalePhone2))
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please input amount greater than {config.SalePhone2} TIGE");
            }

            var totalToken = _userTokenRepo.Get(x => x.UserId == LoggedInUser.Current.Id).Sum(x => x.Quantity);
            var sellTokenQuantity = _userSellTokenRepo.Get(x => x.UserId == LoggedInUser.Current.Id)
                .Sum(x => x.TokenQuantity);
            var currentToken  = totalToken - sellTokenQuantity;

            if (model.Quantity > currentToken)
            {
                throw new CoreException(ErrorCode.BadRequest, "You can not sell token over your current Token.");
            }

            var amount = await _commonService.CalculatingTokenPrice(model.Quantity);
            // add Token
            var newToken = new UserSellTokenEntity()
            {
                UserId = LoggedInUser.Current.Id,
                TokenQuantity = model.Quantity,
                UnitPrice = amount.UnitPrice,
                FeeAmount = amount.FeeAmount,
                TotalAmount = amount.TotalAmount,
                ApproveTime = null,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };
            _userSellTokenRepo.Add(newToken);
            UnitOfWork.SaveChanges();

            _memoryCache.Remove("Dashboard_" + LoggedInUser.Current.Id);
            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.SellToken, cancellationToken);
            return newToken.Id;
        }

        public Task<List<DetailSellTokenModel>> GetMySellToken(string userId, CancellationToken cancellationToken = default)
        {
            var result = _userSellTokenRepo.Get(x => x.UserId == userId)
                .QueryTo<DetailSellTokenModel>().OrderByDescending(x => x.CreatedTime).ToList();
            return Task.FromResult(result);
        }

        public async Task<DetailSellTokenModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var tokenRequest = _userSellTokenRepo.Get(x => x.Id == id).QueryTo<DetailSellTokenModel>().FirstOrDefault();
            return tokenRequest;
        }

        public async Task UpdateAsync(DetailSellTokenModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepo.Get().FirstOrDefault();
            var sellToken = _userSellTokenRepo.Get(x => x.Id == model.Id).FirstOrDefault();

            if (config == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Invalid config");
            }

            if (model.TokenQuantity < Convert.ToDouble(config.SalePhone2))
            {
                throw new CoreException(ErrorCode.BadRequest, $"Please put Quantity higher than {config.SalePhone2}");
            }

            if (sellToken == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Token.DoesNotExist);
            }

            var totalToken = _userTokenRepo.Get(x => x.UserId == sellToken.UserId);
            var totalTokenQuantity = totalToken.Sum(x => x.Quantity);

            var totalAmount = await _commonService.CalculatingTokenPrice(sellToken.TokenQuantity);
            if (sellToken.TokenQuantity > totalTokenQuantity)
            {
                throw new CoreException(ErrorCode.BadRequest, "Your Sell token over your available token.");
            }

            sellToken.TokenQuantity = Math.Round(model.TokenQuantity, 4);
            sellToken.FeeAmount = totalAmount.FeeAmount;
            sellToken.TotalAmount = totalAmount.TotalAmount;
            sellToken.UnitPrice = totalAmount.UnitPrice;
            sellToken.Status = model.Status;

            if (model.Status == Enums.TokenStatus.Approved)
            {
                sellToken.ApproveTime = DateTimeOffset.Now;
                sellToken.ApproveBy = LoggedInUser.Current.Id;
            }

            _userSellTokenRepo.Update(sellToken, x => x.TokenQuantity, x => x.TotalAmount, x => x.TokenQuantity,
                x=>x.Status, x=>x.ApproveBy, x=>x.ApproveTime);
            UnitOfWork.SaveChanges();

            // load balance
            await _commonService.UpdateWalletBalance(sellToken.UserId, cancellationToken);
        }

        public Task<DataTableResponseModel<DetailSellTokenModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userSellTokenRepo.Get();
            var listData = query.QueryTo<DetailSellTokenModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var token = _userSellTokenRepo.Get(x => x.Id == id).FirstOrDefault();
            if (token == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Token.DoesNotExist);
            }

            _userSellTokenRepo.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            await _commonService.UpdateWalletBalance(token.UserId, cancellationToken);
        }

        public async Task VerifySellToken(string token, CancellationToken cancellationToken = default)
        {
            var generated = _userSellTokenRepo.Get(x => x.ConfirmToken == token && x.Status == Enums.TokenStatus.New).FirstOrDefault();
            if (generated == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (generated.ExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest,Messages.Common.ExpiredCodeErrorMessage);
            }

            generated.Status = Enums.TokenStatus.Confirmed;
            var total = await _commonService.CalculatingTokenPrice(generated.TokenQuantity);
            generated.FeeAmount = total.FeeAmount;
            generated.UnitPrice = total.UnitPrice;
            generated.TotalAmount = total.TotalAmount;
            generated.ConfirmToken = null;
            generated.ExpireTime = null;
            generated.ConfirmedTime = CoreHelper.SystemTimeNow;

            _userSellTokenRepo.Update(generated, x => x.Status, x => x.TotalAmount, x=>x.FeeAmount, x=>x.UnitPrice, x=>x.ConfirmToken, x=>x.ConfirmedTime, x=>x.ExpireTime);
            UnitOfWork.SaveChanges();
        }

        public DetailSellTokenModel GetByLatestSellTokenAsync(string userId)
        {
            var tokenRequest = _userSellTokenRepo.Get(x => x.UserId == userId).QueryTo<DetailSellTokenModel>().OrderByDescending(x => x.CreatedTime).FirstOrDefault();
            return tokenRequest;
        }

        public Task<double> GetTotalToken(string userId, CancellationToken cancellationToken = default)
        {
            var allToken = _userTokenRepo.Get(x => x.UserId == userId).ToList();
            var total = allToken.Sum(x => x.Quantity);

            var wallet = _userWalletRepo.Get(x => x.UserId == userId).FirstOrDefault();

            var withdrawToken = _userWithdrawRequestRepo.Get(x => x.WalletId == wallet.Id).Where(x=>x.Status == Enums.WithdrawStatus.TokenNew
            || x.Status == Enums.WithdrawStatus.TokenConfirming
            || x.Status == Enums.WithdrawStatus.TokenApproved);
            var sumWithdraw = withdrawToken.Sum(x => x.AmountUSD) + withdrawToken.Sum(x => x.FeeUSD);

            var transfers = _userWithdrawRequestRepo.Get(x => x.WalletId == wallet.Id).Where(x => x.Status == Enums.WithdrawStatus.NewTransfer
            || x.Status == Enums.WithdrawStatus.ConfirmingTransfer
            || x.Status == Enums.WithdrawStatus.ApprovedTransfer);
            var sumtransfer = transfers.Sum(x => x.AmountUSD) - transfers.Sum(x=>x.FeeUSD);

            var sellToken = _userSellTokenRepo.Get(x => x.UserId == userId).Where(x => x.Status != Enums.TokenStatus.Reject).ToList();
            var sell = sellToken.Sum(x => x.TokenQuantity);
            var fee = sellToken.Sum(x=>x.FeeAmount);

            //return Task.FromResult(total - sell);
            return Task.FromResult(total - sumWithdraw - sumtransfer - sell - fee);
        }

        public async Task ResendSellToken(string data, CancellationToken cancellationToken = default)
        {
            var tokenEntity = _userSellTokenRepo.Get(x => x.UserId == LoggedInUser.Current.Id && x.ConfirmToken != null).OrderByDescending(x => x.CreatedTime)
                .FirstOrDefault();

            if (tokenEntity == null)
            {
                return;
            }


            if (tokenEntity.ExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }

            tokenEntity.ConfirmToken = StringHelper.Generate(6, false, false, true, false);
            tokenEntity.ConfirmedTime = null;
            tokenEntity.ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5);
            _userSellTokenRepo.Update(tokenEntity, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();


            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.SellToken, cancellationToken);
            return;
        }
    }
}