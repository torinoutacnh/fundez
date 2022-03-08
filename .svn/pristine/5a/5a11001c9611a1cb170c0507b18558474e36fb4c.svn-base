using System;
using System.ComponentModel;
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Models
{
    public class CreateWithdrawRequestModel
    {
        public string WalletAddress { get; set; }
        public double Amount { get; set; }
    }


    public class CreateDepositRequestModel
    {
        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        public string TxHash { get; set; }
    }

    public class PriceRequestModel : CreateWithdrawRequestModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public string Id { get; set; }
    }

    public class WithdrawRequestModel
    {
        public string Id { get; set; }
        //fix
        public string FromWalletId { get; set; }
        public string ToAddressWallet { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public double AmountETH { get; set; }
        public double AmountUSD { get; set; }
        public double FeeUSD { get; set; }
        public double Rate { get; set; }

        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        public string TxHash { get; set; }

        //public string ApproveById { get; set; }
        //public DateTimeOffset? ApproveTime { get; set; }

        //public string RejectById { get; set; }
        //public string RejectReason { get; set; }
        //public DateTimeOffset? RejectTime { get; set; }

        //public DateTimeOffset? ConfirmedTime { get; set; }
        //public DateTimeOffset? ExpireTime { get; set; }
        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
        //public string ConfirmToken { get; set; }

        public Enums.WithdrawStatus Status { get; set; }
    }
    public class CreateWithdrawStackRequestModel
    {
        public double Amount { get; set; }
        public string WalletAddress { get; set; }
        public string TxHash { get; set; }
    }
    public class CreateDepositStackingRequestModel
    {
        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        public double Amount { get; set; }
        public string TxHash { get; set; }
    }
    
    public class StackWithdrawModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public double FeeTige { get; set; }
        public double Rate { get; set; }
        public string ToWalletAddress { get; set; }
        public string ApproveBy { get; set; }
        public string ConfirmToken { get; set; }
        public Enums.StackWithdraw Status { get; set; }

        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        public string TxHash { get; set; }

        
        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
    }

    public class TransferRequestModel
    {
        public string Id { get; set; }
        //fix
        //public string FromWalletId { get; set; }
        //public string ToAddressWallet { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        //public double AmountETH { get; set; }
        public double AmountTige { get; set; }
        //public double FeeUSD { get; set; }
        public double Rate { get; set; }

        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        //public string TxHash { get; set; }

        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
        //public string ConfirmToken { get; set; }

        public Enums.WithdrawStatus Status { get; set; }
    }

    public class StackWithdrawUSDModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public double TigePrice { get; set; }
        public double AmountUSD { get; set; }
        public double ConvertFeeUSD { get; set; }
        public double FeeUSD { get; set; }
        public double Rate { get; set; }
        public string ToWalletAddress { get; set; }
        public string ApproveBy { get; set; }
        public string ConfirmToken { get; set; }
        public Enums.StackWithdrawUSD Status { get; set; }


        //[Remote("CheckUniqueTxHash", "Auth", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The Tx Hash is already exist, please try another.")]
        public string TxHash { get; set; }


        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
    }
}