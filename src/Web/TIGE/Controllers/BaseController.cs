using Elect.Web.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Filters.Auth;
using TIGE.Filters.Exception;

namespace TIGE.Controllers
{
    [HideInApiDoc]
    [Auth]
    [ServiceFilter(typeof(LoggedInUserBinderFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    [ServiceFilter(typeof(RootExceptionFilterAttribute))]
    public class BaseController : Controller
    {
        public const string AreaName = "";
    }
}