using System.Threading.Tasks;
using Irdaf.Messaging.Services;
using Rebus.Handlers;

namespace Irdaf.Messaging.Rebus
{
    public class RebusHandler :
        IHandleMessages<ICommand>,
        IHandleMessages<IEvent>
    {
        private readonly IMessageServiceAsync _messageService;

        public RebusHandler(IMessageServiceAsync messageService)
        {
            _messageService = messageService;
        }

        public Task Handle(ICommand message)
        {
            return _messageService.ExecuteAsync(message);
        }

        public Task Handle(IEvent message)
        {
            return _messageService.PublishAsync(message);
        }
    }
}