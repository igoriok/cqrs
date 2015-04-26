using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Services;

namespace Irdaf.Messaging
{
    public static class QueryExtensions
    {
        public static Task<TResult> QueryAsync<TResult>(this IQueryServiceAsync service, IQuery<TResult> query)
        {
            return service.QueryAsync(query, CancellationToken.None);
        }
    }
}