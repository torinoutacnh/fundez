using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TIGE.Core.Share.Constants
{
    public class Enums
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum WalletDepositStatus
        {
            [Display(Name = "Pending")]
            Pending = 1,   
            [Display(Name = "Confirming")]
            Confirming = 2,
            [Display(Name = "Approved")]
            Approved = 3,
            [Display(Name = "Rejected")]
            Rejected = 4,
        }

        public enum Gender
        {
            [Display(Name = "Male")]
            Male = 0,

            [Display(Name = "Female")]
            Female = 1,

            [Display(Name = "Other")]
            Other = 2
        }

        public enum WithdrawStatus
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirming")]
            Confirming = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,

            [Display(Name = "New")]
            TokenNew = 4,

            [Display(Name = "Confirming")]
            TokenConfirming = 5,

            [Display(Name = "Approved")]
            TokenApproved = 6,

            [Display(Name = "Reject")]
            TokenReject = 7,

            [Display(Name = "New")]
            NewTransfer = 8,

            [Display(Name = "Confirming")]
            ConfirmingTransfer = 9,

            [Display(Name = "Approved")]
            ApprovedTransfer = 10,

            [Display(Name = "Reject")]
            RejectTransfer = 11,
        }

        public enum SlotStatus
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirmed = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }

        public enum TokenStatus
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirmed = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }


        [JsonConverter(typeof(StringEnumConverter))]
        public enum WalletHistoryAction
        {
            [Display(Name = "Deposit")]
            Deposit = 1,
            
            [Display(Name = "Withdraw")]
            Withdraw = 2,
        }

        public enum BusinessStatus
        {
            [Display(Name = "Pending")]
            Pending = 1,

            [Display(Name = "Done")]
            Done = 2,
        }


        public enum VerifyType
        {
            Register,
            UpdateProfile,
            Deposit,
            WithDraw,
            //fix
            WithDrawTige,
            BuySlot,
            SellToken,
            ForgetPassword,

            DepositStacking,
            WithdrawStacking,
            Stack,
            TransferTige,
            UpdateStackProfile,
            WithdrawUSDStack,
        }

        public enum StackDeposit
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirming = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }
        public enum StackWithdraw
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirming = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }

        public enum StackTransfer
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirming = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }

        public enum StackState
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Staking")]
            Staking = 1,

            [Display(Name = "Finished")]
            Finished = 2,
        }

        public enum StackWithdrawUSD
        {
            [Display(Name = "New")]
            New = 0,

            [Display(Name = "Confirmed")]
            Confirming = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Reject")]
            Reject = 3,
        }
    }
}
