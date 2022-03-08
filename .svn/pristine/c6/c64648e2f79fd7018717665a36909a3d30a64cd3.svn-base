#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> UserModel.cs </Name>
//         <Created> 20/04/2018 5:32:41 PM </Created>
//         <Key> 9270b16c-da24-4442-a03f-bd12b3653bc2 </Key>
//     </File>
//     <Summary>
//         UserModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models.User
{
    public class CreateUserModel
    {
        [Remote("CheckUniqueEmail", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The email is already exist, please try another.")]
        [DataTable(DisplayName = "Email", Order = 4)]
        public string Email { get; set; }
        
        [DataTableIgnore]
        [Display(Name = "Permissions")]
        public List<Permission> ListPermission { get; set; } = new List<Permission>();
        
        [Display(Name = "Full Name")]
        [DataTable(DisplayName = "Full Name", Order = 5)]
        public string FullName { get; set; }
    }

    public class RegisterUserModel
    {
        [Remote("CheckUniqueEmail", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The email is already exist, please try another.")]
        [DataTable(DisplayName = "Email", Order = 4)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataTable(DisplayName = "Password", Order = 5)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataTable(DisplayName = "Confirm Password", Order = 6)]
        public string ConfirmPassword { get; set; }
        public string Refer { get; set; }
        public string Code { get; set; }
    }
    
    public class UpdateProfileModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string AboutMe { get; set; }

        [Remote("CheckUniqueAddress", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The address is already exist, please try another.")]
        [DataTable(DisplayName = "Address", Order = 3, IsVisible = false, IsSortable = true, IsSearchable = true)]
        public string Address { get; set; }

        public string Password { get; set; }

        public Enums.Gender Gender { get; set; }
        public bool Enable2FA { get; set; }
    }

    public class UpdateStackProfileModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string AboutMe { get; set; }

        [Remote("CheckUniqueAddress", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The address is already exist, please try another.")]
        [DataTable(DisplayName = "Address", Order = 3, IsVisible = false, IsSortable = true, IsSearchable = true)]
        public string Address { get; set; }

        public string Password { get; set; }

        public Enums.Gender Gender { get; set; }
        public bool Enable2FA { get; set; }
    }

    public class UserModel : CreateUserModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public string Id { get; set; }

        [Remote("CheckUniquePhone", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The phone is already exist, please try another.")]
        [DataTable(DisplayName = "Phone", Order = 3, IsVisible = false)]
        public string Phone { get; set; }

        [DataTableIgnore]
        public int AuthyId { get; set; }

        [Display(Name = "Banned")]
        [DataTableIgnore]
        public bool IsBanned { get; set; }

        [Display(Name = "Banned At")]
        [DataTable(DisplayName = "Banned At", Order = 6)]
        public DateTimeOffset? BannedTime { get; set; }

        [Display(Name = "Banned Remark")]
        [DataTableIgnore]
        public string BannedRemark { get; set; }
        
        [Display(Name = "Phone Confirmed At")]
        [DataTableIgnore]
        public DateTimeOffset? PhoneConfirmedTime { get; set; }

        [Display(Name = "Email Confirmed At")]
        [DataTableIgnore]
        public DateTimeOffset? EmailConfirmedTime { get; set; }

        [DataTable(DisplayName = "Address", Order = 800, IsSearchable = true, IsVisible = true)]
        public string Address { get; set; }

        [DataTable(DisplayName = "About Me", Order = 801, IsSearchable = false, IsVisible = false)]
        public string AboutMe { get; set; }

        [Display(Name = "Deposit")]
        [DataTable(DisplayName = "Deposit", Order = 802, IsSearchable = false, IsVisible = true)]
        public string TotalDeposit { get; set; }

        [Display(Name = "WithdrawUSD")]
        [DataTable(DisplayName = "WithdrawUSD", Order = 803, IsSearchable = false, IsVisible = true)]
        public string TotalWithdraw { get; set; }

        [Display(Name = "Total Fee(USD)")]
        [DataTable(DisplayName = "Total Fee(USD)", Order = 803, IsSearchable = false, IsVisible = true)]
        public string TotalFee { get; set; }

        [Display(Name = "WithdrawTige")]
        [DataTable(DisplayName = "WithdrawTige", Order = 803, IsSearchable = false, IsVisible = true)]
        public string TotalWithdrawTige { get; set; }

        [Display(Name = "Total Fee(Tige)")]
        [DataTable(DisplayName = "Total Fee(Tige)", Order = 803, IsSearchable = false, IsVisible = true)]
        public string TotalFeeTige { get; set; }

        [Display(Name = "Commision")]
        [DataTable(DisplayName = "Commision", Order = 804, IsSearchable = false, IsVisible = true)]
        public string TotalCommision { get; set; } 
        
        [Display(Name = "Referred ID")]
        [DataTable(DisplayName = "Referred ID", Order = 805, IsSearchable = false, IsVisible = true)]
        public string ReferredID { get; set; }

        [Display(Name = "Slot")]
        [DataTable(DisplayName = "Slot", Order = 805, IsSearchable = false, IsVisible = true)]
        public string TotalSlot { get; set; }

        [Display(Name = "Token")]
        [DataTable(DisplayName = "Token", Order = 800, IsSearchable = false, IsVisible = true)]
        public string TotalToken { get; set; }

        [Display(Name = "Balance")]
        [DataTable(DisplayName = "Balance", Order = 800, IsSearchable = false, IsVisible = true)]
        public string Balance { get; set; }

        [Display(Name = "Created By")]
        [DataTableIgnore]
        public string CreatedBy { get; set; }

        [Display(Name = "Updated By")]
        [DataTableIgnore]
        public string LastUpdatedBy { get; set; }

        [Display(Name = "Created At")]
        [DataTable(DisplayName = "Created At", Order = 999, IsVisible = false, IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }

        [Display(Name = "Updated At")]
        [DataTable(DisplayName = "Updated At", Order = 1000, SortDirection = SortDirection.Descending)]
        public DateTimeOffset LastUpdatedTime { get; set; }

        [DataTable(DisplayName = "App", Order = 1001, IsSearchable = false, IsVisible = false)]
        public bool IsApp { get; set; }
        
        [DataTableIgnore]
        public string Permissions { get; set; }

        [DataTable(DisplayName = "Code", Order = 700, IsSearchable = false, IsVisible = true)]
        public string Code { get; set; }

        [DataTable(DisplayName = "Enable 2FA", Order = 700, IsSearchable = false, IsVisible = true)]
        public bool Enable2FA { get; set; }

        [DataTable(DisplayName = "Wallet", Order = 1005, IsSearchable = false, IsVisible = false)]
        public string WalletId { get; set; } 
        [DataTable(DisplayName = "Wallet", Order = 1006, IsSearchable = false, IsVisible = false)]
        public double WalletBalance { get; set; }

        [DataTable(DisplayName = "Gender", Order = 1007, IsSearchable = false, IsVisible = false)]
        public Enums.Gender Gender { get; set; }
    }

    public class StackUserModel : CreateUserModel
    {
        [DataTable(IsVisible = false, Order = 0)]
        public string Id { get; set; }

        [DataTable(DisplayName = "App", IsSearchable = false, IsVisible = false)]
        public bool IsApp { get; set; }

        [Remote("CheckUniquePhone", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The phone is already exist, please try another.")]
        [DataTable(DisplayName = "Phone", Order = 3)]
        public string Phone { get; set; }

        [DataTableIgnore]
        public int AuthyId { get; set; }

        [Display(Name = "Banned")]
        [DataTableIgnore]
        public bool IsBanned { get; set; }

        [Display(Name = "Banned At")]
        [DataTable(DisplayName = "Banned At", Order = 6)]
        public DateTimeOffset? BannedTime { get; set; }

        [Display(Name = "Banned Remark")]
        [DataTableIgnore]
        public string BannedRemark { get; set; }

        [Display(Name = "Phone Confirmed At")]
        [DataTableIgnore]
        public DateTimeOffset? PhoneConfirmedTime { get; set; }

        [Display(Name = "Email Confirmed At")]
        [DataTableIgnore]
        public DateTimeOffset? EmailConfirmedTime { get; set; }

        [Display(Name = "Referred ID")]
        public string ReferredID { get; set; }

        [DataTableIgnore]
        public string Permissions { get; set; }

        [DataTable(DisplayName = "Address", IsSearchable = true)]
        public string Address { get; set; }

        [DataTable(DisplayName = "About Me", IsSearchable = false)]
        public string AboutMe { get; set; }

        [DataTable(DisplayName = "Staking Wallet Address", IsSearchable = false, IsSortable = false)]
        public string WalletAddress { get; set; }

        [DataTable(DisplayName = "Total Stakes", IsSearchable = false, IsSortable = false)]
        public string TotalStack { get; set; }

        [DataTable(DisplayName = "Staking Wallet (TIGE)", IsSearchable = false, IsSortable = false)]
        public string Balance { get; set; }

        [DataTable(DisplayName = "Total Token (TIGE)", IsSearchable = false, IsSortable = false)]
        public string OldBalance { get; set; }

        [DataTable(DisplayName = "Total Deposit (TIGE)", IsSearchable = false, IsSortable = false)]
        public string TotalDeposit { get; set; }

        [DataTable(DisplayName = "Total Transfer (TIGE)", IsSearchable = false, IsSortable = false)]
        public string TotalTransfer { get; set; }

        [DataTable(DisplayName = "Total Withdraw (TIGE)", IsSearchable = false, IsSortable = false)]
        public string TotalWithdraw { get; set; }

        [Display(Name = "Total Fee")]
        [DataTable(DisplayName = "Total Fee", IsSearchable = false, IsSortable = false)]
        public string TotalFee { get; set; }

        [DataTable(DisplayName = "Total Rewards (TIGE)", IsSearchable = false, IsSortable = false)]
        public string TotalReward { get; set; }

        [DataTable(DisplayName = "Daily Rewards (TIGE)", IsSearchable = false, IsSortable = false)]
        public string TotalDailyReward { get; set; }        
    }
}