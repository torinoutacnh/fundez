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
using TIGE.Core.Share.Models.ProtectionFee;

namespace TIGE.Contract.Service
{
    public interface IProtectionFeeService
    {
        Task AddProtectionFee(CreateProtectionFeeModel model, CancellationToken cancellationToken = default);
        Task<DetailProtectionFeeModel> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task UpdateAsync(DetailProtectionFeeModel model, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<DetailProtectionFeeModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken =default);
        List<DetailProtectionFeeModel> GetAll();
    }
}