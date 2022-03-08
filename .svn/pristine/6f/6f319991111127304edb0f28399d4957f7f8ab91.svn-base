using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Stack.Controllers
{
    [Route(StackEndPoint.StackingWallet.IndexEndpoint)]
    public class StackingWalletController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly IStackingWalletService _stackingWalletService;
        private readonly IWalletService _walletService;
        private readonly ITokenService _tokenService;
        private readonly ICommonService _commonService;

        public StackingWalletController(IStackingWalletService stackingWalletService, IWalletService walletService, ITokenService tokenService)
        {
            _stackingWalletService = stackingWalletService;
            _walletService = walletService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route(StackEndPoint.StackingWallet.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.wallet = await _stackingWalletService.GetMyWalletAsync(LoggedInUser.Current.Id);

            return View();
        }

        [HttpPost]
        [Route(StackEndPoint.StackingWallet.DepositEndpoint)]
        public async Task<IActionResult> SubmitDeposit([FromForm] CreateDepositStackingRequestModel model)
        {
            ModelState.Remove("TxHash");
            if (!ModelState.IsValid)
            {
                ViewBag.wallet = await _stackingWalletService.GetMyWalletAsync(LoggedInUser.Current.Id);
                return View("Index", model);
            }
            try
            {
                var depositId = await _stackingWalletService.SubmitDepositAsync(model);
                this.SetNotification("Send Deposit Request success, please confirm.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.DepositStacking.ToString(), data = depositId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route(StackEndPoint.StackingWallet.WithdrawEndpoint)]
        public async Task<IActionResult> Withdraw()
        {
            ViewBag.wallet = await _stackingWalletService.GetMyWalletAsync(LoggedInUser.Current.Id);
            ViewBag.WithDraw = await _stackingWalletService.GetMyWithdrawAsync(LoggedInUser.Current.Id);

            return View();
        }

        [HttpPost]
        [Route(StackEndPoint.StackingWallet.WithdrawEndpoint)]
        public async Task<IActionResult> SubmitWithdraw([FromForm] CreateWithdrawStackRequestModel model)
        {
            try
            {
                var withdrawId = await _stackingWalletService.SubmitWithdrawAsync(model);

                this.SetNotification("Send Withdraw Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.WithdrawStacking.ToString(), data = withdrawId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Withdraw");
            }
        }
               
        [HttpGet]
        [AllowAnonymous]
        [Route(StackEndPoint.StackingWallet.ConfirmDepositEndpoint)]
        public async Task<IActionResult> ConfirmDeposit(string token)
        {
            try
            {
                await _stackingWalletService.CheckConfirmDepositWithToken(token);
                return View();
            }
            catch (CoreException ex)
            {
                this.SetNotification(ex.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route(StackEndPoint.StackingWallet.TransferEndpoint)]
        public async Task<IActionResult> Transfer()
        {
            ViewBag.CurrentToken = await _tokenService.GetTotalToken(LoggedInUser.Current.Id);
            ViewBag.Transfers = await _stackingWalletService.GetTranferRequest(LoggedInUser.Current.Id);
            ViewBag.Wallet = await _stackingWalletService.GetMyWalletAsync(LoggedInUser.Current.Id);


            return View();
        }

        [HttpPost]
        [Route(StackEndPoint.StackingWallet.TransferEndpoint)]
        public async Task<IActionResult> CommitTransfer([FromForm] CreateWithdrawRequestModel model)
        {
            try
            {
                var withdrawId = await _stackingWalletService.SubmitTransferAsync(model);

                this.SetNotification("Send transfer Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.TransferTige.ToString(), data = withdrawId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Transfer");
            }
        }

        [HttpGet]
        [Route(StackEndPoint.StackingWallet.WithdrawUSDEndpoint)]
        public async Task<IActionResult> WithdrawUSD()
        {
            ViewBag.wallet = await _stackingWalletService.GetMyWalletAsync(LoggedInUser.Current.Id);
            ViewBag.WithDraw = await _stackingWalletService.GetMyWithdrawUSDAsync(LoggedInUser.Current.Id);

            return View();
        }

        [HttpPost]
        [Route(StackEndPoint.StackingWallet.ConfirmWithdrawUSDEndpoint)]
        public async Task<IActionResult> SubmitWithdrawUSD([FromForm] CreateWithdrawStackRequestModel model)
        {
            try
            {
                var withdrawId = await _stackingWalletService.SubmitWithdrawUSDAsync(model);

                this.SetNotification("Send Withdraw Request success, please confirm request in your email.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.WithdrawUSDStack.ToString(), data = withdrawId });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("WithdrawUSD");
            }
        }
    }
}
