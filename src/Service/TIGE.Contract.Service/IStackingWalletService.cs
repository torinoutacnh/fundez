using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;

namespace TIGE.Contract.Service
{
    public interface IStackingWalletService
    {
        Task<StackingWalletEntity> GetMyWalletAsync(string id, CancellationToken cancellationToken = default);
        Task<List<StackWithdrawModel>> GetMyWithdrawAsync(string currentId, CancellationToken cancellationToken = default);
        Task<string> SubmitWithdrawAsync(CreateWithdrawStackRequestModel model, CancellationToken cancellationToken = default);
        Task<string> SubmitDepositAsync(CreateDepositStackingRequestModel model, CancellationToken cancellationToken = default);
        Task CheckConfirmDepositWithToken(string token, CancellationToken cancellationToken = default);
        Task ResendConfirmDeposit(string data, CancellationToken cancellationToken = default);
        Task DeleteWithdrawAsync(string id, CancellationToken cancellationToken);
        Task<StackDepositModel> GetByDepositByIdAsync(string id, CancellationToken cancellationToken);

        //Task UpdateBalance(string walletid, CancellationToken cancellationToken = default);
        //HistoryDepositEntity GetByLatestDepositAsync(string userId);
        StackDepositModel GetByLatestDepositAsync(string userId);
        StackWithdrawModel GetByLatestWithDrawAsync(string userId);
        Task ResendConfirmWithdraw(string data, CancellationToken cancellationToken = default);
        Task CheckConfirmWithToken(string token, CancellationToken cancellationToken = default);
        Task<List<WithdrawRequestModel>> GetTranferRequest(string userId, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
        Task UpdateDepositAsync(StackDepositModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<StackDepositModel>> GetDepositDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<StackWithdrawModel>> GetWithdrawDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<TransferRequestModel>> GetTransferDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<StackWithdrawModel> GetByWithdrawByIdAsync(string id, CancellationToken cancellationToken);
        Task DeleteTransferAsync(string id, CancellationToken cancellationToken);
        Task UpdateWithdrawAsync(StackWithdrawModel model, CancellationToken cancellationToken);
        Task<WithdrawRequestModel> GetByTransferByIdAsync(string id, CancellationToken cancellationToken);
        Task UpdateTransferAsync(WithdrawRequestModel model, CancellationToken cancellationToken);
        Task<string> SubmitTransferAsync(CreateWithdrawRequestModel model, CancellationToken cancellationToken = default);
        Task<StackDashboardModel> GetDashBoardModel(CancellationToken cancellationToken = default);
        Task CreateWalletAsync(string id, CancellationToken cancellationToken = default);
        Task<string> SubmitWithdrawUSDAsync(CreateWithdrawStackRequestModel model, CancellationToken cancellationToken = default);
        Task CheckConfirmUSDWithToken(string token, CancellationToken cancellationToken = default);
        Task ResendConfirmWithdrawUSD(string data, CancellationToken cancellationToken = default);
        Task<List<StackWithdrawUSDModel>> GetMyWithdrawUSDAsync(string currentId, CancellationToken cancellationToken = default);
        StackWithdrawUSDModel GetByLatestWithDrawUSDAsync(string userId);
        Task DeleteWithdrawUSDAsync(string id, CancellationToken cancellationToken);
        Task<DataTableResponseModel<StackWithdrawUSDModel>> GetWithdrawUSDDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task<StackWithdrawUSDModel> GetConvertByIdAsync(string id, CancellationToken cancellationToken);
        Task UpdateWithdrawUSDAsync(StackWithdrawUSDModel model, CancellationToken cancellationToken);
    }
}
