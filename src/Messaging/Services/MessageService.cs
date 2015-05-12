using System;
using System.Linq;
using Irdaf.Messaging.Dispatch;
using Irdaf.Messaging.Handlers;
using Irdaf.Messaging.Providers;

namespace Irdaf.Messaging.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageDispatcher _dispatcher;
        private readonly IMessageHandlerRegistry _registry;

        public MessageService(IMessageHandlerRegistry registry, IMessageDispatcher dispatcher)
        {
            _registry = registry;
            _dispatcher = dispatcher;
        }

        public virtual TResult Query<TResult>(IQuery<TResult> query)
        {
            var context = new QueryContext<TResult>(query);

            try
            {
                var registrations = _registry.RegistrationsFor(query.GetType()).Take(1);

                _dispatcher.Dispatch(context, registrations);
            }
            catch (Exception exception)
            {

                throw;
            }

            return context.Result;
        }

        public virtual void Execute(ICommand command)
        {
            var context = new CommandContext(command);

            try
            {
                var registrations = _registry.RegistrationsFor(command.GetType()).Take(1);

                _dispatcher.Dispatch(context, registrations);
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public virtual void Publish(IEvent @event)
        {

            var context = new EventContext(@event);

            try
            {
                var registrations = _registry.RegistrationsFor(@event.GetType());

                _dispatcher.Dispatch(context, registrations);
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public virtual IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            return _registry.RegisterEventHandler(handler);
        }

        private class QueryContext<TResult> : IMessageContext
        {
            private readonly IQuery<TResult> _query;
            private TResult _result;
            private bool _hasResult;

            public IQuery<TResult> Query
            {
                get { return _query; }
            }

            public Type MessageType
            {
                get { return _query.GetType(); }
            }

            IMessage IMessageContext.Message
            {
                get { return Query; }
            }

            public TResult Result
            {
                get
                {
                    if (!_hasResult)
                        throw new InvalidOperationException("Result not set");

                    return _result;
                }
                set
                {
                    _result = value;
                    _hasResult = true;
                }
            }

            public QueryContext(IQuery<TResult> query)
            {
                _query = query;
            }

            public void Execute(object handler)
            {
                Result = ((IQueryHandler<IQuery<TResult>, TResult>)handler).Handle(Query);
            }
        }

        private class EventContext : IMessageContext
        {
            private readonly IEvent _event;

            public IEvent Event
            {
                get { return _event; }
            }

            public Type MessageType
            {
                get { return _event.GetType(); }
            }

            IMessage IMessageContext.Message
            {
                get { return Event; }
            }

            public EventContext(IEvent @event)
            {
                _event = @event;
            }

            public void Execute(object handler)
            {
                ((IEventHandler<IEvent>)handler).Handle(Event);
            }
        }

        private class CommandContext : IMessageContext
        {
            private readonly ICommand _command;

            public ICommand Command
            {
                get { return _command; }
            }

            public Type MessageType
            {
                get { return _command.GetType(); }
            }

            IMessage IMessageContext.Message
            {
                get { return Command; }
            }

            public CommandContext(ICommand command)
            {
                _command = command;
            }

            public void Execute(object handler)
            {
                ((ICommandHandler<ICommand>)handler).Handle(Command);
            }
        }
    }
}