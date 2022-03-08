using System;
using System.Collections.Generic;
using System.Text;

namespace TIGE.Core.Constants
{
    public class StackAdmin
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

        public static class Slot
        {
            private const string BaseEndpoint = "~/" + AreaName + "/slots";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
            public const string ResendEndpoint = BaseEndpoint + "/{id}/resend-confirm";
        }

        public static class Token
        {
            private const string BaseEndpoint = "~/" + AreaName + "/tokens";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
        }

        public static class ProtectionFee
        {
            private const string BaseEndpoint = "~/" + AreaName + "/ProtectionFees";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
        }

        public static class Business
        {
            private const string BaseEndpoint = "~/" + AreaName + "/business";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
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
        }

        public static class Image
        {
            private const string BaseEndpoint = "~/" + AreaName + "/images";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string AddEndpoint = BaseEndpoint + "/add";
            public const string EditEndpoint = BaseEndpoint + "/{id}";
            public const string DownloadEndpoint = "~/download/{id}";
        }
    }
}
