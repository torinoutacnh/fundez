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
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class TokenController : BaseController
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        #region Main
        [HttpGet]
        [Route(PortalEndpoint.Token.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Token.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var Token = await _tokenService.GetByIdAsync(id, this.GetRequestCancellationToken());

            if (Token == null)
            {
                this.SetNotification(string.Format(Messages.Token.DoesNotExist), NotificationStatus.Error);
                return View("Index");
            }

            return View(Token);
        }

        [HttpPost]
        [Route(PortalEndpoint.Token.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] DetailSellTokenModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("Edit", model);
            }

            try
            {
                await _tokenService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
                this.SetNotification(string.Format(Messages.Token.UpdatedFormat, model.Id), NotificationStatus.Success);
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return View("Edit", model);
            }

            return RedirectToAction("Edit", new { id = model.Id });
        }

        #endregion
    }
}