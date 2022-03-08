using System;
using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.ObjUtils;
using TIGE.Contract.Service;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class SlotController : BaseController
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        #region Main
        [HttpGet]
        [Route(PortalEndpoint.Slot.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Slot.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var Slot = await _slotService.GetByIdAsync(id, this.GetRequestCancellationToken());

            if (Slot == null)
            {
                this.SetNotification(string.Format(Messages.Slot.DoesNotExist),NotificationStatus.Error);
                return View("Index");
            }

            return View(Slot);
        }

        [HttpPost]
        [Route(PortalEndpoint.Slot.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] DetailSlotRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("Edit", model);
            }

            try
            {
                await _slotService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
                this.SetNotification(string.Format(Messages.Slot.UpdatedFormat, model.Id), NotificationStatus.Success);
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return View("Edit", model);
            }

            return RedirectToAction("Edit", new {id = model.Id});
        }

        [HttpGet]
        [Route(PortalEndpoint.Slot.ResendEndpoint)]
        public async Task<IActionResult> Resend([FromRoute] string id)
        {
            await _slotService.ResendConfirmBuyAsync(id);
            this.SetNotification("Resend buy slot confirmation mail successfully.", NotificationStatus.Success);
            return RedirectToAction("Edit", new { id = id });
        }

        #endregion
    }
}