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
using TIGE.Core.Share.Models.Token;

namespace TIGE.Contract.Service
{
    public interface ITokenService
    {
        Task<string> SellToken(SubmitTokenModel model, CancellationToken cancellationToken = default);
        Task<List<DetailSellTokenModel>> GetMySellToken(string userId, CancellationToken cancellationToken = default);

        Task<DetailSellTokenModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(DetailSellTokenModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<DetailSellTokenModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken =default);
        Task VerifySellToken(string token, CancellationToken cancellationToken = default);
        DetailSellTokenModel GetByLatestSellTokenAsync(string userId);
        Task<double> GetTotalToken(string userId, CancellationToken cancellationToken = default);
        Task ResendSellToken(string data, CancellationToken cancellationToken = default);
    }
}