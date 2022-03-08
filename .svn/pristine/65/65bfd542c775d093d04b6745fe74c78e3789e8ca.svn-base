using System;
using System.Linq;
using System.Threading.Tasks;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Core.EnvUtils;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Models.Authentication;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Utils;
using TIGE.Utils;

namespace TIGE.Controllers
{
    [Route(LandingEndpoint.Business.IndexEndpoint)]
    public class BusinessController : BaseController
    {
        public const string Endpoint = AreaName;
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }


        [HttpGet]
        [Route(LandingEndpoint.Business.IndexEndpoint)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Business = await _businessService.GetMyBusinessAsync(LoggedInUser.Current.Id);
            return View();
        }

    }
}