﻿using Elect.Web.HttpUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TIGE.Core.Share.Filters.Auth;

namespace TIGE.Filters.Auth
{
    public class MvcAuthActionFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                if (!context.IsAuthenticated())
                {
                    context.Result = new JsonResult(new { });

                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return;
                }

                if (!context.IsAuthorized())
                {
                    context.Result = new JsonResult(new { });

                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    return;
                }

                return;
            }

            if (!context.IsAuthenticated())
            {
                var @continue = context.HttpContext.Request.GetDisplayUrl();

                context.Result =
                    new RedirectToActionResult("SignIn", "Auth", new {area = ""}, false);

                return;
            }

            if (!context.IsAuthorized())
            {
                context.Result = new RedirectToActionResult("Oops", "Home", new {area = "", @statusCode = 403}, false);
            }
        }
    }
}