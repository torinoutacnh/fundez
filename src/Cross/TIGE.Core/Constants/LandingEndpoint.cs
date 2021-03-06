#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> Endpoint.cs </Name>
//         <Created> 21/04/2018 10:15:45 PM </Created>
//         <Key> c89acce9-36da-48f1-9260-303a31f98276 </Key>
//     </File>
//     <Summary>
//         Endpoint.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

namespace TIGE.Core.Constants
{
    public class LandingEndpoint
    {
        public const string AreaName = "";

        public static class Home
        {
            private const string BaseEndpoint = "~/" + AreaName;

            public const string IndexEndpoint = BaseEndpoint;

            public const string OopsEndpoint = BaseEndpoint + "/oops";

            public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";
        }

        public static class Wallet
        {
            private const string BaseEndpoint = "~/" + AreaName + "/wallets";

            public const string IndexEndpoint = BaseEndpoint;

            public const string WithdrawEndpoint = BaseEndpoint + "/withdraw";
            public const string DepositEndpoint = BaseEndpoint + "/deposit";
            public const string ConfirmWithdrawEndpoint = BaseEndpoint + "/withdraw/confirm";
            //fix
            public const string ConfirmWithdrawTigeEndpoint = BaseEndpoint + "/withdrawtige/confirm";
            public const string ConfirmDepositEndpoint = BaseEndpoint + "/deposit/confirm";


            public const string WithdrawTigeEndpoint = BaseEndpoint + "/withdrawtige";
        }

        //public static class StackingWallet
        //{
        //    private const string BaseEndpoint = "~/" + AreaName + "/stackingwallets";

        //    public const string IndexEndpoint = BaseEndpoint;

        //    public const string WithdrawEndpoint = BaseEndpoint + "/withdraw";
        //    public const string DepositEndpoint = BaseEndpoint + "/deposit";
        //    public const string ConfirmWithdrawEndpoint = BaseEndpoint + "/withdraw/confirm";
        //    public const string ConfirmDepositEndpoint = BaseEndpoint + "/deposit/confirm";

        //    public const string WithdrawTigeEndpoint = BaseEndpoint + "/withdrawtige";
        //}

        public static class Business
        {
            private const string BaseEndpoint = "~/" + AreaName + "/business";

            public const string IndexEndpoint = BaseEndpoint;

           

        }

        public static class Slot
        {
            private const string BaseEndpoint = "~/" + AreaName + "/slots";

            public const string IndexEndpoint = BaseEndpoint;
            public const string ConfirmEndpoint = BaseEndpoint+ "/confirm";
        }
        public static class Token
        {
            private const string BaseEndpoint = "~/" + AreaName + "/tokens";

            public const string IndexEndpoint = BaseEndpoint;
            public const string ConfirmEndpoint = BaseEndpoint+ "/confirm";
        }
        //public static class Stack
        //{
        //    private const string BaseEndpoint = "~/" + AreaName + "/stacks";

        //    public const string IndexEndpoint = BaseEndpoint;
        //    public const string ConfirmEndpoint = BaseEndpoint + "/confirm";
        //    public const string RewardEndpoint = BaseEndpoint + "/reward";
        //}



        public static class Auth
        {
            private const string BaseEndpoint = "~/" + AreaName + "/accounts";

            public const string IndexEndpoint = BaseEndpoint;

            public const string SignInEndpoint = BaseEndpoint + "/signin";
            public const string SignInWithCodeEndpoint = BaseEndpoint + "/signin-with-code";
            public const string SignUpEndpoint = BaseEndpoint + "/signup";
            public const string SignOutEndpoint = BaseEndpoint + "/signout";
            public const string ForgetPasswordEndpoint = BaseEndpoint + "/forget-password";
            public const string ConfirmEmailEndpoint = BaseEndpoint + "/confirm-email";
            public const string ChangePasswordEndpoint = BaseEndpoint + "/change-password";
            public const string CheckUniqueEmailEndpoint = BaseEndpoint + "/is-unique-email";
            public const string CheckUniquePhoneEndpoint = BaseEndpoint + "/is-unique-phone";
            public const string CheckUniqueAddressEndpoint = BaseEndpoint + "/is-unique-address";
            public const string CheckUniqueUserNameEndpoint = BaseEndpoint + "/is-unique-username";
            public const string CheckUniqueTxHashEndpoint = BaseEndpoint + "/is-unique-tx-hash";
            public const string AuthorizedEndpoint = BaseEndpoint + "/authorized";
            public const string UnAuthorizeEndpoint = BaseEndpoint + "/un-authorize";
        }
    }
}