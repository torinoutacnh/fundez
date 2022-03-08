using System.Collections.Generic;
using System.Linq;
using System.Net;
using Elect.Core.XmlUtils;
using Elect.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Core.Share.Filters.Validation
{
    public class ApiValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // Log Error
            var keyValueInvalidDictionary = GetModelStateInvalidInfo(context);

            // Response Result
            var apiErrorViewModel =
                new ErrorModel(nameof(ErrorCode.BadRequest), ErrorCode.BadRequest, StatusCodes.Status400BadRequest)
                {
                    AdditionalData = keyValueInvalidDictionary
                };

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            if (context.HttpContext.Request.Headers[HttpRequestHeader.Accept.ToString()] == ContentType.Xml)
            {
                context.Result = new ContentResult
                {
                    ContentType = ContentType.Xml,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Content = XmlHelper.ToXmlString(apiErrorViewModel)
                };
            }
            else
            {
                context.Result = new ContentResult
                {
                    ContentType = ContentType.Json,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Content = JsonConvert.SerializeObject(apiErrorViewModel, Formattings.JsonSerializerSettings)
                };
            }
        }

        public static Dictionary<string, object> GetModelStateInvalidInfo(ActionExecutingContext context)
        {
            var keyValueInvalidDictionary = new Dictionary<string, object>();

            foreach (var keyValueState in context.ModelState)
            {
                var error = string.Join(", ", keyValueState.Value.Errors.Select(x => x.ErrorMessage));

                keyValueInvalidDictionary.Add(keyValueState.Key, error);
            }
            return keyValueInvalidDictionary;
        }
    }
}