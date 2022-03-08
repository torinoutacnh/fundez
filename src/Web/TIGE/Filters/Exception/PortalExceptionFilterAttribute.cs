using System;
using Elect.Core.DictionaryUtils;
using Elect.Logger.Logging;
using Elect.Web.HttpUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Filters.Exception;
using TIGE.Utils.Notification.Models;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Filters.Exception
{
    public class PortalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IElectLog _electLog;

        public PortalExceptionFilterAttribute(ITempDataDictionaryFactory tempDataDictionaryFactory, IElectLog electLog)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _electLog = electLog;
        }

        public override void OnException(ExceptionContext context)
        {
            // Ignore cancel action

            if (context.Exception is OperationCanceledException || context.Exception is ObjectDisposedException)
            {
                context.ExceptionHandled = true;
                return;
            }

            var errorModel = ExceptionContextHelper.GetErrorModel(context, _electLog);

            // Ajax Case
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(errorModel, Formattings.JsonSerializerSettings);

                context.ExceptionHandled = true;

                // Keep base Exception
                base.OnException(context);

                return;
            }

            // MVC Page
            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new RedirectToActionResult("SignIn", "Auth", new { area = Areas.Portal.Controllers.BaseController.AreaName }, false);
            }
            else
            {
#if DEBUG
                context.ExceptionHandled = true;

                // Keep base Exception
                base.OnException(context);

                return;
#else
                context.Result = new RedirectToActionResult("OopsWithParamEndpoint", "Home", new { area = Areas.Portal.Controllers.BaseController.AreaName, statusCode = errorModel.StatusCode }, false);
#endif
            }

            // Notify
            var tempData = _tempDataDictionaryFactory.GetTempData(context.HttpContext);

            tempData.AddOrUpdate(Models.Constants.TempDataKey.Notify,
                new NotificationModel
                {
                    Title = "Oops, something went wrong!",
                    Message = errorModel.Message,
                    Status = NotificationStatus.Error
                });

            context.ExceptionHandled = true;

            // Keep base Exception
            base.OnException(context);
        }
    }
}