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
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Stack;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(ISubscriptionService))]
    public class SubscriptionService : Base.Service, ISubscriptionService
    {
        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKeyPrefix = "User_";

        private readonly TimeSpan _cacheSliding = TimeSpan.FromHours(4);

        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailService _emailService;

        private readonly ICommonService _commonService;

        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<StackingWalletEntity> _stackingWalletRepository;
        private readonly IRepository<SubscriptionEntity> _subscriptionRepository;
        private readonly IRepository<StackHistoryEntity> _stackHistoryRepository;
        private readonly IRepository<HistoryRefundEntity> _refundRepository;
        private readonly IRepository<StackingConfigEntity> _configRepository;

        public SubscriptionService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMemoryCache memoryCache, IEmailService emailService, ICommonService commonService) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _emailService = emailService;
            _memoryCache = memoryCache;

            _commonService = commonService;

            _userRepository = unitOfWork.GetRepository<UserEntity>();
            _stackingWalletRepository = unitOfWork.GetRepository<StackingWalletEntity>();
            _subscriptionRepository = unitOfWork.GetRepository<SubscriptionEntity>();
            _stackHistoryRepository = unitOfWork.GetRepository<StackHistoryEntity>();
            _refundRepository = unitOfWork.GetRepository<HistoryRefundEntity>();
            _configRepository = unitOfWork.GetRepository<StackingConfigEntity>();
        }
        public Task<List<SubscriptionEntity>> GetSubscriptions()
        {
            var subscriptions = _subscriptionRepository.Get().ToList();

            return Task.FromResult(subscriptions);
        }

        public async Task<StackingWalletEntity> GetMyWalletAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = _userRepository.Get(x => x.Id == LoggedInUser.Current.Id).FirstOrDefault();
            if (user == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }

            var wallet = _stackingWalletRepository.Get(x => x.UserId == id).FirstOrDefault();

            if (wallet != null)
            {
                var history = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id)
                    .OrderByDescending(x => x.CreatedTime).Take(30)
                    .ToList();

                wallet.StackHistories = history;
            }

            return await Task.FromResult(wallet);
        }

        public async Task ResendConfirmStack(string data, CancellationToken cancellationToken = default)
        {
            var history = _stackHistoryRepository.Get(x => x.Id == data).FirstOrDefault();
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
            _stackHistoryRepository.Update(history, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.ExpireTime);
            UnitOfWork.SaveChanges();

            await _emailService.SendVerifyStackAsync(data, cancellationToken).ConfigureAwait(true);
            return;
        }

        public Task VerifyBuyStack(string token, CancellationToken cancellationToken = default)
        {
            var request = _stackHistoryRepository.Get(x => x.ConfirmToken == token).FirstOrDefault();
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
            request.Status = Enums.StackState.Staking;
            _stackHistoryRepository.Update(request, x => x.ConfirmToken, x => x.ConfirmedTime, x => x.Status);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<string> SubmitStackAsync(SubmitStackModel model, CancellationToken cancellationToken = default)
        {
            var config = _configRepository.Get().FirstOrDefault();
            var wallet = _stackingWalletRepository.Get(x => x.UserId == LoggedInUser.Current.Id).FirstOrDefault();
            if(model == null || wallet == null || config==null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            if (model.AmountTige > wallet.Balance)
            {
                throw new CoreException(ErrorCode.BadRequest, "Input amount samller than your Balance");
            }
            if (model.AmountTige < config.MinStacking)
            {
                throw new CoreException(ErrorCode.BadRequest, $"Input amount higher than {config.MinStacking} TIGE");
            }

            var subscription = _subscriptionRepository.Get(x => x.Day == model.Day).FirstOrDefault();
            if(subscription == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            var stack = new StackHistoryEntity()
            {
                StackAmount = model.AmountTige,
                Status = Enums.StackState.New,
                SubscriptionId = subscription.Id,
                SubscriptionDetail = subscription.Description,
                Day = subscription.Day,
                WalletId = wallet.Id,
                Rate = SystemHelper.GetETHRate(),
                DateEnd = DateTimeOffset.Now.AddDays(model.Day),
                DailyReward = ((model.AmountTige * subscription.Reward) / 100) / subscription.Day,
                TotalReward = (model.AmountTige * subscription.Reward) / 100,
                ConfirmToken = StringHelper.Generate(6, false, false, true, false),
                ExpireTime = CoreHelper.SystemTimeNow.AddMinutes(5)
            };

            _stackHistoryRepository.Add(stack);
            UnitOfWork.SaveChanges();

            await _emailService.SendVerifyStackAsync(stack.Id, cancellationToken).ConfigureAwait(true);
            await _commonService.UpdateStackWalletBalance(wallet.Id);

            return stack.Id;
        }
        public StackHistoryModel GetLatestStackRequest(string userid)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == userid).FirstOrDefault();

            if (wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            var history = _stackHistoryRepository.Get(x => x.WalletId == wallet.Id)
                .OrderByDescending(x => x.CreatedTime).QueryTo<StackHistoryModel>().FirstOrDefault();

            return history;
        }

        public async Task<List<HistoryRefundEntity>> GetRefundList(string userid, CancellationToken cancellationToken = default)
        {
            var wallet = _stackingWalletRepository.Get(x => x.UserId == userid).FirstOrDefault();
            if(wallet == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }

            var histories = _refundRepository.Get(x=>x.WalletId==wallet.Id).OrderByDescending(x=>x.CreatedTime).ToList();

            return await Task.FromResult(histories);
        }

        public Task RefundToUser(CancellationToken cancellationToken = default)
        {
            var stacks = _stackHistoryRepository.Get(x => x.Status != Enums.StackState.Finished && x.Status != Enums.StackState.New);
            foreach (var stack in stacks)
            {
                if (stack.DateEnd < DateTimeOffset.Now)
                {
                    stack.Status = Enums.StackState.Finished;
                    _stackHistoryRepository.Update(stack,x=>x.Status);
                }
                else
                {
                    var count = _refundRepository.Get(x=>x.FromStack==stack.Id && x.CreatedTime.Day == DateTime.Now.Day).Count();
                    if (count < 1)
                    {
                        //var prerefund = _refundRepository.Get(x => x.FromStack == stack.Id).OrderByDescending(x => x.CreatedTime).FirstOrDefault();
                        var refund = new HistoryRefundEntity()
                        {
                            WalletId = stack.WalletId,
                            Amount = stack.DailyReward,
                            Rate = SystemHelper.GetETHRate(),
                            FromStack = stack.Id,
                        };
                        _refundRepository.Add(refund);
                    }
                }
                _commonService.UpdateStackWalletBalance(stack.WalletId);
            }
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task DeleteStackAsync(string id, CancellationToken cancellationToken = default)
        {
            var history = _stackHistoryRepository.Get(x => x.Id == id).Include(x => x.Wallet).Select(x => x.Wallet).FirstOrDefault();

            _stackHistoryRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            if (history != null)
            {
                await _commonService.UpdateStackWalletBalance(history.Id, cancellationToken);
            }
        }

        public async Task DeleteSubscriptionAsync(string id, CancellationToken cancellationToken = default)
        {
            var wallet = _stackHistoryRepository.Get(x => x.Id == id).FirstOrDefault().WalletId;

            _stackHistoryRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            if (wallet != null)
            {
                await _commonService.UpdateStackWalletBalance(wallet, cancellationToken);
            }
        }

        public Task<DataTableResponseModel<SubscriptionModel>> GetSubscriptionDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _subscriptionRepository.Get();
            var list = query.QueryTo<SubscriptionModel>();
            var result = list.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public Task<string> UpdateSubscriptionDetail(SubscriptionModel model , CancellationToken cancellationToken = default)
        {
            var sub = _subscriptionRepository.Get(x => x.Id == model.Id).FirstOrDefault();

            sub.Day = model.Day;
            sub.Description = model.Description;
            sub.Reward = model.Reward;
            sub.LastUpdatedTime = DateTimeOffset.UtcNow;

            _subscriptionRepository.Update(sub, x=>x.Day,x=>x.LastUpdatedTime,x=>x.Description,x=>x.Reward);
            UnitOfWork.SaveChanges();


            return Task.FromResult(sub.Id);
        }

        public Task<SubscriptionModel> GetSubscriptionModel(string id, CancellationToken cancellationToken = default)
        {
            var sub = _subscriptionRepository.Get(x => x.Id == id).QueryTo<SubscriptionModel>().FirstOrDefault();

            return Task.FromResult(sub);
        }

        public Task<SubscriptionEntity> CreateAsync(SubscriptionEntity model, CancellationToken cancellationToken = default)
        {
            var sub = _subscriptionRepository.Add(model);
            UnitOfWork.SaveChanges();

            return Task.FromResult(sub);
        }

        public Task<DataTableResponseModel<RewardModel>> GetRewardDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _refundRepository.Get();
            var listData = query.QueryTo<RewardModel>();
            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (RewardModel)x);
            var rewardids = data.Select(x=>x.Id);
            var walletids = data.Select(x => x.WalletId);

            var wallets = _stackingWalletRepository.Get(x => walletids.Contains(x.Id)).ToList();

            foreach(var reward in data)
            {
                var userid = _stackingWalletRepository.Get(x=>x.Id==reward.WalletId).Select(x=>x.UserId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == userid).FirstOrDefault();
                reward.UserId = user.Code;
                reward.Email = user.Email;
            }


            return Task.FromResult(result);
        }

        public Task<DataTableResponseModel<StackHistoryModel>> GetStakeDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _stackHistoryRepository.Get();
            var listData = query.QueryTo<StackHistoryModel>();
            var result = listData.GetDataTableResponse(model);

            var data = result.Data.Select(x => (StackHistoryModel)x);
            var walletids = data.Select(x => x.WalletId);

            var wallets = _stackingWalletRepository.Get(x => walletids.Contains(x.Id)).ToList();

            foreach (var reward in data)
            {
                var userid = _stackingWalletRepository.Get(x => x.Id == reward.WalletId).Select(x => x.UserId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == userid).FirstOrDefault();
                reward.UserId = user.Code;
                reward.Email = user.Email;
            }


            return Task.FromResult(result);
        }
    }
}
