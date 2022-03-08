using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Api.Models;
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
    public interface ISlotService
    {
        Task<string> BuySlot(SubmitSlotModel model, CancellationToken cancellationToken = default);
        Task<List<DetailSlotRequestModel>> GetMySlot(string userId, CancellationToken cancellationToken = default);

        Task<DetailSlotRequestModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(DetailSlotRequestModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<DetailSlotRequestModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken =default);
        Task VerifyBuySlot(string token, CancellationToken cancellationToken = default);
        Task ResendConfirmBuyAsync(string id, CancellationToken cancellationToken = default);
    }
}