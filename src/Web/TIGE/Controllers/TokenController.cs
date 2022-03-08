using System;
using System.Linq;
using System.Threading.Tasks;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Core.EnvUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Models.Authentication;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Controllers
{
    [Route(LandingEndpoint.Token.IndexEndpoint)]
    public class TokenController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpGet]
        [Route(LandingEndpoint.Token.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Tokens = await _tokenService.GetMySellToken(LoggedInUser.Current.Id);
            ViewBag.CurrentToken = await _tokenService.GetTotalToken(LoggedInUser.Current.Id);
            return View();
        }   
        
        
        [HttpGet]
        [AllowAnonymous]
        [Route(LandingEndpoint.Token.ConfirmEndpoint)]
        public async Task<IActionResult> Confirm([FromQuery] string token)
        {
            await _tokenService.VerifySellToken(token);
            return View();
        }

        [HttpPost]
        [Route(LandingEndpoint.Token.IndexEndpoint)]
        public async Task<IActionResult> SubmitSellToken([FromForm] SubmitTokenModel model )
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification("Please check data again!", NotificationStatus.Error);
                return RedirectToAction("Index");
            }

            try
            {
                var newid = await _tokenService.SellToken(model);
                this.SetNotification("Buy Token success, please confirm via email to complete transaction.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.SellToken.ToString(), data = newid });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }

    }
}