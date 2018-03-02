using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Convensions
{
    public interface IMessageConvention
    {
        Type MessageType { get; }

        Type GetHandlerBaseType();

        Type GetHandlerAsyncBaseType();

        Type GetHandlerType();

        Type GetHandlerAsyncType();

        MethodInfo GetHandlerMethodInfo(Type handlerType);

        MethodInfo GetHandlerAsyncMethodInfo(Type handlerType);

        void Invoke(IList<object> handlers, IPipelineContext context);

        Task InvokeAsync(IList<object> handlers, IPipelineContext context, CancellationToken cancellationToken);
    }
}