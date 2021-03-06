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

using System;
using System.Collections.Generic;
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
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TIGE.Contract.Repository.Models;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(ICommonService))]
    public class CommonService : Base.Service, ICommonService
    {
        private readonly IRepository<UserWalletEntity> _userWalletRepo;
        private readonly IRepository<UserSlotsEntity> _userSlotRepo;
        private readonly IRepository<UserSellTokenEntity> _userSellTokenRepo;
        private readonly IRepository<UserBusinessEntity> _userBusinessRepo;
        private readonly IRepository<UserEntity> _userRepo;
        private readonly IRepository<ConfigurationEntity> _configRepo;
        private readonly IRepository<ProtectionFeeEntity> _feeRepository;
        private readonly IRepository<UserDepositRequestEntity> _userWalletHistoryRepo;
        private readonly IRepository<UserWithDrawRequestEntity> _userWithdrawHistoryRepo;
        private readonly IRepository<UserTokensEntity> _userTokenRepo;
        private readonly IRepository<StackingWalletEntity> _stackingWalletRepository;
        private readonly IRepository<HistoryDepositEntity> _stackingDepositRepository;
        private readonly IRepository<HistoryWithdrawTokenEntity> _stackingWithdrawRepository;
        private readonly IRepository<HistoryWithdrawUSDEntity> _stackingWithdrawUSDRepository;
        private readonly IRepository<StackHistoryEntity> _stackRepository;
        private readonly IRepository<HistoryRefundEntity> _refundRepository;
        private readonly IRepository<StackCommissionEntity> _stackCommission;
        private readonly IRepository<TransferTokenEntity> _transferTokenRepository;

        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKeyPrefix = "User_";

        public CommonService(IUnitOfWork unitOfWork, IMemoryCache memoryCache) : base(unitOfWork)
        {
            _memoryCache = memoryCache;
            _userWalletRepo = unitOfWork.GetRepository<UserWalletEntity>();
            _userWalletHistoryRepo = unitOfWork.GetRepository<UserDepositRequestEntity>();
            _userWithdrawHistoryRepo = unitOfWork.GetRepository<UserWithDrawRequestEntity>();
            _userSlotRepo = unitOfWork.GetRepository<UserSlotsEntity>();
            _configRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _userRepo = unitOfWork.GetRepository<UserEntity>();
            _userBusinessRepo = unitOfWork.GetRepository<UserBusinessEntity>();
            _feeRepository = unitOfWork.GetRepository<ProtectionFeeEntity>();
            _userSellTokenRepo = unitOfWork.GetRepository<UserSellTokenEntity>();
            //_userTokenRepo = unitOfWork.GetRepository<UserTokensEntity>();
            _stackingWalletRepository = unitOfWork.GetRepository<StackingWalletEntity>();
            _stackingDepositRepository = unitOfWork.GetRepository<HistoryDepositEntity>();
            _stackingWithdrawRepository = unitOfWork.GetRepository<HistoryWithdrawTokenEntity>();
            _stackRepository = unitOfWork.GetRepository<StackHistoryEntity>();
            _refundRepository = unitOfWork.GetRepository<HistoryRefundEntity>();
            _transferTokenRepository = unitOfWork.GetRepository<TransferTokenEntity>();
            _stackingWithdrawUSDRepository = unitOfWork.GetRepository<HistoryWithdrawUSDEntity>();
        }

        public Task UpdateWalletBalance(string userId, CancellationToken cancellationToken = default)
        {
            BackgroundJob.Enqueue(() => UpdateWalletBalanceTask(userId));
            return Task.CompletedTask;
        }

        public Task UpdateStackWalletBalance(string walletid, CancellationToken cancellationToken = default)
        {
            BackgroundJob.Enqueue(() => UpdateStackWalletBalanceTask(walletid));
            return Task.CompletedTask;
        }

        //fix
        public Task UpdateWalletBalanceTask(string userId)
        {
            var wallet = _userWalletRepo.Get(x => x.UserId == userId).FirstOrDefault();
            if (wallet == null)
            {
                return Task.CompletedTask;
            }

            //deposit
            var allDeposits = _userWalletHistoryRepo.Get(x => x.WalletId == wallet.Id && x.Status == Enums.WalletDepositStatus.Approved).ToList();
            var depositAmount = allDeposits.Sum(x => x.AmountUSD);

            // withdraw
            var allWithdraw = _userWithdrawHistoryRepo.Get(x => x.WalletId == wallet.Id 
            && (x.Status == Enums.WithdrawStatus.New
            || x.Status == Enums.WithdrawStatus.Confirming
            || x.Status == Enums.WithdrawStatus.Approved)).ToList();
            var withdrawAmount = allWithdraw.Sum(x => x.AmountUSD);
            var totalFee = allWithdraw.Sum(x => x.FeeUSD);

            // buy slots
            var allSlot = _userSlotRepo.Get(x => x.UserId == userId).ToList();
            var slotAmount = allSlot.Sum(x => x.TotalAmount);

            // sell token
            var allSell = _userSellTokenRepo.Get(x => x.UserId == userId && x.Status == Enums.TokenStatus.Approved).ToList();
            var sellTokenAmount = allSell.Sum(x => x.TotalAmount);
             
            // commission
            var allCommission = _userBusinessRepo.Get(x => x.ToUserId == userId).ToList();
            var commisionAmount = allCommission.Sum(x => x.Commission);

            // update balance
            var totalBalance = depositAmount + commisionAmount + sellTokenAmount - withdrawAmount - slotAmount - totalFee;

            wallet.AmountUSD = totalBalance > 0 ? totalBalance : 0;
            _userWalletRepo.Update(wallet, x => x.AmountUSD);
            UnitOfWork.SaveChanges();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + wallet.UserId;
            _memoryCache.Remove(cacheKey);

            var cacheKey2 = "Dashboard_" + userId;
            _memoryCache.Remove(cacheKey2);

            return Task.CompletedTask;
        }

        public Task UpdateStackWalletBalanceTask(string walletid)
        {
            var wallet = _stackingWalletRepository.Get(x => x.Id == walletid).FirstOrDefault();

            if (wallet == null)
            {
                //throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
                return Task.CompletedTask;
            }
            //deposit + refund + transfer - currentstacking - withdraw
            //Total deposit
            var deposit = _stackingDepositRepository.Get(x => x.WalletId == walletid && x.Status == Enums.StackDeposit.Approved).Sum(x => x.AmountTige);

            //Total daily refund
            var dailyrefund = _refundRepository.Get(x => x.WalletId == walletid).Sum(x => x.Amount);

            //Total commission refund
            var commissions = _stackCommission.Get(x => x.WalletId == walletid).Sum(x => x.Amount);

            //Total transfer
            double transfer = 0;
            double fee = 0;
            var tigeWallet = _userWalletRepo.Get(x => x.UserId == wallet.UserId).FirstOrDefault();
            if (tigeWallet != null)
            {
                var transfers = _userWithdrawHistoryRepo.Get(x => x.WalletId == tigeWallet.Id).Where(x => x.Status == Enums.WithdrawStatus.ConfirmingTransfer || x.Status == Enums.WithdrawStatus.ApprovedTransfer);

                transfer = transfers.Sum(x => x.AmountUSD);
            }

            //Total Withdraw USD
            var withdrawUSDs = _stackingWithdrawUSDRepository.Get(x => x.WalletId == wallet.Id).Where(x => x.Status != Enums.StackWithdrawUSD.Reject);
            var totalUSDWithdraw = withdrawUSDs.Sum(x=>x.AmountTige);
            //var transfer = _transferTokenRepository.Get(x => x.WalletId == walletid && x.Status == Enums.StackTransfer.Approved).Sum(x => x.AmountTige);
            //var transferfee = _transferTokenRepository.Get(x => x.WalletId == walletid && x.Status == Enums.StackTransfer.Approved).Sum(x => x.FeeTige);

            //Total stacking
            var stack = _stackRepository.Get(x => x.WalletId == walletid && x.Status != Enums.StackState.Finished).Sum(x => x.StackAmount);

            //Total withdraw and fee
            var totalWithdraw = _stackingWithdrawRepository.Get(x => x.WalletId == walletid && x.Status != Enums.StackWithdraw.Reject).Sum(x => x.AmountTige);
            var totalFee = _stackingWithdrawRepository.Get(x => x.WalletId == walletid && x.Status != Enums.StackWithdraw.Reject).Sum(x => x.FeeTige);

            wallet.Balance = deposit + dailyrefund + transfer - fee - stack - totalWithdraw - totalFee - totalUSDWithdraw;

            _stackingWalletRepository.Update(wallet,x => x.Balance);
            UnitOfWork.SaveChanges();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + wallet.UserId;
            _memoryCache.Remove(cacheKey);

            var cacheKey2 = "StackDashboard_" + wallet.UserId;
            _memoryCache.Remove(cacheKey2);

            return Task.CompletedTask;
        }

        public async Task<List<UserModel>> GetTotalMember(string userId, CancellationToken cancellationToken = default)
        {
            List<UserModel> result = new List<UserModel>();
            var child1 = _userRepo.Get(x => x.ReferenceId == userId).QueryTo<UserModel>().ToList();
            result.AddRange(child1);

            // child2
            var ids = child1.Select(x => x.Id);
            var child2 = _userRepo.Get(x => ids.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child2);

            //child3
            var id2S = child2.Select(x => x.Id);
            var child3 = _userRepo.Get(x => id2S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child3);

            //child4
            var id3S = child3.Select(x => x.Id);
            var child4 = _userRepo.Get(x => id3S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child4);


            //child5
            var id4S = child4.Select(x => x.Id);
            var child5 = _userRepo.Get(x => id4S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child5);

            //child6
            var id5S = child5.Select(x => x.Id);
            var child6 = _userRepo.Get(x => id5S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child6);

            //child7
            var id6S = child6.Select(x => x.Id);
            var child7 = _userRepo.Get(x => id6S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child7);

            //child8
            var id7S = child7.Select(x => x.Id);
            var child8 = _userRepo.Get(x => id7S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child8);

            //child9
            var id8S = child8.Select(x => x.Id);
            var child9 = _userRepo.Get(x => id8S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child9);

            //child10
            var id9S = child9.Select(x => x.Id);
            var child10 = _userRepo.Get(x => id9S.Contains(x.ReferenceId)).QueryTo<UserModel>().ToList();
            result.AddRange(child10);

            return result;
        }

        public Task UpdateCommissionBalance(string slotId, CancellationToken cancellationToken = default)
        {
            BackgroundJob.Enqueue(() => UpdateCommissionTask(slotId));
            return Task.CompletedTask;
        }

        public Task<SellTokenPriceModel> CalculatingTokenPrice(double sellTokenTokenQuantity, CancellationToken cancellationToken = default)
        {

            var config = _configRepo.Get().FirstOrDefault();
            if (config == null)
            {
                return null;
            }


            var totalTemp = sellTokenTokenQuantity * config.TokenPrice;

            var protectFee = _feeRepository.Get(x => x.From < totalTemp && x.To >= totalTemp).FirstOrDefault();
            var fee = 0d;
            if (protectFee != null)
            {
                fee = protectFee.Fee;
            }

          
            var totalFee = (0 + fee) * totalTemp / 100;
            var latestTotal = totalTemp - totalFee;

            var result = new SellTokenPriceModel()
            {
                Quantity = sellTokenTokenQuantity,
                TotalAmount = Math.Round(latestTotal, 4) ,
                FeeAmount = Math.Round(totalFee, 4),
                UnitPrice = config.TokenPrice
            };

            return Task.FromResult(result);
        }

        public Task UpdateCommissionTask(string slotId)
        {
            var config = _configRepo.Get().FirstOrDefault();
            var slot = _userSlotRepo.Get(x => x.Id == slotId).FirstOrDefault();
            if (slot == null)
            {
                return Task.CompletedTask;
            }

            if (config == null)
            {
                return Task.CompletedTask;
            }

            var user = _userRepo.Get(x => x.Id == slot.UserId).FirstOrDefault();
            //level 1
            if (string.IsNullOrWhiteSpace(user?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent1 = _userRepo.Get(x => x.Id == user.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent1, config.Level1, config.ConditionLevel1);

            //level 2
            if (string.IsNullOrWhiteSpace(parent1?.ReferenceId))
            {
                return Task.CompletedTask;
            }

            var parent2 = _userRepo.Get(x => x.Id == parent1.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent2, config.Level2, config.ConditionLevel2);

            // level 3
            if (string.IsNullOrWhiteSpace(parent2?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent3 = _userRepo.Get(x => x.Id == parent2.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent3, config.Level3, config.ConditionLevel3);

            // level 4
            if (string.IsNullOrWhiteSpace(parent3?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent4 = _userRepo.Get(x => x.Id == parent3.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent4, config.Level4, config.ConditionLevel4);

            // level 5
            if (string.IsNullOrWhiteSpace(parent4?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent5 = _userRepo.Get(x => x.Id == parent4.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent5, config.Level5, config.ConditionLevel5);

            // level 6
            if (string.IsNullOrWhiteSpace(parent5?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent6 = _userRepo.Get(x => x.Id == parent5.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent6, config.Level6, config.ConditionLevel6);

            // level 7
            if (string.IsNullOrWhiteSpace(parent6?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent7 = _userRepo.Get(x => x.Id == parent6.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent7, config.Level7, config.ConditionLevel7);

            // level 8
            if (string.IsNullOrWhiteSpace(parent7?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent8 = _userRepo.Get(x => x.Id == parent7.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent8, config.Level8, config.ConditionLevel8);


            // level 9
            if (string.IsNullOrWhiteSpace(parent8?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent9 = _userRepo.Get(x => x.Id == parent8.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent9, config.Level9, config.ConditionLevel9);

            // level 9
            if (string.IsNullOrWhiteSpace(parent9?.ReferenceId))
            {
                return Task.CompletedTask;
            }
            var parent10 = _userRepo.Get(x => x.Id == parent9.ReferenceId).FirstOrDefault();
            UpdateCommision(slot, parent10, config.Level10, config.ConditionLevel10);

            return Task.CompletedTask;
        }

        private void UpdateCommision(UserSlotsEntity slot, UserEntity parent, double configLevel, int condition)
        {
            if (slot ==null)
            {
                return;
            }

            if (parent ==null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(parent.Permission))
            {
                var pers = parent.Permission.Split(',').Select(x => (Permission) int.Parse(x)).ToList();
                if (pers.Contains(Permission.Admin))
                {
                    return;
                }
            }

            var totalSlot = _userSlotRepo.Get(x => x.UserId == parent.Id).Sum(x=>x.Quantity);
            if (totalSlot < condition)
            {
                return;
            }

            var commision = _userBusinessRepo.Get(x => x.FromUserSlotId == slot.Id && x.ToUserId == parent.Id).FirstOrDefault();

            if (commision == null)
            {
                var item = new UserBusinessEntity()
                {
                    AmountSlot = slot.Quantity,
                    AmountUSD = slot.TotalAmount,
                    Commission = slot.TotalAmount * configLevel / 100,
                    FromUserId = slot.UserId,
                    Status = Enums.BusinessStatus.Done,
                    ToUserId = parent.Id,
                    FromUserSlotId = slot.Id
                };
                _userBusinessRepo.Add(item);
                UnitOfWork.SaveChanges();
            }
            else
            {
                commision.AmountSlot = slot.Quantity;
                commision.AmountUSD = slot.TotalAmount;
                commision.Commission = slot.TotalAmount * configLevel / 100;
                _userBusinessRepo.Update(commision, x=>x.AmountUSD, x=>x.AmountSlot, x=>x.Commission);
                UnitOfWork.SaveChanges();
            }

            var task = UpdateWalletBalanceTask(parent.Id);
            task.Wait();

            // Clear Cache
            var cacheKey = _cacheKeyPrefix + parent.Id;
            _memoryCache.Remove(cacheKey);

            var cacheKey2 = "Dashboard_" + parent.Id;
            _memoryCache.Remove(cacheKey2);
        }
    }
}