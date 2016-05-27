namespace Irdaf.Messaging
{
    public static class MessageContextExtensions
    {
        public static T Get<T>(this IMessageContext context)
        {
            return context.Get<T>(typeof(T).FullName);
        }

        public static void Set<T>(this IMessageContext context, T value)
        {
            context.Set(typeof(T).FullName, value);
        }
    }
}