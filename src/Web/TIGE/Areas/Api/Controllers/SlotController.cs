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
using TIGE.Core.Share.Models.Slot;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class SlotController : BaseController
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        /// <summary>
        ///     Get DataTale Slot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Slot.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<DetailSlotRequestModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var slots = await _slotService.GetDataTableAsync(model);
            var result = slots.GetDataTableActionResult(model);
            return result;
        }


        /// <summary>
        ///     Delete Slot 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Slot.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Slot deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _slotService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}