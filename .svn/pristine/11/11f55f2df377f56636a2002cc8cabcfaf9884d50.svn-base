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
using TIGE.Core.Share.Models.ProtectionFee;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class ProtectionFeeController : BaseController
    {
        private readonly IProtectionFeeService _protectionFeeService;

        public ProtectionFeeController(IProtectionFeeService protectionFeeService)
        {
            _protectionFeeService = protectionFeeService;
        }

        #region Main
        [HttpGet]
        [Route(PortalEndpoint.ProtectionFee.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.ProtectionFee.AddEndpoint)]
        public IActionResult Add()
        {
            return View(new CreateProtectionFeeModel());
        }

        [HttpPost]
        [Route(PortalEndpoint.ProtectionFee.AddEndpoint)]
        public async Task<IActionResult> SubmitAdd([FromForm] CreateProtectionFeeModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Add", model);
            }

            await _protectionFeeService.AddProtectionFee(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(Messages.ProtectionFee.AddedFormat,NotificationStatus.Success);

            return RedirectToAction("Add");
        }

        [HttpGet]
        [Route(PortalEndpoint.ProtectionFee.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var ProtectionFee = await _protectionFeeService.GetByIdAsync(id, this.GetRequestCancellationToken());

            if (ProtectionFee == null)
            {
                this.SetNotification(string.Format(Messages.ProtectionFee.DoesNotExist),NotificationStatus.Error);
                return View("Index");
            }

            return View(ProtectionFee);
        }

        [HttpPost]
        [Route(PortalEndpoint.ProtectionFee.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] DetailProtectionFeeModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("Edit", model);
            }

            try
            {
                await _protectionFeeService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
                this.SetNotification(string.Format(Messages.ProtectionFee.UpdatedFormat, model.Id), NotificationStatus.Success);
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