using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Core.EnvUtils;
using Microsoft.AspNetCore.Mvc;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models.Configuration;
using TIGE.Utils;

namespace TIGE.Areas.Portal.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly IConfigurationService _configurationService;

        public ConfigController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        [Route(PortalEndpoint.Config.IndexEndpoint)]
        public IActionResult Index()
        {
            var config = _configurationService.GetConfig(this.GetRequestCancellationToken());
            return View(config);
        }

        [HttpPost]
        [Route(PortalEndpoint.Config.IndexEndpoint)]
        public IActionResult UpdateConfig([FromForm] ConfigurationModel model)
        {
            _configurationService.UpdateAsync(model, this.GetRequestCancellationToken());
            this.SetNotification( "Update system config successfully!", NotificationStatus.Success);
            return RedirectToAction("Index");
        }
    }
}