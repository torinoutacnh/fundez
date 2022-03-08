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
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        /// <summary>
        ///     Get DataTale Wallet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Wallet.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<WalletDepositModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetDepositDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        /// <summary>
        ///     Get DataTale Withdraw
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Wallet.GetWithdrawDataTableEndpoint)]
        public async Task<DataTableActionResult<WithdrawRequestModel>> GetWithdrawDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetWithdrawDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        /// <summary>
        ///     Get DataTale WithdrawTige
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Wallet.GetWithdrawTigeDataTableEndpoint)]
        public async Task<DataTableActionResult<WithdrawRequestModel>> GetWithdrawTigeDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetWithdrawTigeDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        /// <summary>
        ///     Delete wallet 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Wallet.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "wallet deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _walletService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        } 
        
        
        /// <summary>
          ///     Delete withdraw 
          /// </summary>
          /// <returns></returns>
        [HttpPost]
        [Route(ApiEndpoint.Wallet.DeleteWithdrawEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Withdraw deleted.")]
        public async Task<IActionResult> DeleteWithdraw([FromRoute] string id)
        {
            await _walletService.DeleteWithdrawAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}