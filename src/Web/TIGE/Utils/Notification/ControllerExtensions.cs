using TIGE.Models.Constants;
using TIGE.Utils.Notification.Models;
using TIGE.Utils.Notification.Models.Constants;
using Elect.Web.ITempDataDictionaryUtils;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Share.Exceptions;

namespace TIGE.Utils.Notification
{
    public static class ControllerExtensions
    {
        public static void SetNotification(this Controller controller,string message, NotificationStatus status = NotificationStatus.Info)
        {
            controller.TempData.Set(TempDataKey.Notify, new NotificationModel("", message, status));
        }

        public static void SetNotification(this Controller controller, string title, CoreException exception, NotificationStatus status = NotificationStatus.Error)
        {
            var errorCode = new ErrorModel(exception);

            var message = errorCode.Message;

            controller.SetNotification(message, status);
            
        }

        public static void RemoveNotify(this Controller controller)
        {
            controller.TempData.Remove(TempDataKey.Notify);
        }
    }
}