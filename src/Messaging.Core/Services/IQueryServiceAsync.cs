using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Services
{
    public interface IQueryServiceAsync
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken token);
    }
}