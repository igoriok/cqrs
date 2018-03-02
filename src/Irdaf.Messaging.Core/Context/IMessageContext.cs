namespace Irdaf.Messaging
{
    public interface IMessageContext
    {
        object Get(string key);

        bool Contains(string key);
    }
}