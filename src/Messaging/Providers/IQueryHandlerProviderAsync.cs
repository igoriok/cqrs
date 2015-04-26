using System;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface IQueryHandlerProviderAsync
    {
        Task<IQueryHandlerAsync<IQuery<TResult>, TResult>> GetQueryHandlerAsync<TResult>(Type queryType, CancellationToken token);
    }
}