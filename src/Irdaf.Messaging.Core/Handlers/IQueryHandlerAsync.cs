using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Handlers
{
    public interface IQueryHandlerAsync<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, IMessageContext context, CancellationToken cancellationToken);
    }
}