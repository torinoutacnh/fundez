using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using TIGE.Contract.Service.Base;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Slot;

namespace TIGE.Contract.Service
{
    public interface IWalletService
    {
        Task<WalletModel> GetMyWalletAsync(string userid, CancellationToken cancellationToken = default);
        Task<string> SubmitWithdrawAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default);
        Task<string> SubmitDepositAsync(CreateDepositRequestModel model, CancellationToken cancellationToken = default);
        Task<WalletDepositModel> GetByDepositByIdAsync(string id, CancellationToken cancellationToken = default);


        Task UpdateDepositAsync(WalletDepositModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<WalletDepositModel>> GetDepositDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task CheckConfirmWithToken(string token, CancellationToken cancellationToken = default);
        Task<List<WithdrawRequestModel>> GetMyWithdrawAsync(string currentId, CancellationToken cancellationToken = default);
        Task DeleteWithdrawAsync(string id, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<WithdrawRequestModel>> GetWithdrawDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<WithdrawRequestModel> GetWithdrawByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateWithdrawAsync(WithdrawRequestModel model, CancellationToken cancellationToken = default);
        Task CheckConfirmDepositWithToken(string token, CancellationToken cancellationToken = default);
        WalletDepositModel GetByLatestDepositAsync(string userId);
        WithdrawRequestModel GetByLatestWithDrawAsync(string userId);
        DetailSlotRequestModel GetByLatestSlotAsync(string userId);
        Task ResendConfirmDeposit(string data, CancellationToken cancellationToken = default);
        Task ResendConfirmWithdraw(string data, CancellationToken cancellationToken = default);
        //fix
        Task<string> SubmitWithdrawTigeAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default);
        Task CheckConfirmTigeWithToken(string token, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<WithdrawRequestModel>> GetWithdrawTigeDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task CheckConfirmTransferWithToken(string code, CancellationToken cancellationToken = default);
        Task ResendConfirmTransfer(string data, CancellationToken cancellationToken = default);
        Task<string> SubmitTransferAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default);
    }
}