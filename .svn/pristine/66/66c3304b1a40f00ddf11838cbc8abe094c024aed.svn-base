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
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Controllers
{
    [Route(LandingEndpoint.Wallet.IndexEndpoint)]
    public class WalletController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly IWalletService _walletService;
        private readonly ITokenService _tokenService;

        public WalletController(IWalletService walletService, ITokenService tokenService)
        {
            _walletService = walletService;
            _tokenService = tokenService;
        }


        [HttpGet]
        [Route(LandingEndpoint.Wallet.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.wallet = await _walletService.GetMyWalletAsync(LoggedInUser.Current.Id);

            return View();
        }

        [HttpGet]
        [Route(LandingEndpoint.Wallet.WithdrawEndpoint)]
        public async Task<IActionResult> Withdraw()
        {
            ViewBag.wallet = await _walletService.GetMyWalletAsync(LoggedInUser.Current.Id);
            ViewBag.WithDraw = await _walletService.GetMyWithdrawAsync(LoggedInUser.Current.Id);
            return View();
        }


        //fix
        [HttpGet]
        [Route(LandingEndpoint.Wallet.WithdrawTigeEndpoint)]
        public async Task<IActionResult> WithdrawTige()
        {
            ViewBag.WithDraw = await _walletService.GetMyWithdrawAsync(LoggedInUser.Current.Id);
            //ViewBag.WithDraw = await _tokenService.GetMySellToken(LoggedInUser.Current.Id);
            ViewBag.CurrentToken = await _tokenService.GetTotalToken(LoggedInUser.Current.Id);

            return View();
        }

        //fix
        [HttpPost]
        [Route(LandingEndpoint.Wallet.WithdrawTigeEndpoint)]
        public async Task<IActionResult> SubmitWithdrawTige([FromForm] CreateWithdrawRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new CoreException(ErrorCode.BadRequest, $"Bad request");
                }

                var tokens = await _tokenService.GetTotalToken(LoggedInUser.Current.Id);
                if (model.Amount > tokens)
                {
                    throw new CoreException(ErrorCode.BadRequest, $"Please input amount less than your balance");
                }
                var withdrawId = await _walletService.SubmitWithdrawTigeAsync(model);

                this.SetNotification("Send Withdraw Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.WithDrawTige.ToString(), data = withdrawId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("WithdrawTige");
            }
        }

        [HttpPost]
        [Route(LandingEndpoint.Wallet.WithdrawEndpoint)]
        public async Task<IActionResult> SubmitWithdraw([FromForm] CreateWithdrawRequestModel model )
        {
            try
            {
                var withdrawId = await _walletService.SubmitWithdrawAsync(model);

                this.SetNotification("Send Withdraw Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.WithDraw.ToString(), data = withdrawId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Withdraw");
            }
        }
        /// <summary>
        ///     Confirm Deposit Email
        /// </summary>
        /// <param name="token">Confirm Email Token via Email</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route(LandingEndpoint.Wallet.ConfirmDepositEndpoint)]
        public async Task<IActionResult> ConfirmDeposit(string token)
        {
            try
            {
                await _walletService.CheckConfirmDepositWithToken(token);
                return View();
            }
            catch (CoreException ex)
            {
                this.SetNotification(ex.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        ///     Confirm Email
        /// </summary>
        /// <param name="token">Confirm Email Token via Email</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route(LandingEndpoint.Wallet.ConfirmWithdrawEndpoint)]
        public async Task<IActionResult> ConfirmWithdraw(string token)
        {
            try
            {
                await _walletService.CheckConfirmWithToken(token);
            }
            catch (CoreException ex)
            {
                this.SetNotification(ex.Message, NotificationStatus.Error);

                return RedirectToAction("Withdraw");
            }

            return View();
        }

        [HttpPost]
        [Route(LandingEndpoint.Wallet.DepositEndpoint)]
        public async Task<IActionResult> SubmitDeposit([FromForm] CreateDepositRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.wallet = await _walletService.GetMyWalletAsync(LoggedInUser.Current.Id);
                return View("Index", model);
            }
            try
            {
                var depositId = await _walletService.SubmitDepositAsync(model);
                this.SetNotification("Send Deposit Request success, please confirm.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.Deposit.ToString(), data = depositId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }
    }
}