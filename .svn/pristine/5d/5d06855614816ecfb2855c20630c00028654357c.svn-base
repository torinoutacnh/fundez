
using Elect.Core.EnvUtils;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
   
    public class AdminController : BaseController
    {
        [HttpGet]
        [Auth(Permission.Admin, Permission.Manager)]
        [Route(PortalEndpoint.Admin.IndexEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(PortalEndpoint.Admin.OopsWithParamEndpoint)]
        public IActionResult Oops(int statusCode)
        {
            if (EnvHelper.IsDevelopment())
            {
                string message = "";

                message = $"Code: {statusCode}";

                this.SetNotification(message, NotificationStatus.Error);
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}