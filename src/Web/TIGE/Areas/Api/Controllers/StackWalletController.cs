using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TIGE.Contract.Service;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Utils;

namespace TIGE.Areas.Api.Controllers
{
    public class StackWalletController : BaseController
    {
        private readonly IStackingWalletService _walletService;
        private readonly ICommonService _commonService;

        public StackWalletController(IStackingWalletService walletService, ICommonService commonService)
        {
            _walletService = walletService;
            _commonService = commonService;
        }        

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.GetDataTableEndpoint)]
        public async Task<DataTableActionResult<StackDepositModel>> GetDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetDepositDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.GetWithdrawDataTableEndpoint)]
        public async Task<DataTableActionResult<StackWithdrawModel>> GetDataTableWithdraw([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetWithdrawDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.GetWithdrawTransferDataTableEndpoint)]
        public async Task<DataTableActionResult<TransferRequestModel>> GetDataTableTransfer([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetTransferDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }


        [HttpPost]
        [Route(ApiEndpoint.StackWallet.DeleteEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "wallet deleted.")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _walletService.DeleteAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.DeleteWithdrawEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Withdraw deleted.")]
        public async Task<IActionResult> DeleteWithdraw([FromRoute] string id)
        {
            await _walletService.DeleteWithdrawAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.DeleteTransferEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "wallet deleted.")]
        public async Task<IActionResult> DeleteTransfer([FromRoute] string id)
        {
            await _walletService.DeleteTransferAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.GetWithdrawUSDDataTableEndpoint)]
        public async Task<DataTableActionResult<StackWithdrawUSDModel>> GetWithdrawUSDDataTable([FromForm] DataTableRequestModel model)
        {
            var wallets = await _walletService.GetWithdrawUSDDataTableAsync(model);
            var result = wallets.GetDataTableActionResult(model);
            return result;
        }

        [HttpPost]
        [Route(ApiEndpoint.StackWallet.DeleteWithdrawUSDEndpoint)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "wallet deleted.")]
        public async Task<IActionResult> DeleteWithdrawUSD([FromRoute] string id)
        {
            await _walletService.DeleteWithdrawUSDAsync(id, this.GetRequestCancellationToken()).ConfigureAwait(true);

            return NoContent();
        }
    }
}
