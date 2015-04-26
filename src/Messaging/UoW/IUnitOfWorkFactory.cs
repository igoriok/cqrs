namespace Irdaf.Messaging
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}