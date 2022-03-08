using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserWithDrawRequestEntity : Entity
    {
        public string FromWalletId { get; set; }
        public string WalletId { get; set; }
        public virtual UserWalletEntity Wallet { get; set; }

        public double AmountETH { get; set; }
        public double AmountUSD { get; set; }
        public double FeeUSD { get; set; }
        public double Rate { get; set; }
        public string TxHash { get; set; }

        public string ApproveById { get; set; }
        public virtual UserEntity ApproveBy { get; set; }
        public DateTimeOffset? ApproveTime { get; set; }

        public string RejectById { get; set; }
        public virtual UserEntity RejectBy { get; set; }
        public string RejectReason { get; set; }
        public DateTimeOffset? RejectTime { get; set; }


        public DateTimeOffset? ConfirmedTime { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public string ConfirmToken { get; set; }

        public Enums.WithdrawStatus Status { get; set; }
    }
}