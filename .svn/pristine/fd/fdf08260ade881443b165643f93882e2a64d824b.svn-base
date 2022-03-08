using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TIGE.Utils
{
    public static class ControllerExtensions
    {
        [DebuggerStepThrough]
        public static CancellationToken GetRequestCancellationToken(this Controller controller)
        {
            return controller.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}