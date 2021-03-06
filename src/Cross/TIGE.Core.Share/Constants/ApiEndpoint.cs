namespace TIGE.Core.Share.Constants
{
    public static class ApiEndpoint
    {
        public const string AreaName = "api";

        public static class Auth
        {
            private const string BaseEndpoint = "~/" + AreaName + "/auth";
            public const string TokenEndpoint = BaseEndpoint + "/token";
        }

        public static class Subscription
        {
            private const string BaseEndpoint = "~/" + AreaName + "/subscriptions";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
            public const string RewardEndpoint = BaseEndpoint + "/reward";
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";

            public const string GetRewardDataTableEndpoint = BaseEndpoint + "/rewarddata";
            public const string GetStackHistoryTableEndpoint = BaseEndpoint + "/stakehistory";
        }

        public static class Commission
        {
            private const string BaseEndpoint = "~/" + AreaName + "/commissions";
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";

            private const string RateBaseEndpoint = "~/" + AreaName + "/commissionrates";
            public const string GetRateDataTableEndpoint = RateBaseEndpoint + "/datatable";
            public const string DeleteRateEndpoint = RateBaseEndpoint + "/{id}";
        }

        public static class User
        {
            private const string BaseEndpoint = "~/" + AreaName + "/users";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string GetStackDataTableEndpoint = BaseEndpoint + "/stackdatatable";
            public const string GetEndpoint = BaseEndpoint + "/{id}";
            public const string ProfileEndpoint = BaseEndpoint + "/profile";
            public const string CreateEndpoint = BaseEndpoint;
            public const string UpdateEndpoint = BaseEndpoint;
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
        }    
        
        //fix
        public static class Wallet
        {
            private const string BaseEndpoint = "~/" + AreaName + "/wallets";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string GetWithdrawDataTableEndpoint = BaseEndpoint + "/withdraws/datatable";
            public const string GetWithdrawTigeDataTableEndpoint = BaseEndpoint + "/withdraws/datatableTige";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
            public const string DeleteWithdrawEndpoint = BaseEndpoint + "/withdraw/{id}";
        }

        public static class StackWallet
        {
            private const string BaseEndpoint = "~/" + AreaName + "/stackwallets";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string GetWithdrawDataTableEndpoint = BaseEndpoint + "/withdraws/datatable";
            public const string GetWithdrawTigeDataTableEndpoint = BaseEndpoint + "/withdraws/datatableTige";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
            public const string DeleteWithdrawEndpoint = BaseEndpoint + "/withdraw/{id}";
            public const string GetWithdrawTransferDataTableEndpoint = BaseEndpoint + "/transfer/datatable";
            public const string DeleteTransferEndpoint = BaseEndpoint + "/transfer/{id}";
            public const string DeleteWithdrawUSDEndpoint = BaseEndpoint + "/withdrawusd/{id}";
            public const string GetWithdrawUSDDataTableEndpoint = BaseEndpoint + "/withdrawusds/datatable";
        }

        public static class Slot
        {
            private const string BaseEndpoint = "~/" + AreaName + "/slots";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
        }           
        
        public static class Token
        {
            private const string BaseEndpoint = "~/" + AreaName + "/tokens";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
            public const string CalculatingEndpoint = BaseEndpoint + "/cal/{quantity}";
        }       
        
        
        public static class ProtectionFee
        {
            private const string BaseEndpoint = "~/" + AreaName + "/ProtectionFees";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
        }
        public static class Business
        {
            private const string BaseEndpoint = "~/" + AreaName + "/business";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
        }

        public static class Image
        {
            private const string BaseEndpoint = "~/" + AreaName + "/images";
            public const string GetPagedEndpoint = BaseEndpoint;
            public const string GetEndpoint = BaseEndpoint + "/{id}";
            public const string GetDataTableEndpoint = BaseEndpoint + "/datatable";
            public const string UploadBase64Endpoint = BaseEndpoint + "/base64";
            public const string UploadMultiPartEndpoint = BaseEndpoint + "/multi-part";
            public const string DeleteEndpoint = BaseEndpoint + "/{id}";
        }
    }
}