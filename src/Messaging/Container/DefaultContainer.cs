using System;

namespace Irdaf.Messaging.Container
{
    public class DefaultContainer : IContainer
    {
        public static readonly DefaultContainer Instance = new DefaultContainer();

        public object Build(Type handlerType)
        {
            return Activator.CreateInstance(handlerType);
        }

        public IContainer CreateChildContainer()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}