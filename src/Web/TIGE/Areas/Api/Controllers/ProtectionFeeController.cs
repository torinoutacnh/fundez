using System;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Utils;
using TIGE.Utils;
using Elect.Web.Api;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Elect.Web.IUrlHelperUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Utils;
using Swashbuckle.AspNetCore.Annotations;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.ProtectionFee;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class ProtectionFeeController : BaseController
    {
        private readonly IProtectionFeeService _protectionFeeService;

        public ProtectionFeeController(IProtectionFeeService protectionFeeService)
        {
            _protectionFeeService = protectionFeeService;
        }

        /// <summary>
        ///     Get DataTale ProtectionFee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.ProtectionFee.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<DetailProtectionFeeModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var ProtectionFees = await _protectionFeeService.GetDataTableAsync(model);
            var result = ProtectionFees.GetDataTableActionResult(model);
            return result;
        }


        /// <summary>
        ///     Delete ProtectionFee 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.ProtectionFee.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "ProtectionFee deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _protectionFeeService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}