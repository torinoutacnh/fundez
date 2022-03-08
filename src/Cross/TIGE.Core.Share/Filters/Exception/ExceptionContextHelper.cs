using System;
using Elect.Core.EnvUtils;
using Elect.Logger.Logging;
using Elect.Logger.Models.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Core.Share.Filters.Exception
{
    public static class ExceptionContextHelper
    {
        public static ErrorModel GetErrorModel(ExceptionContext context, IElectLog electLog)
        {
            ErrorModel errorModel;

            switch (context.Exception)
            {
                case CoreException exception:
                    errorModel = new ErrorModel(exception);
                  
                    electLog.Capture(exception, LogType.Warning, context.HttpContext);
                    
                    break;
                case UnauthorizedAccessException _:
                    errorModel = new ErrorModel(nameof(ErrorCode.UnAuthorized), ErrorCode.UnAuthorized, StatusCodes.Status401Unauthorized);
                   
                    break;
                default:
                    var message = EnvHelper.IsDevelopment() ? context.Exception.Message : ErrorCode.Unknown;
                    
                    errorModel = new ErrorModel(nameof(ErrorCode.Unknown), message, StatusCodes.Status500InternalServerError);
                    
                    electLog.Capture(context.Exception, LogType.Error, context.HttpContext);

                    if (EnvHelper.IsDevelopment())
                    {
                        // Add additional data
                        errorModel.AdditionalData.Add("stackTrace", context.Exception.StackTrace);
                    
                        errorModel.AdditionalData.Add("innerException", context.Exception.InnerException?.Message);
                        
                        errorModel.AdditionalData.Add("note", "The message is exception message and additional data such as 'stackTrace', 'internalException' and 'note' only have in [Development Environment].");
                    }

                    break;
            }
            
            context.HttpContext.Response.StatusCode = errorModel.StatusCode;

            return errorModel;
        }
    }
}