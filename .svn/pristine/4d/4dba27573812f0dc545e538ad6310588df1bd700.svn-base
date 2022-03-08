using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserDepositRequestEntity : Entity
    {
        public string WalletId { get; set; }
        public virtual UserWalletEntity Wallet { get; set; }
        public double AmountETH { get; set; }
        public double AmountUSD { get; set; }
        public double Rate { get; set; }
        public string TxHash { get; set; }
        public string ApproveBy { get; set; }
        public string ConfirmToken { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public DateTimeOffset? ConfirmedTime { get; set; }

        public virtual UserEntity Approve { get; set; }
        public Enums.WalletDepositStatus Status { get; set; }
    }
}