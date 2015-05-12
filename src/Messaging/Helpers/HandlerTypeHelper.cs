using System;
using Irdaf.Messaging.Handlers;

namespace Irdaf.Messaging.Helpers
{
    public static class HandlerTypeHelper
    {
        public static Type GetHandlerType(Type messageType)
        {
            if (messageType.IsAssignableFrom(typeof(IQuery<>)))
            {
                var resultType = messageType.GetGenericArguments()[0];

                return typeof(IQueryHandler<,>).MakeGenericType(messageType, resultType);
            }

            if (messageType.IsAssignableFrom(typeof(ICommand)))
            {
                return typeof(ICommandHandler<>).MakeGenericType(messageType);
            }

            if (messageType.IsAssignableFrom(typeof(IEvent)))
            {
                return typeof(IEventHandler<>).MakeGenericType(messageType);
            }

            throw new ArgumentException("Unsupported message type");
        }
    }
}