using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Api.Models;

namespace TIGE.Contract.Service.Base
{
    public interface IGetPagedable<T> where T : class, new()
    {
        Task<PagedResponseModel<T>> GetPagedAsync(TIGE.Core.Models.PagedRequestModel model, CancellationToken cancellationToken = default);
    }
}