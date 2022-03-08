using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Service;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.Stack;
using TIGE.Utils;

namespace TIGE.Areas.Api.Controllers
{
    public class StackCommissionController : BaseController
    {
        private readonly IStackCommissionService _stackCommissionService;

        public StackCommissionController(IStackCommissionService stackCommissionService)
        {
            _stackCommissionService = stackCommissionService;
        }

        [HttpPost]
        [Route(ApiEndpoint.Commission.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<CommissionModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var comms = await _stackCommissionService.GetCommissionDataTableAsync(model);
            var result = comms.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.Commission.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Delete.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _stackCommissionService.DeleteCommissionAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.Commission.GetRateDataTableEndpoint)]
        public async Task<DataTableActionResult<CommissionRateModel>> GetCommissionRateDataTable([FromForm] DataTableRequestModel model)
        {
            var comms = await _stackCommissionService.GetCommissionRateDataTableAsync(model);
            var result = comms.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.Commission.DeleteRateEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Delete.")]
        public async Task<IActionResult> DeleteCommissionRate([FromRoute] string id)
        {
            await _stackCommissionService.DeleteCommissionRateAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}
