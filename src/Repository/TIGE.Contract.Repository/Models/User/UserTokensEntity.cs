using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserTokensEntity : Entity
    {
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }

        public string SlotId { get; set; }
        public virtual UserSlotsEntity Slot { get; set; }

        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalAmount { get; set; }
    }
}