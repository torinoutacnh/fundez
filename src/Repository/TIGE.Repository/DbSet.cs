using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.Application;
using TIGE.Contract.Repository.Models.User;
using Microsoft.EntityFrameworkCore;
using TIGE.Contract.Repository.Models.Stacking;

namespace TIGE.Repository
{
    public sealed partial class TigeDbContext
    {
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        //user 
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProtectionFeeEntity> ProtectionFees { get; set; }
        public DbSet<UserTempProfileEntity> TempProfiles { get; set; }
        public DbSet<UserSellTokenEntity> UserSellTokens { get; set; }
        public DbSet<UserWalletEntity> UserWallets { get; set; }
        public DbSet<UserDepositRequestEntity> UserWalletHistories { get; set; }

        //image
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<TigeHistoryEntity> TigeHistories { get; set; }
        public DbSet<UserSlotsEntity> UserSlots { get; set; }
        public DbSet<UserBusinessEntity> UserBusiness { get; set; }
        public DbSet<UserTokensEntity> UserTokens { get; set; }
        public DbSet<TokenChangesEntity> TokenChanges { get; set; }
        public DbSet<ConfigurationEntity> Configurations {get;set; }
        public DbSet<UserWithDrawRequestEntity> UserWithDrawRequests {get;set; }

        //Stacking
        public DbSet<HistoryWithdrawTokenEntity> HistoryWithdrawTokens { get; set; }
        public DbSet<HistoryWithdrawUSDEntity> HistoryWithdrawUSDs { get; set; }
        public DbSet<HistoryDepositEntity> HistoryDeposits { get; set; }
        public DbSet<StackingWalletEntity> StackingWallets { get; set; }
        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<StackingConfigEntity> StackingConfigurations { get; set; }
        public DbSet<StackHistoryEntity> StackHistories { get; set; }
        public DbSet<HistoryRefundEntity> HistoryRefunds { get; set; }
        public DbSet<TransferTokenEntity> TransferTokens { get; set; }
        public DbSet<StackCommissionEntity> StackCommissions { get; set; }
        public DbSet<StackCommissionRateEntity> StackCommissionRates { get; set; }
    }
}