using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models.Configuration;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Stack;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.StackAdmin.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class StackSubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IStackConfigService _stackconfigService;

        public StackSubscriptionController(ISubscriptionService subscriptionService, IStackConfigService stackconfigService)
        {
            _subscriptionService = subscriptionService;
            _stackconfigService = stackconfigService;
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.IndexEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.AddEndpoint)]
        public IActionResult Add()
        {
            return View(new SubscriptionEntity());
        }

        [HttpPost]
        [Route(StackingAdmin.Subscription.AddEndpoint)]
        public async Task<IActionResult> SubmitAddAsync([FromForm] SubscriptionEntity model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Add", model);
            }

            var sub = await _subscriptionService.CreateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return RedirectToAction("Add");
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var subscription = await _subscriptionService.GetSubscriptionModel(id);

            if (subscription == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Index");
            }

            return View(subscription);
        }

        [HttpPost]
        [Route(StackingAdmin.Subscription.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit(SubscriptionModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            var id = await _subscriptionService.UpdateSubscriptionDetail(model);
            if (id == null)
            {
                return View("Edit", model);
            }

            this.SetNotification(string.Format(Messages.Subscription.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("Edit");
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.ConfigEndpoint)]
        public IActionResult Config()
        {
            var config = _stackconfigService.GetEditConfig();
            return View(config);
        }
        [HttpPost]
        [Route(StackingAdmin.Subscription.ConfigEndpoint)]
        public async Task<IActionResult> UpdateConfig(StackConfigurationModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            await _stackconfigService.UpdateAsync(model);

            this.SetNotification(string.Format(Messages.Subscription.UpdatedConfigFormat), NotificationStatus.Success);

            return RedirectToAction("Config");
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.RefundEndpoint)]
        public IActionResult GetReward()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Subscription.SubListEndpoint)]
        public IActionResult GetSubscriptionList()
        {
            return View();
        }
    }
}
