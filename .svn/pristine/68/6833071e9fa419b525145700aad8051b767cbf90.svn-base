using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Models.Email;
using TIGE.Core.Utils;

namespace TIGE.Areas.Portal.Controllers
{
    [AllowAnonymous]
    public class TemplateController : BaseController
    {
        [HttpGet]
        [Route(PortalEndpoint.Email.EmailTemplateEndpoint)]
        public IActionResult Email([FromQuery]EmailModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ApplicationName))
            {
                model.ApplicationName = SystemHelper.Setting.ApplicationName;
            }
            
            return View(model);
        }
    }
}