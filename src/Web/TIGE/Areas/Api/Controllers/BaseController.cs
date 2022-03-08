using TIGE.Filters.Auth;
using Elect.Web.Models;
using Elect.Web.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Filters.Exception;
using TIGE.Core.Share.Filters.Validation;

namespace TIGE.Areas.Api.Controllers
{
    [Auth]
    [ShowInApiDoc]
    [Area(ApiEndpoint.AreaName)]
    [Produces(ContentType.Json)]
    [ServiceFilter(typeof(LoggedInUserBinderFilter))]
    [ServiceFilter(typeof(ApiAuthActionFilter))]
    [ServiceFilter(typeof(ApiValidationActionFilterAttribute))]
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    public class BaseController : Controller
    {
    }
}