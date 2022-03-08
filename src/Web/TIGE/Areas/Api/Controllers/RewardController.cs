using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TIGE.Contract.Service;

namespace TIGE.Areas.Api.Controllers
{
    [Area("Api")]
    [Route("api/[controller]")]
    [ApiController]
    public class RewardController : ControllerBase
    {
        private readonly IStackingWalletService _walletService;
        private readonly ISubscriptionService _subscriptionService;

        public RewardController(IStackingWalletService walletService, ISubscriptionService subscriptionService)
        {
            _walletService = walletService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> DailyReward()
        {
            await _subscriptionService.RefundToUser().ConfigureAwait(true);

            return NoContent();
        }
    }
}
