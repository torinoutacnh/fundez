using System.Threading;
using System.Threading.Tasks;

namespace TIGE.Contract.Service.Base
{
    public interface ICreateable<in T, TKey> where T : class, new()
    {
        Task<TKey> CreateAsync(T model, CancellationToken cancellationToken = default);
    }
}