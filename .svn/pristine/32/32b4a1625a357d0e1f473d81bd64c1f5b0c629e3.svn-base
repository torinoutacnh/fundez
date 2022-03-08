using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Share.Models.Stack;

namespace TIGE.Contract.Service
{
    public interface IStackCommissionService
    {
        Task<StackCommissionEntity> GetCommissionAsync(string id, CancellationToken cancellationToken = default);
        Task<StackCommissionEntity> CreateCommissionAsync(StackCommissionEntity model,CancellationToken cancellationToken = default);
        Task<string> UpdateCommissionAsync(StackCommissionEntity model, CancellationToken cancellationToken = default);
        Task DeleteCommissionAsync(string id, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<CommissionModel>> GetCommissionDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);

        Task<StackCommissionRateEntity> GetCommissionRateAsync(string id, CancellationToken cancellationToken = default);
        Task<StackCommissionRateEntity> CreateCommissionRateAsync(StackCommissionRateEntity model, CancellationToken cancellationToken = default);
        Task<string> UpdateCommissionRateAsync(StackCommissionRateEntity model, CancellationToken cancellationToken = default);
        Task DeleteCommissionRateAsync(string id, CancellationToken cancellationToken = default);
        Task<DataTableResponseModel<CommissionRateModel>> GetCommissionRateDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default);
    }
}
