using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging
{
    public static class RegistrationExtensions
    {
        public static IDisposable RegisterQueryHandler<TQuery, TResult, TQueryHandler>(this IMessageHandlerRegistry registry)
            where TQuery : IQuery<TResult>
            where TQueryHandler : IQueryHandler<TQuery, TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = typeof(TQueryHandler);
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerTypeRegistration(queryType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterQueryHandlerAsync<TQuery, TResult, TQueryHandler>(this IMessageHandlerRegistry registry)
            where TQuery : IQuery<TResult>
            where TQueryHandler : IQueryHandlerAsync<TQuery, TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = typeof(TQueryHandler);
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerTypeRegistration(queryType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterQueryHandler<TQuery, TResult>(this IMessageHandlerRegistry registry, IQueryHandler<TQuery, TResult> queryHandler)
            where TQuery : IQuery<TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = queryHandler.GetType();
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(queryType, handlerMethod, queryHandler));
        }

        public static IDisposable RegisterQueryHandler<TQuery, TResult>(this IMessageHandlerRegistry registry, Func<TQuery, TResult> queryHandler)
            where TQuery : IQuery<TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = queryHandler.GetType();
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(queryType, handlerMethod, queryHandler.CreateQueryHandler()));
        }

        public static IDisposable RegisterQueryHandlerAsync<TQuery, TResult>(this IMessageHandlerRegistry registry, IQueryHandlerAsync<TQuery, TResult> queryHandler)
            where TQuery : IQuery<TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = queryHandler.GetType();
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(queryType, handlerMethod, queryHandler));
        }

        public static IDisposable RegisterQueryHandlerAsync<TQuery, TResult>(this IMessageHandlerRegistry registry, Func<TQuery, CancellationToken, Task<TResult>> queryHandler)
            where TQuery : IQuery<TResult>
        {
            var queryType = typeof(TQuery);
            var resultType = typeof(TResult);
            var handlerType = queryHandler.GetType();
            var handlerMethod = GetQueryHandlerMethod(queryType, resultType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(queryType, handlerMethod, queryHandler.CreateQueryHandlerAsync()));
        }

        public static IDisposable RegisterCommandHandler<TCommand, TCommandHandler>(this IMessageHandlerRegistry registry)
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var commandType = typeof(TCommand);
            var handlerType = typeof(TCommandHandler);
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerTypeRegistration(commandType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterCommandHandlerAsync<TCommand, TCommandHandler>(this IMessageHandlerRegistry registry)
            where TCommand : ICommand
            where TCommandHandler : ICommandHandlerAsync<TCommand>
        {
            var commandType = typeof(TCommand);
            var handlerType = typeof(TCommandHandler);
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerTypeRegistration(commandType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterCommandHandler<TCommand>(this IMessageHandlerRegistry registry, ICommandHandler<TCommand> commandHandler)
            where TCommand : ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = commandHandler.GetType();
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(commandType, handlerMethod, commandHandler));
        }

        public static IDisposable RegisterCommandHandler<TCommand>(this IMessageHandlerRegistry registry, Action<ICommand> commandHandler)
            where TCommand : ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = commandHandler.GetType();
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(commandType, handlerMethod, commandHandler.CreateCommandHandler()));
        }

        public static IDisposable RegisterCommandHandlerAsync<TCommand>(this IMessageHandlerRegistry registry, ICommandHandlerAsync<TCommand> commandHandler)
            where TCommand : ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = commandHandler.GetType();
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(commandType, handlerMethod, commandHandler));
        }

        public static IDisposable RegisterCommandHandlerAsync<TCommand>(this IMessageHandlerRegistry registry, Func<TCommand, CancellationToken, Task> commandHandler)
            where TCommand : ICommand
        {
            var commandType = typeof(TCommand);
            var handlerType = commandHandler.GetType();
            var handlerMethod = GetCommandHandlerMethod(commandType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(commandType, handlerMethod, commandHandler.CreateCommandHandlerAsync()));
        }

        public static IDisposable RegisterEventHandler<TEvent, TEventHandler>(this IMessageHandlerRegistry registry)
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(TEventHandler);
            var handlerMethod = GetEventHandlerMethod(eventType, handlerType);

            return registry.Register(new HandlerTypeRegistration(eventType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterEventHandlerAsync<TEvent, TEventHandler>(this IMessageHandlerRegistry registry)
            where TEvent : IEvent
            where TEventHandler : IEventHandlerAsync<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(TEventHandler);
            var handlerMethod = GetEventHandlerMethod(eventType, handlerType);

            return registry.Register(new HandlerTypeRegistration(eventType, handlerMethod, handlerType));
        }

        public static IDisposable RegisterEventHandler<TEvent>(this IMessageHandlerRegistry registry, IEventHandler<TEvent> eventHandler)
            where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            var handlerType = eventHandler.GetType();
            var handlerMethod = GetEventHandlerMethod(eventType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(eventType, handlerMethod, eventHandler));
        }

        public static IDisposable RegisterEventHandler<TEvent>(this IMessageHandlerRegistry registry, Action<TEvent> eventHandler)
            where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            var handlerType = eventHandler.GetType();
            var handlerMethod = GetEventHandlerMethod(eventType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(eventType, handlerMethod, eventHandler.CreateEventHandler()));
        }

        public static IDisposable RegisterEventHandlerAsync<TEvent>(this IMessageHandlerRegistry registry, Func<TEvent, CancellationToken, Task> eventHandler)
            where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            var handlerType = eventHandler.GetType();
            var handlerMethod = GetEventHandlerMethod(eventType, handlerType);

            return registry.Register(new HandlerInstanceRegistration(eventType, handlerMethod, eventHandler.CreateEventHandlerAsync()));
        }

        private static MethodBase GetQueryHandlerMethod(Type queryType, Type resultType, Type handlerType)
        {
            var interfaceType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
            var interfaceMap = handlerType.GetInterfaceMap(interfaceType);

            return interfaceMap.InterfaceMethods[0];
        }

        private static MethodBase GetCommandHandlerMethod(Type commandType, Type handlerType)
        {
            var interfaceType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var interfaceMap = handlerType.GetInterfaceMap(interfaceType);

            return interfaceMap.InterfaceMethods[0];
        }

        private static MethodBase GetEventHandlerMethod(Type eventType, Type handlerType)
        {
            var interfaceType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var interfaceMap = handlerType.GetInterfaceMap(interfaceType);

            return interfaceMap.InterfaceMethods[0];
        }

        private class HandlerTypeRegistration : IHandlerRegistration
        {
            private readonly Type _messageType;
            private readonly Type _handlerType;
            private readonly MethodBase _handlerMethod;

            public Type MessageType
            {
                get { return _messageType; }
            }

            public Type HandlerType
            {
                get { return _handlerType; }
            }

            public MethodBase HandlerMethod
            {
                get { return _handlerMethod; }
            }

            public HandlerTypeRegistration(Type messageType, MethodBase handlerMethod, Type handlerType)
            {
                _messageType = messageType;
                _handlerType = handlerType;
                _handlerMethod = handlerMethod;
            }

            public object CreateHandler()
            {
                throw new NotImplementedException();
            }
        }

        private class HandlerInstanceRegistration : IHandlerRegistration
        {
            private readonly Type _messageType;
            private readonly object _handlerInstance;
            private readonly MethodBase _handlerMethod;

            public Type MessageType
            {
                get { return _messageType; }
            }

            public Type HandlerType
            {
                get { return _handlerInstance.GetType(); }
            }

            public MethodBase HandlerMethod
            {
                get { return _handlerMethod; }
            }

            public HandlerInstanceRegistration(Type messageType, MethodBase handlerMethod, object handlerInstance)
            {
                _messageType = messageType;
                _handlerMethod = handlerMethod;
                _handlerInstance = handlerInstance;
            }

            public object CreateHandler()
            {
                throw new NotImplementedException();
            }
        }
    }
}