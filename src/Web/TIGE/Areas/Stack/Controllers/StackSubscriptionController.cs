using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models.Stack;
using TIGE.Core.Share.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using X.PagedList;

namespace TIGE.Areas.Stack.Controllers
{
    [Route(StackEndPoint.Subscription.IndexEndpoint)]
    public class StackSubscriptionController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly IStackingWalletService _stackingWalletService;
        private readonly ISubscriptionService _subscriptionService;

        public StackSubscriptionController(IStackingWalletService walletService, ISubscriptionService subscriptionService)
        {
            _stackingWalletService = walletService;
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        [Route(StackEndPoint.Subscription.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Subscriptions = await _subscriptionService.GetSubscriptions();
            ViewBag.Wallet = await _subscriptionService.GetMyWalletAsync(LoggedInUser.Current.Id);
            return View();
        }

        [HttpPost]
        [Route(StackEndPoint.Subscription.ConfirmEndpoint)]
        public async Task<IActionResult> SubmitStacking([FromForm] SubmitStackModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Common.SomethingWentWrong);
            }
            try
            {
                var stackid = await _subscriptionService.SubmitStackAsync(model);

                this.SetNotification("Send Stack Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.Stack.ToString(), data = stackid });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Index", "StackSubscription");
            }
        }

        [HttpGet]
        [Route(StackEndPoint.Subscription.RewardEndpoint)]
        public async Task<IActionResult> Reward(int? size,int? page)
        {
            //ViewBag.Refund = await _subscriptionService.GetRefundList(LoggedInUser.Current.Id);
            var refunds = await _subscriptionService.GetRefundList(LoggedInUser.Current.Id);
            int pageNumber = (page ?? 1);
            int pagesize = (size ?? 25);
            //ViewBag.Wallet = await _subscriptionService.GetMyWalletAsync(LoggedInUser.Current.Id);
            return View(refunds.ToPagedList(pageNumber, pagesize));
        }

    }
}
