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
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        ///     Get DataTale Business
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Business.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<BusinessDetailModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var Businesss = await _businessService.GetDataTableAsync(model);
            var result = Businesss.GetDataTableActionResult(model);
            return result;
        }


        /// <summary>
        ///     Delete Business 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Business.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Business deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _businessService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}