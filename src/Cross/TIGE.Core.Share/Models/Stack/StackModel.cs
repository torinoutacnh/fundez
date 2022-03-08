using System;
using System.Collections.Generic;
using System.Text;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models.Stack
{
    public class SubmitStackModel
    {
        public double AmountTige { get; set; }
        public int Day { get; set; }
    }

    public class StackHistoryModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionDetail { get; set; }
        public double Rate { get; set; }
        public double StackAmount { get; set; }
        public double DailyReward { get; set; }
        public double TotalReward { get; set; }
        public Enums.StackState Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset DateEnd { get; set; }
    }

    public class SubscriptionModel
    {
        public string Id { get; set; }
        public int Day { get; set; }
        public double Reward { get; set; }
        public string Description { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }

    public class RewardModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public string FromStack { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }

    public class CommissionModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public double Amount { get; set; }
        public string FromStack { get; set; }
        public string FromReward { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }

    public class CommissionRateModel
    {
        public string Id { get; set; }

        public int Level { get; set; }
        public double Rate { get; set; }
        public double Condition { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
    }
}
