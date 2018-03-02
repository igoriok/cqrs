using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Pipeline.Convensions
{
    public abstract class BaseMessageConvention<TContext> : IMessageConvention
        where TContext : IPipelineContext
    {
        public Type MessageType { get; }

        protected BaseMessageConvention(Type messageType)
        {
            MessageType = messageType;
        }

        public abstract Type GetHandlerBaseType();
        public abstract Type GetHandlerAsyncBaseType();

        public abstract Type GetHandlerType();
        public abstract Type GetHandlerAsyncType();

        public MethodInfo GetHandlerBaseMethodInfo()
        {
            return GetHandlerBaseType().GetRuntimeMethods().First();
        }

        public MethodInfo GetHandlerAsyncBaseMethodInfo()
        {
            return GetHandlerAsyncBaseType().GetRuntimeMethods().First();
        }

        public MethodInfo GetHandlerMethodInfo(Type handlerType)
        {
            var interfaceMap = handlerType.GetTypeInfo().GetRuntimeInterfaceMap(GetHandlerBaseType());

            return interfaceMap.TargetMethods[0];
        }

        public MethodInfo GetHandlerAsyncMethodInfo(Type handlerType)
        {
            var interfaceMap = handlerType.GetTypeInfo().GetRuntimeInterfaceMap(GetHandlerAsyncBaseType());

            return interfaceMap.TargetMethods[0];
        }

        protected abstract void Invoke(IList<object> handlers, TContext context);

        public void Invoke(IList<object> handlers, IPipelineContext context)
        {
            Invoke(handlers, (TContext)context);
        }

        protected abstract Task InvokeAsync(IList<object> handlers, TContext context, CancellationToken cancellationToken);

        public async Task InvokeAsync(IList<object> handlers, IPipelineContext context, CancellationToken cancellationToken)
        {
            await InvokeAsync(handlers, (TContext)context, cancellationToken);
        }
    }
}