using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TIGE.Contract.Repository.Models;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class StackHistoryEntity : Entity
    {
        public string WalletId { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionDetail { get; set; }
        public double StackAmount { get; set; }
        public int Day { get; set; }
        public double Rate { get; set; }
        public double DailyReward { get; set;}
        public double TotalReward { get; set; }
        public string ConfirmToken { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public DateTimeOffset? ConfirmedTime { get; set; }
        public Enums.StackState Status { get; set; }
        public DateTimeOffset DateEnd { get; set; }

        [ForeignKey("WalletId")]
        public virtual StackingWalletEntity Wallet { get; set; }
        [ForeignKey("SubscriptionId")]
        public virtual SubscriptionEntity Subscription { get; set; }

    }
}
