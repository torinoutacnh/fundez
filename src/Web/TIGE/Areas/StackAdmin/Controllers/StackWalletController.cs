using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.StackAdmin.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class StackWalletController : BaseController
    {
        private readonly IStackingWalletService _stackwalletService;

        public StackWalletController(IStackingWalletService stackingWalletService)
        {
            _stackwalletService = stackingWalletService;
        }

        #region Deposit
        [HttpGet]
        [Route(StackingAdmin.Wallet.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Wallet.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var wallet = await _stackwalletService.GetByDepositByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Index");
            }

            return View(wallet);
        }

        [HttpPost]
        [Route(StackingAdmin.Wallet.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] StackDepositModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            await _stackwalletService.UpdateDepositAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("Edit");
        }

        #endregion


        #region Withdraw
        [HttpGet]
        [Route(StackingAdmin.Wallet.GetWithdrawPagedEndpoint)]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Wallet.EditWithdrawEndpoint)]
        public async Task<IActionResult> EditWithdraw(string id)
        {
            var wallet = await _stackwalletService.GetByWithdrawByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Withdraw");
            }

            return View(wallet);
        }

        [HttpPost]
        [Route(StackingAdmin.Wallet.EditWithdrawEndpoint)]
        public async Task<IActionResult> SubmitEditWithdraw([FromForm] StackWithdrawModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("EditWithdraw", model);
            }

            await _stackwalletService.UpdateWithdrawAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("EditWithdraw");
        }

        #endregion


        #region Transfer
        [HttpGet]
        [Route(StackingAdmin.Wallet.GetTransferPagedEndpoint)]
        public IActionResult Transfer()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Wallet.EditTransferEndpoint)]
        public async Task<IActionResult> EditTransfer(string id)
        {
            var wallet = await _stackwalletService.GetByTransferByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Transfer");
            }

            return View(wallet);
        }

        [HttpPost]
        [Route(StackingAdmin.Wallet.EditTransferEndpoint)]
        public async Task<IActionResult> SubmitEditTransfer([FromForm] WithdrawRequestModel model)
        {
            ModelState.Remove("TxHash");
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("EditTransfer", model);
            }

            await _stackwalletService.UpdateTransferAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("EditTransfer");
        }

        #endregion

        #region Convert
        [HttpGet]
        [Route(StackingAdmin.Wallet.GetConvertPagedEndpoint)]
        public IActionResult Convert()
        {
            return View();
        }

        [HttpGet]
        [Route(StackingAdmin.Wallet.EditConvertEndpoint)]
        public async Task<IActionResult> EditConvert(string id)
        {
            var wallet = await _stackwalletService.GetConvertByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Convert");
            }

            return View(wallet);
        }

        [HttpPost]
        [Route(StackingAdmin.Wallet.EditConvertEndpoint)]
        public async Task<IActionResult> SubmitEditConvert([FromForm] StackWithdrawUSDModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("EditConvert", model);
            }

            await _stackwalletService.UpdateWithdrawUSDAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("EditConvert");
        }
        #endregion
    }
}
