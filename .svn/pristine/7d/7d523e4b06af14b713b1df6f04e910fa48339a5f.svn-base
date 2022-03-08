using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.StackAdmin.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class CommissionRateController : BaseController
    {
        private readonly IStackCommissionService _stackCommissionService;

        public CommissionRateController(IStackCommissionService stackCommissionService)
        {
            _stackCommissionService = stackCommissionService;
        }

        [HttpGet]
        [Route(StackingAdmin.StackCommissionRate.IndexEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.StackCommissionRate.AddEndpoint)]
        public IActionResult Add()
        {
            return View(new StackCommissionRateEntity());
        }

        [HttpPost]
        [Route(StackingAdmin.StackCommissionRate.AddEndpoint)]
        public async Task<IActionResult> SubmitAddAsync([FromForm] StackCommissionRateEntity model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Add", model);
            }

            var sub = await _stackCommissionService.CreateCommissionRateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return RedirectToAction("Add");
        }

        [HttpGet]
        [Route(StackingAdmin.StackCommissionRate.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var comm = await _stackCommissionService.GetCommissionRateAsync(id);

            if (comm == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Index");
            }

            return View(comm);
        }

        [HttpPost]
        [Route(StackingAdmin.StackCommissionRate.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit(StackCommissionRateEntity model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            var id = await _stackCommissionService.UpdateCommissionRateAsync(model);
            if (id == null)
            {
                return View("Edit", model);
            }

            this.SetNotification(string.Format(Messages.Subscription.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("Edit");
        }
    }
}
