using System;

namespace Irdaf.Messaging.Container
{
    public interface IContainer : IDisposable
    {
        object Build(Type handlerType);

        IContainer CreateChildContainer();
    }
}