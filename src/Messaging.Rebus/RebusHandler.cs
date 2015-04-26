using Irdaf.Messaging.Services;
using Rebus;

namespace Irdaf.Messaging.Rebus
{
    public class RebusHandler<TResult> :
        IHandleMessages<IQuery<TResult>>,
        IHandleMessages<ICommand>,
        IHandleMessages<IEvent>
    {
        private readonly IBus _bus;
        private readonly IMessageService _messageService;

        public RebusHandler(IBus bus, IMessageService messageService)
        {
            _bus = bus;
            _messageService = messageService;
        }

        public void Handle(IQuery<TResult> message)
        {
            var result = _messageService.Query(message);

            _bus.Reply(result);
        }

        public void Handle(ICommand message)
        {
            _messageService.Execute(message);
        }

        public void Handle(IEvent message)
        {
            _messageService.Publish(message);
        }
    }
}