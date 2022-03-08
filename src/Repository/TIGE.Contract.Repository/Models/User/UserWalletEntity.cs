using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserWalletEntity : Entity
    {
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public string AddressWallet { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public double AmountUSD { get; set; }
        public string WebHook { get; set; }
        public string WebHookId { get; set; }
        public DateTimeOffset? LastWebHookNotify { get; set; }

        public virtual ICollection<UserDepositRequestEntity> DepositRequests { get; set; }
        public virtual ICollection<UserWithDrawRequestEntity> WithdrawRequests { get; set; }
    }
}