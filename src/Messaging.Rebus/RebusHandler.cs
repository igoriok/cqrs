using System.Threading.Tasks;
using Irdaf.Messaging.Services;
using Rebus.Bus;
using Rebus.Handlers;

namespace Irdaf.Messaging.Rebus
{
    public class RebusHandler : IHandleMessages<IMessage>
    {
        private readonly IBus _bus;
        private readonly IMessageServiceAsync _messageService;

        public RebusHandler(IBus bus, IMessageServiceAsync messageService)
        {
            _bus = bus;
            _messageService = messageService;
        }

        public async Task Handle(IMessage message)
        {
            var @event = message as IEvent;
            if (@event != null)
            {
                await _messageService.PublishAsync(@event);

                return;
            }

            var command = message as ICommand;
            if (command != null)
            {
                await _messageService.ExecuteAsync(command);

                await _bus.Reply(string.Empty);

                return;
            }

            var type = message.GetType();

            while (type != null)
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IQuery<>))
                    {
                        var returnType = interfaceType.GetGenericArguments()[0];
                        var queryMethod = typeof(IMessageServiceAsync).GetMethod("QueryAsync").MakeGenericMethod(returnType);

                        var result = await (Task<object>)queryMethod.Invoke(_messageService, new object[] { message });

                        await _bus.Reply(result);
                    }
                }

                type = type.BaseType;
            }
        }
    }
}