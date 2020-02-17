namespace Irdaf.Messaging.Pipeline
{
    public interface IPipelineContext : IMessageContext
    {
        IMessage Message { get; }

        void Set(string key, object value);

        void Remove(string key);
    }
}