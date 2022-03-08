using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TIGE.Contract.Repository.Models.Stacking
{
    public class HistoryRefundEntity : Entity
    {
        public string WalletId { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public string FromStack { get; set; }

        [ForeignKey("WalletId")]
        public virtual StackingWalletEntity StackWallet { get; set; }
    }
}
