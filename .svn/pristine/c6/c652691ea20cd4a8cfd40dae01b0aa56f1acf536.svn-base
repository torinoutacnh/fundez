using Elect.Web.Swagger.Attributes;
using TIGE.Filters.Auth;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Filters.Exception;

namespace TIGE.Areas.Stack.Controllers
{
    [HideInApiDoc]
    [Auth]
    [Area(AreaName)]
    [ServiceFilter(typeof(LoggedInUserBinderFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    [ServiceFilter(typeof(PortalExceptionFilterAttribute))]
    public class BaseController : Controller
    {
        public const string AreaName = "stack";
    }
}