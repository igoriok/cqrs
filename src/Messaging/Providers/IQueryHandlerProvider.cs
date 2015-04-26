using System;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Providers
{
    public interface IQueryHandlerProvider
    {
        IQueryHandler<IQuery<TResult>, TResult> GetQueryHandler<TResult>(Type queryType);
    }
}