using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class UserEntity : Entity
    {
        // Password

        public string PasswordHash { get; set; }

        public DateTimeOffset? PasswordLastUpdatedTime { get; set; }

        // Phone

        public string Phone { get; set; }

        public DateTimeOffset? PhoneConfirmedTime { get; set; }


        // Email

        public string Email { get; set; }
        public DateTimeOffset? EmailConfirmedTime { get; set; }

        public string ConfirmEmailToken { get; set; }

        public DateTimeOffset? ConfirmEmailTokenExpireTime { get; set; }

        // Set Password

        public string SetPasswordToken { get; set; }

        public DateTimeOffset? SetPasswordTokenExpireTime { get; set; }

        // Ban

        public DateTimeOffset? BannedTime { get; set; }

        public string BannedRemark { get; set; }

        public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
        // Application In Charge

        // Permission

        public string Permission { get; set; }

        public string AboutMe { get; set; }
        public int AuthyId { get; set; }
        public bool Enable2FA { get; set; }

        // Profile

        public string Code { get; set; } = StringHelper.Generate(4, true, false, false);

        public string AvatarUrl { get; set; }

        public string IdentityCardNo { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }
        public Enums.Gender Gender { get; set; }

        public virtual ICollection<UserWalletEntity> Wallets { get; set; }

        public string ReferenceId { get; set; }

        public virtual UserEntity Reference { get; set; }

        public int Slots { get; set; }
        public double Tokens { get; set; }

        public virtual ICollection<UserSlotsEntity> SlotHistories { get; set; }
        public virtual ICollection<UserTokensEntity> TokensHistories { get; set; }
        public virtual ICollection<UserSlotsEntity> ApproveSlots { get; set; }

        public virtual ICollection<UserWithDrawRequestEntity> ApproveRequest { get; set; }
        public virtual ICollection<UserTempProfileEntity> TempProfiles { get; set; }
        public virtual ICollection<UserWithDrawRequestEntity> RejectRequest { get; set; }
        public virtual ICollection<UserBusinessEntity> FromBusinessUsers { get; set; }
        public virtual ICollection<UserBusinessEntity> ToBusinessUsers { get; set; }
        public virtual ICollection<UserSellTokenEntity> UserSellTokens { get; set; }
    }
}