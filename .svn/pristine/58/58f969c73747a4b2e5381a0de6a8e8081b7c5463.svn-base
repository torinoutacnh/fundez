using System;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        #region Main
        [HttpGet]
        [Route(PortalEndpoint.Business.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Business.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var business = await _businessService.GetByIdAsync(id, this.GetRequestCancellationToken());

            if (business == null)
            {
                this.SetNotification(string.Format(Messages.Business.DoesNotExist),NotificationStatus.Error);
                return View("Index");
            }

            return View(business);
        }

        [HttpPost]
        [Route(PortalEndpoint.Business.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] BusinessDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("Edit", model);
            }

            try
            {
                await _businessService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
                this.SetNotification(string.Format(Messages.Business.UpdatedFormat, model.Id), NotificationStatus.Success);
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return View("Edit", model);
            }

            return RedirectToAction("Edit", new {id = model.Id});
        }

        #endregion
    }
}