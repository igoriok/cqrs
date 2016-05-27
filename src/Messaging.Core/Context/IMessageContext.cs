namespace Irdaf.Messaging
{
    public interface IMessageContext
    {
        IMessage Message { get; }

        T Get<T>(string key);

        void Set<T>(string key, T value);
    }
}