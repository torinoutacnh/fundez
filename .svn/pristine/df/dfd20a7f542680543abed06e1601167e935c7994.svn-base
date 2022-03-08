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
using Elect.Web.DataTable.Attributes;
using Elect.Web.DataTable.Models.Constants;
using EnumsNET;
using Microsoft.AspNetCore.Mvc;
using Enums = TIGE.Core.Share.Constants.Enums;

namespace TIGE.Core.Share.Models.Crypto
{
    public class ApiResponseModel<T>
    {
        public T Data { get; set; }
    }

    public class RateResponseModel
    {
        public string Currency { get; set; }
        public RateModel Rates { get; set; }
    }

    public class RateModel
    {
        public string ETH { get; set; }
    }

    public class WalletModel
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public double Balance { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public List<WalletDepositModel> Deposits { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

    }

    public class WalletDepositModel
    {
        public string Id { get; set; }

       
        public string TxHash { get; set; }
        public double AmountETH { get; set; }
        public double AmountUSD { get; set; }
        public double Rate { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public Enums.WalletDepositStatus Status { get; set; }

        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset Time { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
    }

    
    public class StackDepositModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Email { get; set; }

        public string WalletId { get; set; }
        public double AmountTige { get; set; }
        public string TxHash { get; set; }
        public double Rate { get; set; }
        //public double EveryDayRefund { get; set; }
        //public double TotalRefund { get; set; }
        public string ConfirmToken { get; set; }
        //public DateTimeOffset DayEnd { get; set; }
        public Enums.StackDeposit Status { get; set; }

        [DataTable(IsSortable = true, SortDirection = SortDirection.Descending)]
        public DateTimeOffset CreatedTime { get; set; }
    }

}