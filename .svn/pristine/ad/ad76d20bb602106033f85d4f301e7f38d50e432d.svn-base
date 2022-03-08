using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Stack;

namespace TIGE.Contract.Service
{
    public interface ISubscriptionService
    {
        Task<StackingWalletEntity> GetMyWalletAsync(string id, CancellationToken cancellationToken = default);
        Task VerifyBuyStack(string token, CancellationToken cancellationToken = default);
        Task ResendConfirmStack(string data, CancellationToken cancellationToken = default);
        Task<List<SubscriptionEntity>> GetSubscriptions();
        Task<string> SubmitStackAsync(SubmitStackModel model, CancellationToken cancellationToken = default);
        StackHistoryModel GetLatestStackRequest(string userid);
        Task<List<HistoryRefundEntity>> GetRefundList(string userid, CancellationToken cancellationToken = default);
        Task RefundToUser(CancellationToken cancellationToken = default);
        Task DeleteStackAsync(string id, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<SubscriptionModel>> GetSubscriptionDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<string> UpdateSubscriptionDetail(SubscriptionModel model, CancellationToken cancellationToken = default);
        Task<SubscriptionModel> GetSubscriptionModel(string id, CancellationToken cancellationToken = default);
        Task DeleteSubscriptionAsync(string id, CancellationToken cancellationToken = default);
        Task<SubscriptionEntity> CreateAsync(SubscriptionEntity model, CancellationToken cancellationToken = default);

        Task<DataTableResponseModel<RewardModel>> GetRewardDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<StackHistoryModel>> GetStakeDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
    }
}
