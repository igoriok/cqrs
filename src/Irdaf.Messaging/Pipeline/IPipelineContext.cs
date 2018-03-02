using Irdaf.Messaging.Pipeline.Convensions;

namespace Irdaf.Messaging.Pipeline
{
    public interface IPipelineContext : IMessageContext
    {
        IMessage Message { get; }

        IMessageConvention Convention { get; }

        void Set(string key, object value);

        void Remove(string key);
    }
}