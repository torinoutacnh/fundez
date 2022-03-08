using System;
using System.ComponentModel.DataAnnotations.Schema;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class HistoryDepositEntity : Entity
    {
        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public string TxHash { get; set; }
        public double Rate { get; set; }
        public string ConfirmToken { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public DateTimeOffset? ConfirmedTime { get; set; }
        public DateTimeOffset DayEnd { get; set; }
        public Enums.StackDeposit Status { get; set; }

        [ForeignKey("WalletId")]
        public virtual StackingWalletEntity StackingWallet { get; set; }
        public string ApproveBy { get; set; }
    }
}
