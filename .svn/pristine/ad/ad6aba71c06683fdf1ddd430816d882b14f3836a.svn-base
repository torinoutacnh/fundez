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
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Controllers
{
    [Route(LandingEndpoint.Slot.IndexEndpoint)]
    public class SlotController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }


        [HttpGet]
        [Route(LandingEndpoint.Slot.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Slots = await _slotService.GetMySlot(LoggedInUser.Current.Id);
            return View();
        }   
        
        
        [HttpGet]
        [AllowAnonymous]
        [Route(LandingEndpoint.Slot.ConfirmEndpoint)]
        public async Task<IActionResult> Confirm([FromQuery] string token)
        {
            await _slotService.VerifyBuySlot(token);
            return View();
        }

        [HttpPost]
        [Route(LandingEndpoint.Slot.IndexEndpoint)]
        public async Task<IActionResult> SubmitBuySlot([FromForm] SubmitSlotModel model )
        {

            if (!ModelState.IsValid)
            {
                this.SetNotification("Please check data again!", NotificationStatus.Error);
                return RedirectToAction("Index");
            }

            try
            {
                var newid = await _slotService.BuySlot(model);
                this.SetNotification("Buy slot success, please confirm via email to complete transaction.", NotificationStatus.Success);
                return RedirectToAction("Verify", "Verify", new { type = Enums.VerifyType.BuySlot.ToString(), data = newid });
            }
            catch (Exception e)
            {
                this.SetNotification(e.Message, NotificationStatus.Error);
                return RedirectToAction("Index");
            }
        }

    }
}