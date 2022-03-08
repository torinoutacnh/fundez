using System.Collections.Generic;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class StackingWalletEntity : Entity
    {
        public string UserId { get; set; }
        public string WalletAddress { get; set; }
        public double Balance { get; set; }
        public double TotalReward { get; set; }
        public double DailyReward { get; set; }

        public ICollection<HistoryDepositEntity> Deposits { get; set; }
        public ICollection<HistoryWithdrawTokenEntity> WithdrawTokens { get; set; }
        public ICollection<StackHistoryEntity> StackHistories { get; set; }
        public ICollection<TransferTokenEntity> TransferTokens { get; set; }
    }
}
