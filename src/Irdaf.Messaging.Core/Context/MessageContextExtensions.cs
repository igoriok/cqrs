namespace Irdaf.Messaging
{
    public static class MessageContextExtensions
    {
        public static T Get<T>(this IMessageContext context, string key)
        {
            object value;

            if (context.Items.TryGetValue(key, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        public static T Get<T>(this IMessageContext context)
        {
            return context.Get<T>(typeof(T).FullName);
        }

        public static void Set<T>(this IMessageContext context, string key, T value)
        {
            if (value == null)
                context.Items.Remove(key);
            else
                context.Items[key] = value;
        }

        public static void Set<T>(this IMessageContext context, T value)
        {
            context.Set(typeof(T).FullName, value);
        }
    }
}