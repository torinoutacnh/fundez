using System;
using System.Linq;
using System.Threading.Tasks;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Core.EnvUtils;
using Elect.Core.ObjUtils;
using Elect.Data.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Areas.Stack.Controllers
{
    [Route(Endpoint)]
    public class ProfileController : BaseController
    {
        public const string Endpoint = AreaName + "/profile";

        public const string OopsEndpoint = "oops";

        public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";

        private readonly IUserService _userService;
        private readonly IStackingWalletService _walletService;

        public ProfileController(IUserService userService, IStackingWalletService walletService)
        {
            _userService = userService;
            _walletService = walletService;
        }


        [HttpGet]
        [Route(Endpoint + "")]
        public async Task<IActionResult> Index()
        {
            var profile = await _userService.GetByIdAsync(LoggedInUser.Current.Id);
            var model = new UpdateStackProfileModel()
            {
                Id = profile.Id,
                Address = profile.Address,
                AboutMe = profile.AboutMe,
                Gender = profile.Gender,
                FullName = profile.FullName,
                Code = profile.Code,
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Endpoint + "/confirm")]
        public async Task<IActionResult> Confirm([FromQuery] string token)
        {
            await _userService.ConfirmUpdateStackProfileAsync(token);
            return View("ConfirmProfile");
        }

        [HttpGet]
        [Route(Endpoint + "/authy")]
        public async Task<IActionResult> ConfirmAuthy()
        {
            await _userService.ConfirmAuthy(LoggedInUser.Current.Id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] UpdateProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);
                return View("Index", model);
            }

            try
            {
                await _userService.UpdateProfileAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);
                this.SetNotification("Submit Success, Please check your email to confirm update your profile.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.UpdateStackProfile.ToString(), data = LoggedInUser.Current.Id });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return View("Index", model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Endpoint + "/code/{code}")]
        public async Task<IActionResult> GetByCodeDetail([FromRoute] string code)
        {
            var data = await _userService.GetByCodeAsync(code);
            return Ok(data);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("web-hook/authy")]
        public async Task<IActionResult> WebhookAuthy([FromBody] AuthyModel model)
        {
            var test = model.ToJsonString();
            System.IO.File.WriteAllText("testWebhook.txt", test);
            return NoContent();
        }

    }
}