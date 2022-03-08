using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class HistoryWithdrawUSDEntity : Entity
    {
        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public double TigePrice { get; set; }
        public double AmountUSD { get; set; }
        public double ConvertFeeUSD { get; set; }
        public double FeeUSD { get; set; }
        public string TxHash { get; set; }
        public double Rate { get; set; }
        public string ToWalletAddress { get; set; }
        public string ConfirmToken { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public DateTimeOffset? ConfirmedTime { get; set; }
        public Enums.StackWithdrawUSD Status { get; set; }

        [ForeignKey("WalletId")]
        public virtual StackingWalletEntity StackingWallet { get; set; }
        public string ApproveBy { get; set; }
    }
}
