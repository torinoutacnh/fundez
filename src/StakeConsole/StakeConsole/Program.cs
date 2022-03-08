using Elect.Data.EF.Interfaces.DbContext;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Core.Share.Constants;
using TIGE.Repository;

namespace StakeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddTigeDbContext()
                .AddMemoryCache()
                .AddScoped<IRepository<HistoryRefundEntity>, Repository<HistoryRefundEntity>>()
                .AddScoped<IRepository<StackHistoryEntity>, Repository<StackHistoryEntity>>()
                .AddScoped<IRepository<StackingWalletEntity>, Repository<StackingWalletEntity>>()
                .AddScoped<IRepository<UserWalletEntity>, Repository<UserWalletEntity>>()
                .AddScoped<IRepository<StackCommissionEntity>, Repository<StackCommissionEntity>>()
                .AddScoped<IRepository<StackCommissionRateEntity>, Repository<StackCommissionRateEntity>>()
                .AddScoped<IRepository<UserEntity>, Repository<UserEntity>>()
                .AddScoped<IRepository<UserSlotsEntity>, Repository<UserSlotsEntity>>()
                .BuildServiceProvider();
            //configure console logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            //do the actual work here
            string _cacheKeyPrefix = "User_";
            string _dashboardCacheKey = "StackDashboard_";

            Log.Logger.Information($"Program start at {DateTimeOffset.Now}");

            //using (var db = serviceProvider.GetRequiredService<IDbContext>())
            //{
            //    var cache = serviceProvider.GetRequiredService<IMemoryCache>();
            //    var rewards = serviceProvider.GetRequiredService<IRepository<HistoryRefundEntity>>();
            //    var Stakcs = serviceProvider.GetRequiredService<IRepository<StackHistoryEntity>>();
            //    var Wallets = serviceProvider.GetRequiredService<IRepository<StackingWalletEntity>>();
            //    var Users = serviceProvider.GetRequiredService<IRepository<UserEntity>>();

            //    var stacks = Stakcs.Get(x => x.Status == Enums.StackState.Staking);
            //    var walletids = stacks.Select(x => x.WalletId).ToList();
            //    var userids = Wallets.Get(x => walletids.Contains(x.Id)).Select(x => x.UserId);

            //    Log.Logger.Information($"{DateTimeOffset.Now} - Reward section");
            //    var todayRewards = rewards.Get(x => x.CreatedTime > DateTime.Today);
            //    foreach (var stack in stacks)
            //    {
            //        if (stack.DateEnd < DateTimeOffset.Now)
            //        {
            //            stack.Status = Enums.StackState.Finished;
            //            Stakcs.Update(stack, x => x.Status);
            //        }
            //        else
            //        {
            //            var count = todayRewards.Where(x => x.FromStack == stack.Id).Count();
            //            if (count == 0)
            //            {
            //                var refund = new HistoryRefundEntity()
            //                {
            //                    WalletId = stack.WalletId,
            //                    Amount = stack.DailyReward,
            //                    Rate = 1,
            //                    FromStack = stack.Id,
            //                };
            //                rewards.Add(refund);

            //                Log.Logger.Information($"{DateTimeOffset.Now} - Id:{refund.Id} from stakeId {stack.Id}");
            //            }
            //        }
            //    }
            //    db.SaveChanges();
            //}


            using (var db = serviceProvider.GetRequiredService<IDbContext>())
            {
                var cache = serviceProvider.GetRequiredService<IMemoryCache>();
                var rewards = serviceProvider.GetRequiredService<IRepository<HistoryRefundEntity>>();
                var Stakcs = serviceProvider.GetRequiredService<IRepository<StackHistoryEntity>>();
                var Commissions = serviceProvider.GetRequiredService<IRepository<StackCommissionEntity>>();
                var CommissionRates = serviceProvider.GetRequiredService<IRepository<StackCommissionRateEntity>>();
                var Wallets = serviceProvider.GetRequiredService<IRepository<StackingWalletEntity>>();
                var MainWallets = serviceProvider.GetRequiredService<IRepository<UserWalletEntity>>();
                var Users = serviceProvider.GetRequiredService<IRepository<UserEntity>>();
                var UsersSlot = serviceProvider.GetRequiredService<IRepository<UserSlotsEntity>>();

                Log.Logger.Information($"{DateTimeOffset.Now} - Commission section");
                var todayCommission = Commissions.Get(x => x.CreatedTime > DateTime.Today);
                if (todayCommission.Any())
                {
                    return;
                }

                var todayRewards = rewards.Get(x => x.CreatedTime > DateTime.Today);
                var userids = Wallets.Get(x=> todayRewards.Select(y => y.WalletId).Contains(x.Id)).Select(x=>x.UserId).ToList();

                foreach(var reward in todayRewards)
                {
                    var loop1wallet = Wallets.Get(x => x.Id == reward.WalletId).FirstOrDefault();
                    var userref = Users.Get(x => x.Id == loop1wallet.UserId).FirstOrDefault().ReferenceId;
                    if(userref == null)
                    {
                        continue;
                    }
                    foreach(var commissionrate in CommissionRates.Get().OrderBy(x => x.Level))
                    {
                        if (userref == null)
                        {
                            continue;
                        }
                        var qualified = UsersSlot.Get(x => x.Quantity >= commissionrate.Condition)
                        .GroupBy(x => x.UserId).Select(x => new
                        {
                            x.Key,
                            Sum = x.Sum(y => y.Quantity),
                        }).Select(x => x.Key).Contains(userref);

                        if (qualified)
                        {
                            var mainwallet = MainWallets.Get(x => x.UserId == userref).FirstOrDefault();
                            if (mainwallet == null)
                            {
                                userref = Users.Get(x=>x.Id==userref).FirstOrDefault().ReferenceId;
                                continue;
                            }
                            else
                            {
                                userids.Add(userref);
                                var wallet = Wallets.Get(x => x.UserId == userref).FirstOrDefault();
                                if (wallet == null)
                                {
                                    var newwallet = Wallets.Add(new StackingWalletEntity()
                                    {
                                        UserId = userref,
                                        WalletAddress = mainwallet.AddressWallet,
                                        Balance = 0,
                                        DailyReward = 0,
                                        TotalReward = 0,
                                    });
                                    wallet = newwallet;
                                }
                                userref = Users.Get(x => x.Id == userref).FirstOrDefault().ReferenceId;
                                var commission = Commissions.Add(new StackCommissionEntity()
                                {
                                    WalletId = wallet.Id,
                                    Amount = reward.Amount * commissionrate.Rate / 100.0,
                                    Rate = 1,
                                    FromStack = reward.FromStack,
                                    FromReward = reward.Id,
                                    Level = commissionrate.Level,
                                });
                                Log.Logger.Information($"{DateTimeOffset.Now} - Id:{commission.Id} from stakeId {reward.Id}");
                            }
                        }
                    }
                }

                db.SaveChanges();


                var cachelist = userids.AsQueryable().Distinct();
                foreach (var cacheid in cachelist)
                {
                    cache.Remove(_cacheKeyPrefix + cacheid);
                    cache.Remove(_dashboardCacheKey + cacheid);
                }
            }

            Log.Logger.Information($"Program finish at {DateTimeOffset.Now}");
        }
    }
}
