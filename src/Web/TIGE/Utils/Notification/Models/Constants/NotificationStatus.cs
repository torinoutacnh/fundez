﻿using System.ComponentModel.DataAnnotations;

namespace TIGE.Utils.Notification.Models.Constants
{
    public enum NotificationStatus
    {
        [Display(Name = "success")]
        Success = 1,

        [Display(Name = "error")]
        Error = 2,

        [Display(Name = "warning")]
        Warning = 3,

        [Display(Name = "info")]
        Info = 4
    }
}