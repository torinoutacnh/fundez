using System;
using System.Collections.Generic;
using System.Text;

namespace TIGE.Core.Constants
{
    public class StackingAdmin
    {
        public const string AreaName = "stackadmin";

        public static class Email
        {
            public const string EmailTemplateEndpoint = "~/templates/email";
        }

        public static class Admin
        {
            private const string BaseEndpoint = "~/" + AreaName;

            public const string IndexEndpoint = BaseEndpoint + "/admin";

            public const string OopsEndpoint = BaseEndpoint + "/admin/oops";

            public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";
        }

        public static class Config
        {
            private const string BaseEndpoint = "~/" + AreaName + "/configs";

            public const string IndexEndpoint = BaseEndpoint;
        }

        public static class Subscription
        {
            private const string BaseEndpoint = "~/" + AreaName + "/subscriptions";

            public const string IndexEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/Add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
            public const string RefundEndpoint = BaseEndpoint + "/refunds";
            public const string SubListEndpoint = BaseEndpoint + "/suscriptionlist";

            public const string ConfigEndpoint = BaseEndpoint + "/configs";
        }

        public static class StackCommission
        {
            private const string BaseEndpoint = "~/" + AreaName + "/commissions";

            public const string IndexEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/Add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
            
        }

        public static class StackCommissionRate
        {
            private const string BaseEndpoint = "~/" + AreaName + "/commissionrates";

            public const string IndexEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/Add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
        }

        public static class Auth
        {
            public const string SignInEndpoint = "~/signin";
            public const string SignOutEndpoint = "~/signout";
            public const string AuthorizedEndpoint = "~/authorized";
            public const string UnAuthorizeEndpoint = "~/un-authorize";
            public const string ForgetPasswordEndpoint = "~/forget-password";
            public const string SubmitForgetPasswordEndpoint = "~/forget-password";
            public const string ConfirmEmailEndpoint = "~/confirm-email";
            public const string ChangePasswordEndpoint = "~/change-password";
            public const string CheckUniqueEmailEndpoint = "~/is-unique-email";
            public const string CheckUniquePhoneEndpoint = "~/is-unique-phone";

        }

        public static class User
        {
            private const string BaseEndpoint = "~/" + AreaName + "/users";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
            public const string EditProfileEndpoint = BaseEndpoint + "/profile";
            public const string ResendEndpoint = BaseEndpoint + "/{id}/resend-confirm";
        }

        public static class Verify
        {
            private const string BaseEndpoint = "~/" + AreaName + "/verify";
            public const string IndexEndpoint = BaseEndpoint;
            public const string ResendEndpoint = BaseEndpoint + "/resend";
        }

        public static class Wallet
        {
            private const string BaseEndpoint = "~/" + AreaName + "/wallets";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string EditEndpoint = BaseEndpoint + "/{id}";

            private const string WithDrawBaseEndpoint = "~/" + AreaName + "/withdraws";
            public const string GetWithdrawPagedEndpoint = WithDrawBaseEndpoint + "";
            public const string GetWithdrawTigePagedEndpoint = WithDrawBaseEndpoint + "Tige";
            public const string EditWithdrawEndpoint = WithDrawBaseEndpoint + "/{id}";

            private const string TransferBaseEndpoint = "~/" + AreaName + "/transfers";
            public const string GetTransferPagedEndpoint = TransferBaseEndpoint + "";
            public const string EditTransferEndpoint = TransferBaseEndpoint + "/{id}";

            private const string ConvertBaseEndpoint = "~/" + AreaName + "/converts";
            public const string GetConvertPagedEndpoint = ConvertBaseEndpoint + "";
            public const string EditConvertEndpoint = ConvertBaseEndpoint + "/{id}";
        }
    }
}
