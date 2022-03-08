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

namespace TIGE.Contract.Service
{
    public interface IBusinessService
    {
        Task<List<BusinessDetailModel>> GetMyBusinessAsync(string userId);
        Task<double> GetTotalCommission(string userId);
        Task<DataTableResponseModel<BusinessDetailModel>> GetDataTableAsync(DataTableRequestModel model);
        Task DeleteAsync (string id, CancellationToken cancellationToken = default);
        Task<BusinessDetailModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(BusinessDetailModel model, CancellationToken cancellationToken = default);
    }
}