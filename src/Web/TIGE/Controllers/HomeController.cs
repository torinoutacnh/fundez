using System.Linq;
using System.Threading.Tasks;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Core.EnvUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TIGE.Contract.Service;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Controllers
{
    [Route(Endpoint)]
    public class HomeController : BaseController
    {
        public const string Endpoint = AreaName;

        public const string OopsEndpoint = "oops";

        public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";

        private readonly IConfigurationService _configurationService;
        private readonly IUserService _userService;

        public HomeController(IConfigurationService configurationService, IUserService userService)
        {
            _configurationService = configurationService;
            _userService = userService;
        }


        [HttpGet]
        [Route("~/")]
        public async Task<IActionResult> Index()
        {

            if (LoggedInUser.Current == null)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            if (LoggedInUser.Current.ListPermission.Contains(Permission.Admin))
            {
                return RedirectToAction("Index", "Admin", new { Area = "Portal" });
            }
            //if (LoggedInUser.Current.ListPermission.Contains(Permission.Admin))
            //{
            //    return RedirectToAction("Index", "User", new { Area = "StackAdmin" });
            //}

            var model = await _userService.GetDashBoardAsync();
            //return RedirectToAction("Index", "Home",new {area="stack"});
            return View(model);
        }

        [HttpGet]
        [Route("~/policy")]
        public IActionResult Policy()
        {
            
            return View();
        }

        [HttpGet]
        [Route("~/terms-of-use")]
        public IActionResult TermCondition()
        {

            return View();
        }

        [HttpGet]
        [Route("~/privacy-policy")]
        public IActionResult Privacy()
        {

            return View();
        }

        // Oops

        [HttpGet]
        [Route(OopsWithParamEndpoint)]
        public IActionResult Oops(int statusCode)
        {
            if (EnvHelper.IsDevelopment())
            {
                string message = "";

                message = $"Code: {statusCode}";

                this.SetNotification(message, NotificationStatus.Error);
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("~/Error/{code}")]
        [AllowAnonymous]
        public IActionResult HandleError(int code)
        {
            ViewBag.Code = code;
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            return View("PageNotFound");
        }
    }
}