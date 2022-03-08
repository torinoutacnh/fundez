using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserSellTokenEntity : Entity
    {
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }

        public double TokenQuantity { get; set; }
      
        public double UnitPrice { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }
        public Enums.TokenStatus Status { get; set; }

        public DateTimeOffset? ApproveTime { get; set; }
        public string ApproveBy { get; set; }
        public virtual UserEntity Approve { get; set; }

        public DateTimeOffset? ConfirmedTime { get; set; }
        public DateTimeOffset? ExpireTime { get; set; }
        public string ConfirmToken { get; set; }
    }
}