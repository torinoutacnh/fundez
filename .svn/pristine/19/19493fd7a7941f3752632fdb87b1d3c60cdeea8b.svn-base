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
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(ISlotService))]
    public class SlotService : Base.Service, ISlotService
    {
        private readonly IRepository<UserSlotsEntity> _userSlotRepo;
        private readonly IRepository<UserBusinessEntity> _userBusinessRepo;
        private readonly IRepository<UserWalletEntity> _userWalletRepo;
        private readonly IRepository<UserTokensEntity> _userTokenRepo;
        private readonly IRepository<ConfigurationEntity> _configRepo;
        private readonly ICommonService _commonService;
        private readonly IEmailService _emailService;

        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKeyPrefix = "User_";


        public SlotService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ICommonService commonService, IEmailService emailService) : base(unitOfWork)
        {
            _memoryCache = memoryCache;
            _commonService = commonService;
            _emailService = emailService;
            _userSlotRepo = unitOfWork.GetRepository<UserSlotsEntity>();
            _userTokenRepo = unitOfWork.GetRepository<UserTokensEntity>();
            _configRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userWalletRepo = unitOfWork.GetRepository<UserWalletEntity>();
            _userBusinessRepo = unitOfWork.GetRepository<UserBusinessEntity>();
        }

        public async Task<string> BuySlot(SubmitSlotModel model, CancellationToken cancellationToken = default)
        {
            // config 
            var config = _configRepo.Get().FirstOrDefault();

            if (config == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Invalid config");
            }

            var totalAmount = model.Quantity * config.SlotPrice;
            if (totalAmount > LoggedInUser.Current.WalletBalance)
            {
                throw new CoreException(ErrorCode.BadRequest, "Your Slot Quantity over your balance Amount can pay.");
            }

            // add slot
            var newSlot = new UserSlotsEntity()
            {
                UserId = LoggedInUser.Current.Id,
                CurrentBalance = LoggedInUser.Current.WalletBalance,
                Quantity = model.Quantity,
                UnitPrice = config.SlotPrice,
                TotalAmount = totalAmount,
                ApproveTime = null,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };
            _userSlotRepo.Add(newSlot);
            UnitOfWork.SaveChanges();

            // update balance
            await _commonService.UpdateWalletBalance(LoggedInUser.Current.Id, cancellationToken);

            await _emailService.SendAsync(LoggedInUser.Current.Id, EmailTemplate.BuySlot, cancellationToken);
            return newSlot.Id;
        }

        public Task<List<DetailSlotRequestModel>> GetMySlot(string userId, CancellationToken cancellationToken = default)
        {
            var result = _userSlotRepo.Get(x => x.UserId == userId)
                .QueryTo<DetailSlotRequestModel>().OrderByDescending(x=>x.Time).ToList();
            return Task.FromResult(result);
        }

        public async Task<DetailSlotRequestModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var slotRequest = _userSlotRepo.Get(x => x.Id == id).QueryTo<DetailSlotRequestModel>().FirstOrDefault();
            return slotRequest;
        }

        public async Task UpdateAsync(DetailSlotRequestModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepo.Get().FirstOrDefault();
            var slot = _userSlotRepo.Get(x => x.Id == model.Id).FirstOrDefault();
            var commission = _userBusinessRepo.Get(x => x.FromUserSlotId == slot.Id).FirstOrDefault();
            var wallet = _userWalletRepo.Get(x => x.UserId == slot.UserId).FirstOrDefault();
            var token = _userTokenRepo.Get(x => x.SlotId == slot.Id).FirstOrDefault();

            if (config == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Invalid config");
            }

            if (slot == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Slot.DoesNotExist);
            }

            var totalAmount = model.Quantity * config.SlotPrice;
            if (wallet != null && totalAmount > wallet.AmountUSD)
            {
                throw new CoreException(ErrorCode.BadRequest, "Your Slot Quantity over your balance Amount can pay.");
            }

            slot.Quantity = model.Quantity;
            slot.TotalAmount = totalAmount;
            slot.TokenQuantity = Math.Round(model.TokenQuantity, 4);

            _userSlotRepo.Update(slot, x => x.Quantity, x => x.TotalAmount, x => x.TokenQuantity);
            UnitOfWork.SaveChanges();

            if (token != null)
            {
                token.Quantity = Math.Round(slot.TokenQuantity, 4);
                token.TotalAmount = token.Quantity * config.TokenPrice;
                token.Rate = config.TokenPrice;

                _userTokenRepo.Update(token, x => x.Quantity, x => x.TotalAmount, x => x.Rate);
                UnitOfWork.SaveChanges();
            }

            await _commonService.UpdateCommissionBalance(slot.Id, cancellationToken);

            // load balance
            await _commonService.UpdateWalletBalance(slot.UserId, cancellationToken);
        }

        public Task<DataTableResponseModel<DetailSlotRequestModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _userSlotRepo.Get();
            var listData = query.QueryTo<DetailSlotRequestModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var slot = _userSlotRepo.Get(x => x.Id == id).FirstOrDefault();
            if (slot == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Slot.DoesNotExist);
            }

            var business = _userBusinessRepo.Get(x => x.FromUserSlotId == id).ToList();
            _userBusinessRepo.DeleteWhere(x => x.FromUserSlotId == id);
            UnitOfWork.SaveChanges();

            _userSlotRepo.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            await _commonService.UpdateWalletBalance(slot.UserId, cancellationToken);
            foreach (var userBusinessEntity in business)
            {
                await _commonService.UpdateWalletBalance(userBusinessEntity.ToUserId, cancellationToken);
            }
        }

        public async Task VerifyBuySlot(string token, CancellationToken cancellationToken = default)
        {
            var slot = _userSlotRepo.Get(x => x.ConfirmToken == token).FirstOrDefault();
            if (slot == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.InvalidCodeCodeErrorMessage);
            }

            if (slot.ExpireTime < CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ExpiredCodeErrorMessage);
            }

            var config = _configRepo.Get().FirstOrDefault();
            if (config == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Invalid config");
            }

            slot.Status = Enums.SlotStatus.Confirmed;
            slot.TokenQuantity = Math.Round(slot.Quantity * config.SlotToToken, 4);
            slot.ConfirmToken = null;
            slot.ExpireTime = null;
            slot.ConfirmedTime = CoreHelper.SystemTimeNow;

            _userSlotRepo.Update(slot, x => x.Status, x => x.TokenQuantity, x=>x.ConfirmToken, x=>x.ExpireTime, x=>x.ConfirmedTime);
            UnitOfWork.SaveChanges();

            // add tokens
            var newToken = new UserTokensEntity()
            {
                Quantity = Math.Round(slot.Quantity * config.SlotToToken, 4),
                Rate = config.TokenPrice,
                TotalAmount = config.TokenPrice * slot.Quantity * config.SlotToToken,
                SlotId = slot.Id,
                UserId = slot.CreatedBy,
            };
            _userTokenRepo.Add(newToken);
            UnitOfWork.SaveChanges();


            _userSlotRepo.Update(slot, x=>x.Status);
            UnitOfWork.SaveChanges();

            await _commonService.UpdateCommissionBalance(slot.Id, cancellationToken);

            // load balance
            await _commonService.UpdateWalletBalance(slot.UserId, cancellationToken);
        }

        public async Task ResendConfirmBuyAsync(string id, CancellationToken cancellationToken = default)
        {
            var slot = _userSlotRepo.Get(x => x.Id == id).FirstOrDefault();


            if (slot != null && slot.ExpireTime > CoreHelper.SystemTimeNow)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.ResendCodeErrorMessage);
            }

            _emailService.SendBuySlotById(id);
            return;
        }
    }
}