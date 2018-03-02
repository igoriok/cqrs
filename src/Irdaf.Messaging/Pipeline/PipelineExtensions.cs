namespace Irdaf.Messaging.Pipeline
{
    public static class PipelineExtensions
    {
        public static void Set<T>(this IPipelineContext context, string key, T value)
        {
            context.Set(key, value);
        }

        public static void Set<T>(this IPipelineContext context, T value)
        {
            context.Set(typeof(T).FullName, value);
        }

        public static void Remove<T>(this IPipelineContext context)
        {
            context.Remove(typeof(T).FullName);
        }
    }
}