using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserBusinessEntity : Entity
    {
     
        public string FromUserId { get; set; }
        public virtual UserEntity FromUser { get; set; }

        public string ToUserId { get; set; }
        public virtual UserEntity ToUser { get; set; }


        public string FromUserSlotId { get; set; }
        public virtual UserSlotsEntity FromUserSlot { get; set; }

        public double AmountUSD { get; set; }
        public int AmountSlot { get; set; }
        public double Commission { get; set; }
        public Enums.BusinessStatus Status { get; set; }
    }
}