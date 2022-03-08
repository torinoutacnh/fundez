#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> EmailTemplate.cs </Name>
//         <Created> 21/04/2018 9:30:59 PM </Created>
//         <Key> c3a28efc-23d1-40cf-9e10-2df25e1b4403 </Key>
//     </File>
//     <Summary>
//         EmailTemplate.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TIGE.Core.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailTemplate
    {
        /// <summary>
        ///     This email will be sent whenever a user signs up or admin invite a user. 
        /// </summary>
        [Display(Name = "Verify Email")]
        VerifyEmail,

        /// <summary>
        ///     This email will be sent once the user make a first success login. 
        /// </summary>
        [Display(Name = "Welcome")]
        Welcome,

        /// <summary>
        ///     This email will be sent whenever a user requests a password change. The password will
        ///     not be changed until the user follows the verification link in the email.
        /// </summary>
        [Display(Name = "Change Password")]
        ChangePassword,


        /// <summary>
        ///     This email will be sent whenever a user requests a password change. The password will
        ///     not be changed until the user follows the verification link in the email.
        /// </summary>
        [Display(Name = "WithDraw Request")]
        WithDraw,


        /// <summary>
        ///     This email will be sent whenever a user requests a password change. The password will
        ///     not be changed until the user follows the verification link in the email.
        /// </summary>
        [Display(Name = "WithDrawTige Request")]
        WithDrawTige,


        /// <summary>
        ///     This email will be sent whenever a user requests a password change. The password will
        ///     not be changed until the user follows the verification link in the email.
        /// </summary>
        [Display(Name = "deposit Request")]
        Deposit,

        /// <summary>
        ///     This email will be sent whenever a user requests a profile change. The password will
        ///     not be changed until the user follows the verification link in the email.
        /// </summary>
        [Display(Name = "Update Profile")]
        UpdateProfile,


        [Display(Name = "Buy Slot")]
        BuySlot,

        [Display(Name = "Sell Token")]
        SellToken,

        [Display(Name = "Deposit")]
        DepositStack,

        [Display(Name = "Withdraw")]
        WithdrawStack,

        [Display(Name = "Stack")]
        Stack,

        [Display(Name = "TransferTige")]
        TransferTige,

        [Display(Name = "Withdraw USD")]
        WithdrawUSDStack,
    }
}