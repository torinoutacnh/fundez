using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Service;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Stack;
using TIGE.Utils;

namespace TIGE.Areas.Api.Controllers
{
    public class StackSubscriptionController : BaseController
    {
        private readonly IStackingWalletService _walletService;
        private readonly ISubscriptionService _subscriptionService;

        public StackSubscriptionController(IStackingWalletService walletService, ISubscriptionService subscriptionService)
        {
            _walletService = walletService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        [Route(ApiEndpoint.Subscription.RewardEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Reward user.")]
        public async Task<IActionResult> DailyReward()
        {
            await _subscriptionService.RefundToUser(this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.Subscription.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Delete stack.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _subscriptionService.DeleteSubscriptionAsync(id,this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.Subscription.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<SubscriptionModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _subscriptionService.GetSubscriptionDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.Subscription.GetRewardDataTableEndpoint)]
        public async Task<DataTableActionResult<RewardModel>> GetRewardTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _subscriptionService.GetRewardDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.Subscription.GetStackHistoryTableEndpoint)]
        public async Task<DataTableActionResult<StackHistoryModel>> GetStackHistoryTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _subscriptionService.GetStakeDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }
    }    
}
