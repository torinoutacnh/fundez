using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Models.Verify;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class VerifyController : BaseController
    {
        private readonly IVerifyService _verifyService;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IStackingWalletService _stackWalletService;
        private readonly ISlotService _slotService;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISubscriptionService _stackService;

        public VerifyController(IVerifyService verifyService, IUserService userService,
            IAuthenticationService authenticationService, IStackingWalletService stackWalletService,
            IWalletService walletService, ISlotService slotService, ITokenService tokenService, ISubscriptionService stackService)
        {
            _verifyService = verifyService;
            _userService = userService;
            _authenticationService = authenticationService;
            _walletService = walletService;
            _stackWalletService = stackWalletService;
            _slotService = slotService;
            _tokenService = tokenService;
            _stackService = stackService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(PortalEndpoint.Verify.IndexEndpoint)]
        public IActionResult Verify([FromQuery] Enums.VerifyType type, [FromQuery] string data = "")
        {
            var model = new VerifyModel()
            {
                Data = data,
                Type = type
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(PortalEndpoint.Verify.IndexEndpoint)]
        public async Task<IActionResult> SubmitVerify([FromForm] VerifyModel model)
        {
            try
            {
                switch (model.Type)
                {
                    case Enums.VerifyType.Register:
                        _authenticationService.CheckConfirmEmailToken(model.Code);
                        return RedirectToAction("Index", "Home");
                    case Enums.VerifyType.UpdateProfile:
                        await _userService.ConfirmUpdateProfileAsync(model.Code);
                        return RedirectToAction("Index", "Profile");
                    case Enums.VerifyType.UpdateStackProfile:
                        await _userService.ConfirmUpdateStackProfileAsync(model.Code);
                        return RedirectToAction("Index", "Profile", new {area = "Stack"});
                    case Enums.VerifyType.Deposit:
                        await _walletService.CheckConfirmDepositWithToken(model.Code);
                        return RedirectToAction("Index", "Wallet");
                    case Enums.VerifyType.DepositStacking:
                        await _stackWalletService.CheckConfirmDepositWithToken(model.Code);
                        return RedirectToAction("Index", "StackingWallet", new { area = "stack" });
                    case Enums.VerifyType.WithDraw:
                        await _walletService.CheckConfirmWithToken(model.Code);
                        return RedirectToAction("Withdraw", "Wallet");
                    case Enums.VerifyType.TransferTige:
                        await _walletService.CheckConfirmTransferWithToken(model.Code);
                        return RedirectToAction("Transfer", "StackingWallet", new { area = "stack" });
                    case Enums.VerifyType.WithdrawStacking:
                        await _stackWalletService.CheckConfirmWithToken(model.Code);
                        return RedirectToAction("Withdraw", "StackingWallet", new {area="stack"});
                    case Enums.VerifyType.WithdrawUSDStack:
                        await _stackWalletService.CheckConfirmUSDWithToken(model.Code);
                        return RedirectToAction("WithdrawUSD", "StackingWallet", new { area = "stack" });
                    //fix
                    case Enums.VerifyType.WithDrawTige:
                        await _walletService.CheckConfirmTigeWithToken(model.Code);
                        return RedirectToAction("WithdrawTige", "Wallet");
                    case Enums.VerifyType.BuySlot:
                        await _slotService.VerifyBuySlot(model.Code);
                        return RedirectToAction("Index", "Slot");
                    case Enums.VerifyType.SellToken:
                        await _tokenService.VerifySellToken(model.Code);
                        return RedirectToAction("Index", "Token");
                    case Enums.VerifyType.Stack:
                        await _stackService.VerifyBuyStack(model.Code);
                        return RedirectToAction("Index", "StackSubscription", new { area = "stack" });
                    case Enums.VerifyType.ForgetPassword:
                        _authenticationService.CheckSetPasswordToken(model.Code);
                        return RedirectToAction("SetPassword", "Auth", new {token = model.Code});
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Verify", "Verify", new { type = model.Type.ToString(), data = model.Data });
            }
            
        }


        [HttpPost]
        [AllowAnonymous]
        [Route(PortalEndpoint.Verify.ResendEndpoint)]
        public async Task<IActionResult> ResendVerify([FromForm] ResendVerifyModel model)
        {
            try
            {
                this.SetNotification("Resend code to your email, please check again.", NotificationStatus.Success);
                switch (model.Type)
                {
                    case Enums.VerifyType.Register: await _userService.ResendConfirmRegisterAsync(model.Data); break;
                    case Enums.VerifyType.UpdateProfile: await _userService.ResendUpdateProfileAsync(); break;
                    case Enums.VerifyType.UpdateStackProfile: await _userService.ResendUpdateProfileAsync(); break;
                    case Enums.VerifyType.Deposit: await _walletService.ResendConfirmDeposit(model.Data); break;
                    case Enums.VerifyType.DepositStacking: await _stackWalletService.ResendConfirmDeposit(model.Data); break;
                    case Enums.VerifyType.WithDraw: await _walletService.ResendConfirmWithdraw(model.Data); break;
                    case Enums.VerifyType.WithdrawUSDStack: await _stackWalletService.ResendConfirmWithdrawUSD(model.Data); break;
                    case Enums.VerifyType.TransferTige: await _walletService.ResendConfirmTransfer(model.Data); break;
                    case Enums.VerifyType.Stack: await _stackService.ResendConfirmStack(model.Data); break;
                    case Enums.VerifyType.WithdrawStacking: await _stackWalletService.ResendConfirmWithdraw(model.Data); break;
                    case Enums.VerifyType.BuySlot: await _slotService.ResendConfirmBuyAsync(model.Data); break;
                    case Enums.VerifyType.SellToken: await _tokenService.ResendSellToken(model.Data); break;
                    case Enums.VerifyType.ForgetPassword: await _authenticationService.SendSetPasswordAsync(model.Data); break;
                    default: break;
                }
                return RedirectToAction("Verify", "Verify", new { type = model.Type.ToString(), data = model.Data });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Verify", "Verify", new { type = model.Type.ToString(), data = model.Data });
            }
        }

    }
}