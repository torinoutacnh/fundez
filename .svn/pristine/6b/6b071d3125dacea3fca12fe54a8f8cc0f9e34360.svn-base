using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserTempProfileEntity : Entity
    {
        public string Phone { get; set; }
        public DateTimeOffset? PhoneConfirmedTime { get; set; }
        // Email
        public string Email { get; set; }
        public DateTimeOffset? EmailConfirmedTime { get; set; }
        public string ConfirmEmailToken { get; set; }
        public DateTimeOffset? ConfirmEmailTokenExpireTime { get; set; }
        public string Password { get; set; }
        public string AboutMe { get; set; }
        public string AvatarUrl { get; set; }
        public string FullName { get; set; }
        public Enums.Gender Gender { get; set; }

        public bool Enable2FA { get; set; }
        public string WalletAddress { get; set; }
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
       

    }
}