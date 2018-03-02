using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Pipeline.Convensions
{
    public class CommandConvention<TCommand> : BaseMessageConvention<CommandContext>
        where TCommand : ICommand
    {
        public CommandConvention()
            : base(typeof(TCommand))
        {
        }

        public override Type GetHandlerBaseType()
        {
            return typeof(ICommandHandler<>);
        }

        public override Type GetHandlerAsyncBaseType()
        {
            return typeof(ICommandHandlerAsync<>);
        }

        public override Type GetHandlerType()
        {
            return GetHandlerBaseType().MakeGenericType(MessageType);
        }

        public override Type GetHandlerAsyncType()
        {
            return GetHandlerAsyncBaseType().MakeGenericType(MessageType);
        }

        protected override void Invoke(IList<object> handlers, CommandContext context)
        {
            var handler = handlers.Single();

            if (handler is ICommandHandler<TCommand> syncHandler)
            {
                syncHandler.Handle((TCommand)context.Command, context);
            }
            else if (handler is ICommandHandlerAsync<TCommand> asyncHandler)
            {
                asyncHandler.HandleAsync((TCommand)context.Command, context, CancellationToken.None).GetAwaiter().GetResult();
            }
            else
            {
                throw new InvalidOperationException("Handlers not found");
            }
        }

        protected override async Task InvokeAsync(IList<object> handlers, CommandContext context, CancellationToken cancellationToken)
        {
            var handler = handlers.Single();

            if (handler is ICommandHandlerAsync<TCommand> asyncHandler)
            {
                await asyncHandler.HandleAsync((TCommand)context.Command, context, cancellationToken);
            }
            else if (handler is ICommandHandler<TCommand> syncHandler)
            {
                await Task.Run(() => syncHandler.Handle((TCommand)context.Command, context), cancellationToken);
            }
            else
            {
                throw new InvalidOperationException("Handlers not found");
            }
        }
    }
}