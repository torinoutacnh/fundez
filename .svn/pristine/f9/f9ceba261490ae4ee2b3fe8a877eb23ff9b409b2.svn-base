using System.Threading;
using System.Threading.Tasks;

namespace TIGE.Contract.Service.Base
{
    public interface IDeleteable<in TKey>
    {
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }
}