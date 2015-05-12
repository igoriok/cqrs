using System.Threading;
using System.Threading.Tasks;

namespace Irdaf.Messaging.Behaviors
{
    public class UnitOfWorkServiceBehaviorAsync : IServiceBehaviorAsync
    {
        private readonly IUnitOfWorkFactoryAsync _unitOfWorkFactory;
        private IUnitOfWorkAsync _unitOfWork;

        public UnitOfWorkServiceBehaviorAsync(IUnitOfWorkFactoryAsync unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task BeforeDispatch(IMessage message, CancellationToken token)
        {
            _unitOfWork = await _unitOfWorkFactory.CreateAsync(token);
        }

        public async Task AfterDispatch(IMessage message, CancellationToken token)
        {
            _unitOfWork.Dispose();
            _unitOfWork = null;
        }
    }
}