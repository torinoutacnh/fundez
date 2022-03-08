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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Utils;
using Swashbuckle.AspNetCore.Annotations;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Token;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Areas.Api.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class TokenController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly ICommonService _commonService;

        public TokenController(ITokenService tokenService, ICommonService commonService)
        {
            _tokenService = tokenService;
            _commonService = commonService;
        }

        /// <summary>
        ///     Get DataTale Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Token.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<DetailSellTokenModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var tokens = await _tokenService.GetDataTableAsync(model);
            var result = tokens.GetDataTableActionResult(model);
            return result;
        }


        /// <summary>
        ///     Delete Token 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Token.DeleteEndpoint)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Token deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _tokenService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }


        /// <summary>
        ///     Token Calculating
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiEndpoint.Token.CalculatingEndpoint)]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, "Token Calculating.")]
        public async Task<IActionResult> Calculating([FromRoute] double quantity)
        {
            var result = await _commonService.CalculatingTokenPrice(quantity);
            return Ok(result);
        }
    }
}