﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class HistoryWithdrawTokenEntity : Entity
    {
        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public double FeeTige { get; set; }
        public string TxHash { get; set; }
        public double Rate { get; set; }
        public string ToWalletAddress { get; set; }
        public string ConfirmToken { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public DateTimeOffset? ConfirmedTime { get; set; }
        public Enums.StackWithdraw Status { get; set; }

        [ForeignKey("WalletId")]
        public virtual StackingWalletEntity StackingWallet { get; set; }
        public string ApproveBy { get; set; }
    }
}
