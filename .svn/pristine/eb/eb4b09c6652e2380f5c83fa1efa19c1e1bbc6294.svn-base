using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.ObjUtils;
using TIGE.Contract.Service;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        #region Deposit
        [HttpGet]
        [Route(PortalEndpoint.Wallet.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Wallet.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var wallet = await _walletService.GetByDepositByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist),NotificationStatus.Error);
                return View("Index");
            }

            return View(wallet);
        }

        [HttpPost]
        [Route(PortalEndpoint.Wallet.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] WalletDepositModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            await _walletService.UpdateDepositAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction("Edit");
        }

        #endregion


        #region Withdraw
        [HttpGet]
        [Route(PortalEndpoint.Wallet.GetWithdrawPagedEndpoint)]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Wallet.GetWithdrawTigePagedEndpoint)]
        public IActionResult WithdrawTige()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Wallet.EditWithdrawEndpoint)]
        public async Task<IActionResult> EditWithdraw(string id)
        {
            var wallet = await _walletService.GetWithdrawByIdAsync(id, this.GetRequestCancellationToken());

            if (wallet == null)
            {
                this.SetNotification(string.Format(Messages.Wallet.DoesNotExist), NotificationStatus.Error);
                return View("Index");
            }

            return View(wallet);
        }

        //fix
        [HttpPost]
        [Route(PortalEndpoint.Wallet.EditWithdrawEndpoint)]
        public async Task<IActionResult> SubmitEditWithdraw([FromForm] WithdrawRequestModel model)
        {
            if (model.Status == Enums.WithdrawStatus.Reject || model.Status == Enums.WithdrawStatus.TokenReject)
            {
                ModelState.ClearValidationState("TxHash");
                ModelState.Remove("TxHash");
            }
            //if (model.TxHash == null)
            //{
            //    ModelState.ClearValidationState("TxHash");
            //    ModelState.Remove("TxHash");
            //}    
            if (!ModelState.IsValid)
            {
                this.SetNotification(Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("EditWithdraw", model);
            }

            var redirectAction = "";
            if (model.Status == Enums.WithdrawStatus.New || model.Status == Enums.WithdrawStatus.Approved || model.Status == Enums.WithdrawStatus.Confirming || model.Status == Enums.WithdrawStatus.Reject)
                redirectAction = "Withdraw";
            else
                redirectAction = "WithdrawTige";
            ;
            await _walletService.UpdateWithdrawAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(string.Format(Messages.Wallet.UpdatedFormat, model.Id), NotificationStatus.Success);

            return RedirectToAction(redirectAction);
        }
        #endregion
    }
}