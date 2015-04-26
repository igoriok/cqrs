namespace Irdaf.Messaging.Services
{
    public interface ICommandService
    {
        void Execute(ICommand command);
    }
}