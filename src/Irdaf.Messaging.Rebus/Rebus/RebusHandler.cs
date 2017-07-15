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

        public async Task Handle(ICommand message)
        {
            using (RebusContext.Assing(global::Rebus.Pipeline.MessageContext.Current))
            {
                await _messageService.ExecuteAsync(message).ConfigureAwait(false);
            }
        }

        public async Task Handle(IEvent message)
        {
            using (RebusContext.Assing(global::Rebus.Pipeline.MessageContext.Current))
            {
                await _messageService.PublishAsync(message).ConfigureAwait(false);
            }
        }
    }
}