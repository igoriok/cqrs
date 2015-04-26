using System.Threading.Tasks;
using Irdaf.Messaging.Services;
using Rebus;

namespace Irdaf.Messaging.Rebus
{
    public class RebusHandlerAsync<TResult> :
        IHandleMessagesAsync<IQuery<TResult>>,
        IHandleMessagesAsync<IEvent>,
        IHandleMessagesAsync<ICommand>
    {
        private readonly IBus _bus;
        private readonly IMessageServiceAsync _messageService;

        public RebusHandlerAsync(IBus bus, IMessageServiceAsync messageService)
        {
            _bus = bus;
            _messageService = messageService;
        }

        public async Task Handle(IQuery<TResult> message)
        {
            var result = await _messageService.QueryAsync(message);

            await Task.Run(() => _bus.Reply(result));
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