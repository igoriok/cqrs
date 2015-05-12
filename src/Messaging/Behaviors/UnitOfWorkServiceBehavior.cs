using Irdaf.Messaging.Dispatch;

namespace Irdaf.Messaging.Behaviors
{
    public class UnitOfWorkServiceBehavior : IServiceBehavior
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private IUnitOfWork _unitOfWork;

        public UnitOfWorkServiceBehavior(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void BeforeDispatch(IMessageDispatcher dispatcher, IMessage message)
        {
            _unitOfWork = _unitOfWorkFactory.Create();
        }

        public void AfterDispatch(IMessageDispatcher dispatcher, IMessage message)
        {
            _unitOfWork.Dispose();
            _unitOfWork = null;
        }
    }
}